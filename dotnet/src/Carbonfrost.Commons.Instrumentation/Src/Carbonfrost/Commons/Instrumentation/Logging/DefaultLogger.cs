//
// - DefaultLogger.cs -
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
using System.Collections.Generic;
using System.Linq;
using Carbonfrost.Commons.Instrumentation.Configuration;
using Carbonfrost.Commons.Instrumentation.Logging;

namespace Carbonfrost.Commons.Instrumentation {

    sealed class DefaultLogger : Logger {

        private readonly object _lock = new object();
        private readonly SuspendTracker _suspend = new SuspendTracker();
        private readonly string name;
        private readonly LoggerFilter filter;
        private readonly Profiler profiler;
        private readonly LoggerLevels levels;

        internal Target target;

        internal InstrumentContext Instruments { get; set; }

        public DefaultLogger(string name, Target target)
            : this(name, LoggerLevels.All, null, target, null) {
        }

        public DefaultLogger(string name,
                             LoggerLevels levels,
                             LoggerFilter filter,
                             Target target,
                             ProfilerBuilder profiler) {

            if (target == null)
                throw new ArgumentNullException("target");

            this.levels = levels ?? LoggerLevels.All;
            this.filter = filter ?? LoggerFilter.All;
            this.name = name;
            this.target = target;
            this.target.Logger = this;

            if (profiler == null || profiler.Enabled) {
                this.profiler = new DefaultProfiler(this);
            } else {
                this.profiler = Profiler.Null;
            }
        }

        internal override Profiler Profiler {
            get {
                return profiler;
            }
        }

        internal override FileTarget File {
            get { return this.target as FileTarget; }
        }

        internal override Logger Parent {
            get {
                ForwardingTarget f = this.target as ForwardingTarget;
                return f == null ? null : f.Parent;
            }
        }

        public override string Name { get { return name; } }

        public override bool IsSuspended {
            get {
                return _suspend.IsSuspended;
            }
        }

        internal override void Initialize() {
            this.target.Initialize();
        }

        protected override void Dispose(bool manualDispose) {
            if (manualDispose) {
                this.target.Flush();
                this.target.Dispose();
                Utility.Dispose(this.filter);
            }

            base.Dispose(manualDispose);
        }

        public override void Resume() {
            _suspend.Resume();
        }

        public override void Suspend() {
            _suspend.Suspend();
        }

        protected override bool ShouldLog(LoggerLevel loggerLevel) {
            // Fast consideration of level
            return base.ShouldLog(loggerLevel) && levels.IsMatch(loggerLevel);
        }

        protected override bool ShouldCapture(LoggerEvent loggerEvent) {
            return filter.ShouldCapture(loggerEvent);
        }

        protected override void Capture(LoggerEvent loggerEvent) {
            if (loggerEvent.Level == null)
                throw new NotImplementedException();
            RunInspectors(loggerEvent);
            try {
                target.Write(loggerEvent);

            } catch (Exception ex) {
                Traceables.FinalCaptureFailed(ex);
                target = Target.Null;
            }
        }

        internal override IEnumerable<string> FindOutputDataKeys() {
            // HACK This is hackish -- need a way to find the expressions outside of this asm
            var hasLayout = target as IHasParsedLayout;
            if (hasLayout == null) {
                return Empty<string>.Array;
            } else {
                return hasLayout.ParsedLayout.DataExpressions;
            }
        }

        void RunInspectors(LoggerEvent evt) {
            if (Instruments != null)
                Instruments.RunInstruments(evt);
        }

    }
}
