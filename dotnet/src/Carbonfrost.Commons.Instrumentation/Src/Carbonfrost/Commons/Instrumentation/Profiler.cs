//
// Copyright 2012 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Globalization;
using System.Reflection;

namespace Carbonfrost.Commons.Instrumentation {

    public abstract class Profiler : IProfilerScope {

        private readonly Logger _logger;
        private readonly ProfilerTimingHelper _timings = new ProfilerTimingHelper();

        protected Logger Logger {
            get {
                return _logger;
            }
        }

        public static Profiler Null {
            get {
                return new NullProfiler();
            }
        }

        public static Profiler Root {
            get {
                return Logger.Root.Profiler;
            }
        }

        internal Profiler(Logger logger) {
            _logger = logger;
        }

        public static Profiler ForLogger(Logger logger) {
            if (logger == null)
                throw new ArgumentNullException("logger");

            return logger.Profiler;
        }

        public static Profiler FromName(string name) {
            return ForLogger(Logger.FromName(name));
        }

        public static Profiler ForType(Type type) {
            return ForLogger(Logger.ForType(type));
        }

        public static Profiler ForAssembly(Assembly assembly) {
            return ForLogger(Logger.ForAssembly(assembly));
        }

        public static Profiler ForAssembly() {
            var assembly = Assembly.GetCallingAssembly();
            return ForLogger(Logger.ForAssembly(assembly));
        }

        void IDisposable.Dispose() {}

        internal bool ShouldProfile() {
            return _logger.InfoEnabled;
        }

        public void AddTime(string name,
                            TimeSpan duration,
                            DateTime? time = default(DateTime?))
        {
            AddTime(name, (long) duration.TotalMilliseconds, time);
        }

        public void AddTime(string name,
                            long durationMillis,
                            DateTime? time = default(DateTime?))
        {
            AddMetric(name, (double) durationMillis, time);
        }

        public void AddMetric(string name,
                              int value,
                              DateTime? time = default(DateTime?))
        {
            AddMetricCore(name,
                          value.ToString(CultureInfo.InvariantCulture),
                          time);
        }

        public void AddMetric(string name,
                              bool value,
                              DateTime? time = default(DateTime?))
        {
            AddMetricCore(name, value ? "1" : "0", time);
        }

        public void AddMetric(string name,
                              decimal value,
                              DateTime? time = default(DateTime?))
        {
            AddMetricCore(name, value.ToString(CultureInfo.InvariantCulture), time);
        }

        public void AddMetric(string name,
                              double value,
                              DateTime? time = default(DateTime?))
        {
            AddMetricCore(name, value.ToString(CultureInfo.InvariantCulture), time);
        }

        public void AddMetric(string name,
                              string value,
                              DateTime? time = default(DateTime?))
        {
            AddMetricCore(name, value, time);
        }

        public void AddMetric(string name,
                              Enum value,
                              DateTime? time = default(DateTime?))
        {
            AddMetricCore(name, value.ToString(), time);
        }

        public IDisposable Timing(string name) {
            return ProfilerScope.NewTiming(this, name);
        }

        public IProfilerScope Profiling(string name) {
            return new ProfilerScope(this, name);
        }

        internal abstract void AddMetricCore(string name, string str, DateTime? time);

        public void MarkStart(string name) {
            _timings.MarkStart(name);
        }

        public void MarkEnd() {
            MarkEnd(_timings.MarkEnd());
        }

        internal void MarkEnd(LoggerEvent evt) {
            if (_logger.InfoEnabled) {
                _logger.Info(evt);
            }
        }

        public IProfilerScope NewScope(string name) {
            return new ProfilerScope(this, name);
        }
    }
}
