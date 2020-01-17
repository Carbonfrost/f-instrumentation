//
// Copyright 2012, 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Globalization;
using System.Text;

namespace Carbonfrost.Commons.Instrumentation {

    public class ProfilerScope : IProfilerScope {

        private readonly Profiler _parent;
        private string _name;
        private readonly IList<string> _keys = new List<string>(32);
        private readonly IList<string> _values = new List<string>(32);
        private readonly ProfilerTimingHelper _timings = new ProfilerTimingHelper();
        private DateTime _lastTime;

        public static readonly IProfilerScope Null = new NullImpl();

        public string Name { get { return _name; } }

        internal ProfilerScope(Profiler parent, string name) {
            _parent = parent;
            _name = name;

            // Implicit scope timing
            // TODO Probably need to support disabling scope timing
            _timings.MarkStart(GetFullName("duration"));
            _lastTime = DateTime.UtcNow;
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

        public IDisposable Timing(string name = null) {
            return NewTiming(this, name);
        }

        public void MarkStart(string name = null) {
            _timings.MarkStart(name);
        }

        public void MarkEnd() {
            _parent.MarkEnd(_timings.MarkEnd());
        }

        private string GetFullName(string name) {
            return string.Concat(Name, ".", name);
        }

        private void AddMetricCore(string name, string str, DateTime? time) {
            if (time.HasValue && time.Value > _lastTime) {
                Flush();
            }
            _keys.Add(name);
            _values.Add(str);
        }

        IProfilerScope IProfilerScope.NewScope(string name) {
            return this;
        }

        internal static IDisposable NewTiming(IProfilerScope self, string name) {
            self.MarkStart(name);
            return new DisposableAction(self.MarkEnd);
        }

        public void Dispose() {
            Flush();
            foreach (var e in _timings.MarkEndAll())
                _parent.MarkEnd(e);
        }

        private string FormatValues() {
            var sb = new StringBuilder();
            bool comma = false;

            for (int i = 0; i < _values.Count; i++) {
                if (comma)
                    sb.Append(";");

                sb.Append(_keys[i]);
                sb.Append("=");
                sb.Append(_values[i]);
                comma = true;
            }

            return sb.ToString();
        }

        private void Flush() {
            if (_values.Count == 0)
                return;

            if (_values.Count == 1) {
                _parent.AddMetricCore(GetFullName(_keys[0]), _values[0], _lastTime);
            } else {
                _parent.AddMetricCore(Name, FormatValues(), _lastTime);
            }

            _keys.Clear();
            _values.Clear();
            _lastTime = DateTime.UtcNow;
        }

        sealed class NullImpl : IProfilerScope {
            void IProfilerScope.AddTime(string name, TimeSpan duration, DateTime? time) {}
            void IProfilerScope.AddTime(string name, long durationMillis, DateTime? time) {}
            void IProfilerScope.AddMetric(string name, int value, DateTime? time) {}
            void IProfilerScope.AddMetric(string name, bool value, DateTime? time) {}
            void IProfilerScope.AddMetric(string name, decimal value, DateTime? time) {}
            void IProfilerScope.AddMetric(string name, double value, DateTime? time) {}
            void IProfilerScope.AddMetric(string name, string value, DateTime? time) {}
            void IProfilerScope.AddMetric(string name, Enum value, DateTime? time) {}
            void IProfilerScope.MarkStart(string name) {}
            void IProfilerScope.MarkEnd() {}

            IDisposable IProfilerScope.Timing(string name) {
                return this;
            }

            IProfilerScope IProfilerScope.NewScope(string name) {
                return this;
            }
            void IDisposable.Dispose() {}
        }
    }
}
