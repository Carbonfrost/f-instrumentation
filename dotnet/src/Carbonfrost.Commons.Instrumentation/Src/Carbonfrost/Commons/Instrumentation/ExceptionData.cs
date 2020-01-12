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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Instrumentation {

    [XmlRoot("ExceptionData", Namespace = Xmlns.Instrumentation2008)]
    public class ExceptionData : IFormattable {

        private IDictionary<string, object> data;

        [XmlElement("data")]
        public IDictionary<string, object> Data {
            get {
                if (this.data == null)
                    this.data = new Dictionary<string, object>();
                return this.data;
            }
        }

        [XmlAttribute("helpLink")]
        public string HelpLink { get; set; }

        [XmlAttribute("hResult")]
        public int HResult { get; set; }

        [XmlElement("innerException")]
        public ExceptionData InnerException { get; set; }

        [XmlElement("exceptionType")]
        public TypeReference ExceptionType { get; set; }

        [XmlAttribute("message")]
        public string Message { get; set; }

        [XmlAttribute("source")]
        public string Source { get; set; }

        [XmlAttribute("stackTrace")]
        public string StackTrace { get; set; }

        internal static ExceptionData Create(Exception ex) {
            if (ex == null) return null;

            int hResult = 0;
            try {
                PropertyInfo pi = typeof(Exception).GetTypeInfo().GetProperty(
                    "HResult", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                hResult = (pi == null) ? 0 : (int) pi.GetValue(ex, null);

            } catch (AmbiguousMatchException) {
            } catch (MethodAccessException) {
            }

            ExceptionData result = new ExceptionData {
                HelpLink = ex.HelpLink,
                HResult = hResult,
                InnerException = Create(ex.InnerException),
                Message = ex.Message,
                Source = ex.Source,
                StackTrace = ex.StackTrace,
                ExceptionType = TypeReference.FromType(ex.GetType()),
            };

            foreach (DictionaryEntry de in ex.Data) {
                string s = de.Key as string;
                if (s == null) continue;

                result.Data.Add(s, de.Value);
            }
            return result;
        }

        public override string ToString() {
            // TODO Exact format like Exception.ToString() would be ideal here
            return string.Concat(Message, Environment.NewLine, StackTrace);
        }

        public string ToString(string format, IFormatProvider formatProvider = null) {
            if (string.IsNullOrEmpty(format))
                return ToString();

            if (format.Length != 1)
                throw new FormatException();

            switch (format[0]) {
                case 'G':
                case 'g':
                    return ToString();

                case 'M':
                case 'm':
                    return Message;

                default:
                    throw new FormatException();
            }
        }
    }
}
