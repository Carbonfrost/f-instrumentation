//
// - Logger.Static.cs -
//
// Copyright 2010 Carbonfrost Systems, Inc. (http://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Carbonfrost.Commons.Instrumentation.Configuration;
using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.Core;

namespace Carbonfrost.Commons.Instrumentation {

    partial class Logger {

        private static Logger _root = Logger.Null;
        private static readonly ConcurrentDictionary<string, Logger> cache = new ConcurrentDictionary<string, Logger>();
        private static volatile bool init;

        public static Logger Null { get { return new NullLogger(); } }

        public static Logger Root {
            get {
                Init();
                return _root.IsDisposed ? Logger.Null : _root;
            }
        }

        public static Logger System {
            get {
                return LogLogAdapter.Instance;
            }
        }

        public static Logger Create(Target target,
                                    LoggerFilter filter = null) {
            return new DefaultLogger(string.Empty, LoggerLevels.All, filter, target, null);
        }

        public static Logger FromName(string name) {
            if (name == null)
                throw new ArgumentNullException("name");
            if (name.Length == 0)
                throw Failure.EmptyString("name");
            if (name == System.Name)
                return System;

            Init();

            LoggerBuilder lb = InstrumentationConfiguration.Current.Loggers[name];
            return BuildLoggerSafe(lb, name);
        }

        public static Logger ForType(Type type) {
            if (type == null)
                throw new ArgumentNullException("type");

            return FromName(type.FullName);
        }

        public static Logger ForAssembly(Assembly assembly) {
            if (assembly == null) {
                throw new ArgumentNullException("assembly");
            }
            return FromName(assembly.GetName().Name);
        }

        public static Logger ForAssembly() {
            return FromName(Assembly.GetCallingAssembly().GetName().Name);
        }

        static Logger BuildLoggerSafe(LoggerBuilder lb, string name) {
            Logger result;
            if (cache.TryGetValue(name, out result)) {
                return result;
            }

            result = BuildLoggerSafeCore(lb, name);
            cache.TryAdd(name, result);
            return result;
        }

        static Logger BuildLoggerSafeCore(LoggerBuilder lb, string name) {
            if (lb != null) {
                try {
                    return lb.Build();

                } catch (Exception ex) {
                    Traceables.FailedToBuildLogger(name, ex);

                    if (Failure.IsCriticalException(ex)) {
                        throw;
                    }
                }
            }

            LoggerBuilder forward = LoggerBuilder.PickSourceForward(
                InstrumentationConfiguration.Current.Loggers, name);

            if (forward == null)
                return new NullLogger(name);

            var result = new DefaultLogger(name, new ForwardingTarget(forward.Name));
            result.Initialize();
            return result;
        }

        static void LeaveLogger(object o, EventArgs e) {
            _root.Dispose();

            AppDomain.CurrentDomain.DomainUnload -= LeaveLogger;
            AppDomain.CurrentDomain.ProcessExit -= LeaveLogger;
        }

        static void Init() {
            if (init)
                return;

            init = true;
            int loggerCount = InstrumentationConfiguration.Current.Loggers.Count;
            Traceables.RootLoggerInitializing(loggerCount);

            AppDomain.CurrentDomain.ProcessExit += LeaveLogger;
            AppDomain.CurrentDomain.DomainUnload += LeaveLogger;

            // Break cycles in forwarding
            var builders = InstrumentationConfiguration.Current.Loggers;

            foreach (var lb in builders) {
                lb.ComputeParent(builders);

                Logger logger = BuildLoggerSafe(lb, lb.Name);
                bool hasParent = !string.IsNullOrWhiteSpace(lb.Parent);

                if (hasParent) {
                    if (lb.Name == "root") {
                        Traceables.RootCannotHaveParent();
                    }
                    if (!(lb.Target is NullTarget || lb.Target is ForwardingTarget)) {
                        Traceables.CannotSpecifyParentAndTarget(lb.Name);
                    }
                }

                CheckParents(builders, lb, logger);
            }

            _root = cache.GetValueOrDefault("root", Logger.Null);

            // Clone list to prevent concurrent modification in
            // ForwardingTarget
            foreach (var log in cache.Values.ToArray()) {
                log.Initialize();
            }

            Traceables.RootLoggerDoneInitializing();
        }

        static void CheckParents(LoggerBuilderCollection builders,
                                 LoggerBuilder lb, Logger logger) {

            if (lb.ComputeParent(null).Length == 0) {
                return;
            }

            HashSet<string> ancestors = new HashSet<string>();
            LoggerBuilder current = lb;
            StringBuilder forwards = new StringBuilder(current.Name);

            while (current != null) {
                if (!ancestors.Add(current.ComputeName())) {
                    Traceables.DetectedCycleInLogParents(forwards.ToString());
                    logger.ForwardToParentBlocked = true;
                    break;
                }
                string parent = current.ComputeParent(null);
                if (parent.Length == 0)
                    break;

                // Detect missing parents
                current = builders[parent];
                if (current == null) {
                    Traceables.MissingParentLoggerReference(lb.Name);
                    break;
                }

                forwards.Append(" -> " + parent);
            }
        }
    }

}
