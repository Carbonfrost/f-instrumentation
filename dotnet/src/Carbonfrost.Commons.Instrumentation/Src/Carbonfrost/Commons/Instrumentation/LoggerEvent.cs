//
// - LoggerEvent.cs -
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
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using Carbonfrost.Commons.PropertyTrees;

namespace Carbonfrost.Commons.Instrumentation {

    public class LoggerEvent {

        private readonly List<object> _arguments = new List<object>();
        private PropertyTree _data;
        private const string EXCEPTION = "exception";

        [XmlIgnoreAttribute]
        public Logger SourceLogger {
            get; internal set;
        }

        [XmlElement("data")]
        public PropertyTree Data {
            get {
                if (_data == null) {
                    _data = new PropertyTree("data");
                }

                return _data;
            }
        }

        [XmlIgnore]
        public ExceptionData Exception {
            get { return Data.GetPropertyOrDefault(EXCEPTION, (ExceptionData) null); }
            set {
                if (value != null) {
                    Data.AppendTree(EXCEPTION).Value = value;
                }
            }
        }

        [XmlAttribute("source")]
        public string Source { get; set; }

        [XmlAttribute("level")]
        public LoggerLevel Level { get; set; }

        [XmlAttribute("threadName")]
        public string ThreadName { get; set; }

        [XmlAttribute("appDomainName")]
        public string AppDomainName { get; set; }

        [XmlAttribute("message")]
        public string Message { get; set; }

        [XmlAttribute("stackFrame")]
        public StackFrame StackFrame { get; set; }

        [XmlAttribute("timeStamp")]
        public DateTime TimeStamp { get; set; }

        [XmlElement("arguments")]
        public IList<object> Arguments {
            get {
                return _arguments;
            }
        }

        public string FormatMessage() {
            return FormatMessage(_arguments.ToArray());
        }

        public string FormatMessage(params object[] args) {
            return string.Format(this.Message, args ?? Empty<object>.Array);
        }

        public string FormatMessage(object arg) {
            return string.Format(this.Message, arg);
        }

        public string FormatMessage(object arg1, object arg2) {
            return string.Format(this.Message, arg1, arg2);
        }

        public string FormatMessage(object arg1, object arg2, object arg3) {
            return string.Format(this.Message, arg1, arg2, arg3);
        }

        internal void Capturing_(object data, Func<string, bool> capturePropertyFilter) {
            Capturing(data, capturePropertyFilter);
        }

        protected virtual void Capturing(object data, Func<string, bool> capturePropertyFilter) {
            if (data != null) {
                Data.Value = data;
            }
        }

        internal virtual void FinalizeTimestamp(DateTime loggerTime) {
            // Somewhat a hack, but set the desired time
            TimeStamp = loggerTime;
        }
    }
}
