//
// Copyright 2010, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

using Carbonfrost.Commons.Core;

namespace Carbonfrost.Commons.Instrumentation {

    public class LoggerLevels : LoggerLevelMap<bool>, IEquatable<LoggerLevels> {

        public static readonly new LoggerLevels Debug = new LoggerLevels(LoggerLevel.Debug);
        public static readonly new LoggerLevels Error = new LoggerLevels(LoggerLevel.Error);
        public static readonly new LoggerLevels Fatal = new LoggerLevels(LoggerLevel.Fatal);
        public static readonly new LoggerLevels Info = new LoggerLevels(LoggerLevel.Info);
        public static readonly new LoggerLevels Trace = new LoggerLevels(LoggerLevel.Trace);
        public static readonly new LoggerLevels Verbose = new LoggerLevels(LoggerLevel.Verbose);
        public static readonly new LoggerLevels Warn = new LoggerLevels(LoggerLevel.Warn);

        public static readonly new LoggerLevels All = new LoggerLevels(true);
        public static readonly LoggerLevels None = new LoggerLevels(false);
        public static readonly new LoggerLevels Off = None;

        public LoggerLevels(IEnumerable<LoggerLevel> values) {
            if (values == null) {
                throw new ArgumentNullException("values");
            }

            foreach (var v in values)
                this[v] = true;

            MakeReadOnly();
        }

        public LoggerLevels(LoggerLevelMap<bool> copyFrom)
            : base(copyFrom) {

            MakeReadOnly();
        }

        public LoggerLevels(params LoggerLevel[] values) {
            if (values == null) {
                throw new ArgumentNullException("values");
            }

            foreach (var v in values) {
                this[v] = true;
            }

            MakeReadOnly();
        }

        private LoggerLevels(bool defaultValue) : base(defaultValue) {
            MakeReadOnly();
        }

        public LoggerLevels(LoggerLevel singleton) : base(false) {
            this[singleton] = true;

            MakeReadOnly();
        }

        public static LoggerLevels Exactly(LoggerLevel level) {
            return new LoggerLevels(level);
        }

        public static LoggerLevels Above(LoggerLevel level) {
            if (level == null) {
                throw new ArgumentNullException("level");
            }

            return new LoggerLevels(
                LoggerLevel.AllValues.Where(l => l >= level));
        }

        public static LoggerLevels Below(LoggerLevel level) {
            if (level == null) {
                throw new ArgumentNullException("level");
            }

            return new LoggerLevels(
                LoggerLevel.AllValues.Where(l => l <= level));
        }

        public static LoggerLevels Parse(string text) {
            LoggerLevels result;
            Exception ex = _TryParse(text, out result);
            if (ex == null) {
                return result;
            }

            throw ex;
        }

        public static bool TryParse(string text, out LoggerLevels result) {
            return _TryParse(text, out result) == null;
        }

        internal IEnumerable<LoggerLevel> TrueValues() {
            using (IEnumerator<KeyValuePair<LoggerLevel, bool>> e = GetEnumerator()) {
                while (e.MoveNext()) {
                    if (e.Current.Value)
                        yield return e.Current.Key;
                }
            }
        }

        public override string ToString() {
            LoggerLevel[] t = TrueValues().ToArray();
            if (t.Length == 0) {
                return "None";
            }
            if (t.Length == Keys.Count) {
                return "All";
            }

            return string.Join(",", TrueValues());
        }

        public new LoggerLevels Clone() {
            return new LoggerLevels(this);
        }

        protected override LoggerLevelMap<bool> CloneCore() {
            return Clone();
        }

        public bool Contains(LoggerLevel level) {
            if (level == null) {
                throw new ArgumentNullException("level");
            }

            return this[level];
        }

        public bool IsMatch(LoggerLevel value) {
            return IsMatch(value, null);
        }

        public bool IsMatch(LoggerLevel value, IServiceProvider serviceProvider) {
            if (value == null) {
                throw new ArgumentNullException("value");
            }

            return this[value];
        }

        public bool Equals(LoggerLevels other) {
            if (other == null) {
                return false;
            }

            return other.Values.SequenceEqual(Values);
        }

        static Exception _TryParse(string text, out LoggerLevels result) {
            result = null;
            if (string.IsNullOrWhiteSpace(text)) {
                return Failure.NotParsable("text", typeof(LoggerLevels));
            }

            text = text.Trim();
            if (text == "*" || text == "All") {
                result = LoggerLevels.All;
                return null;

            } else if (text == "None" || text == "Off") {
                result = LoggerLevels.None;
                return null;

            } else if (text[0] == ':') {
                LoggerLevel l;
                if (LoggerLevel.TryParse(text.Substring(1), out l)) {
                    result = LoggerLevels.Below(l);
                    return null;
                } else {
                    return Failure.NotParsable("text", typeof(LoggerLevels));
                }

            } else if (text[text.Length - 1] == ':') {
                LoggerLevel l;
                if (LoggerLevel.TryParse(text.Substring(0, text.Length - 1), out l)) {
                    result = LoggerLevels.Above(l);
                    return null;
                }

                return Failure.NotParsable("text", typeof(LoggerLevels));
            }

            string[] levels = text.Split(new char[] { ',', ';', ' ', '\n', '\t', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            result = new LoggerLevels();
            foreach (var s in levels) {
                LoggerLevel ll = LoggerLevel._TryParse(s.Trim(), false);
                if (ll == null)
                    return Failure.NotParsable("text", typeof(LoggerLevels));

                result[ll] = true;
            }

            return null;
        }

    }
}
