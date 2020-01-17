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
using System.Globalization;

using Carbonfrost.Commons.Core;

namespace Carbonfrost.Commons.Instrumentation {

    public sealed partial class LoggerLevel : IComparable<LoggerLevel>, IEquatable<LoggerLevel>, IFormattable {

        private readonly string name;
        private readonly int value;
        internal static readonly char[] VALID_FORMATS = { 'c', 'g', 'v' };

        public static readonly LoggerLevel Off;
        public static readonly LoggerLevel Debug;
        public static readonly LoggerLevel Trace;
        public static readonly LoggerLevel Info;
        public static readonly LoggerLevel Warn;
        public static readonly LoggerLevel Error;
        public static readonly LoggerLevel Fatal;
        public static readonly LoggerLevel Verbose;

        private static LoggerLevel[] logLevels = {
            Verbose = new LoggerLevel("Verbose", 0),
            Debug = new LoggerLevel("Debug", 1),
            Trace = new LoggerLevel("Trace", 2),
            Info = new LoggerLevel("Info", 3),
            Warn = new LoggerLevel("Warn", 4),
            Error = new LoggerLevel("Error", 5),
            Fatal = new LoggerLevel("Fatal", 6),
            Off = new LoggerLevel("Off",  7),
        };

        internal static LoggerLevel[] AllValues { get { return logLevels; } }

        // Properties
        public string Name { get { return this.name; } }

        // Constructors
        private LoggerLevel(string name, int value) {
            this.name = name;
            this.value = value;
        }

        public static LoggerLevel FromValue(int value) {
            if (value < 0 || value >= logLevels.Length)
                return logLevels[value];
            else
                throw InstrumentationFailure.LogLevelOutOfRange("value", value);
        }

        public static bool TryParse(string text, out LoggerLevel value) {
            value = _TryParse(text, false);
            return (value != null);
        }

        public static LoggerLevel Parse(string text) {
            return _TryParse(text, true);
        }

        public static bool operator >(LoggerLevel l1, LoggerLevel l2) {
            return SafeValue(l1) > SafeValue(l2);
        }

        public static bool operator >=(LoggerLevel l1, LoggerLevel l2) {
            return SafeValue(l1) >= SafeValue(l2);
        }

        public static bool operator <(LoggerLevel l1, LoggerLevel l2) {
            return SafeValue(l1) < SafeValue(l2);
        }

        public static bool operator <=(LoggerLevel l1, LoggerLevel l2) {
            return SafeValue(l1) <= SafeValue(l2);
        }

        public static bool operator ==(LoggerLevel l1, LoggerLevel l2) {
            return EqualsImpl(l1, l2);
        }

        public static bool operator !=(LoggerLevel l1, LoggerLevel l2) {
            return !EqualsImpl(l1, l2);
        }

        public int ToInt32() { return this.value; }

        // object
        public override string ToString() {
            return this.Name;
        }

        public override int GetHashCode() {
            int hashCode = 0;
            unchecked {
                hashCode += 7 * name.GetHashCode();
                hashCode += 9 * value.GetHashCode();
            }
            return hashCode;
        }

        public override bool Equals(object obj) {
            LoggerLevel other = obj as LoggerLevel;
            return Equals(other);
        }

        // IEquatable
        public bool Equals(LoggerLevel other) {
            return EqualsImpl(this, other);
        }

        // IComparable
        public int CompareTo(LoggerLevel other) {
            if (other == null)
                throw new ArgumentNullException("other");

            return this.value.CompareTo(other.value);
        }

        // IFormattable
        public string ToString(string format) {
            return ToString(format, null);
        }

        public string ToString(string format, IFormatProvider formatProvider) {
            formatProvider = formatProvider ?? CultureInfo.CurrentCulture;
            char fmt;
            if (string.IsNullOrEmpty(format))
                fmt = 'g';
            else if (format.Length > 1)
                throw new FormatException();
            else
                fmt = format[0];

            switch (fmt) {
                case 'g':
                    return Name;
                case 'G':
                    return Name.ToUpperInvariant();
                case 'c':
                case 'C':
                    return Name[0].ToString();
                case 'v':
                case 'V':
                    return ToInt32().ToString();
                default:
                    throw new FormatException();
            }
        }

        static bool EqualsImpl(LoggerLevel a, LoggerLevel b) {
            if (object.ReferenceEquals(a, b))
                return true;
            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
                return false;
            else
                return (a.name == b.name && a.value == b.value);
        }

        static int SafeValue(LoggerLevel a) {
            if (a == null)
                return 0;
            else
                return a.value;
        }

        internal static LoggerLevel _TryParse(string text, bool throwError) {
            if (text == null) {
                if (throwError)
                    throw new ArgumentNullException("text");
                else
                    return null;
            }

            if (text.Length == 0) {
                if (throwError)
                    throw Failure.EmptyString("text");
                else
                    return null;
            }

            text = text.Trim();
            if (text.Length == 0) {
                if (throwError)
                    throw Failure.AllWhitespace("text");
                else
                    return null;
            }

            switch (text.ToLowerInvariant()) {
                case "off":
                    return LoggerLevel.Off;
                case "debug":
                    return LoggerLevel.Debug;
                case "trace":
                    return LoggerLevel.Trace;
                case "info":
                    return LoggerLevel.Info;
                case "warn":
                    return LoggerLevel.Warn;
                case "error":
                    return LoggerLevel.Error;
                case "fatal":
                    return LoggerLevel.Fatal;
                case "verbose":
                    return LoggerLevel.Verbose;
            }

            if (throwError)
                throw Failure.NotParsable("text", typeof(LoggerLevel));
            else
                return null;
        }

        public static LoggerLevel[] GetValues() {
            return (LoggerLevel[]) AllValues.Clone();
        }
    }

#if NET

    partial class LoggerLevel : IObjectReference {

        // IObjectReference
        object IObjectReference.GetRealObject(StreamingContext context) {
            return FromValue(this.value);
        }

    }

#endif

}
