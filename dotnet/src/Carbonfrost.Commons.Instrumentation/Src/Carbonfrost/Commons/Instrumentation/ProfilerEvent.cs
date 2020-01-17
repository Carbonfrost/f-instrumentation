//
// - ProfilerEvent.cs -
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
using System.Xml.Serialization;

namespace Carbonfrost.Commons.Instrumentation {

    [XmlRoot(Xmlns.Instrumentation2008)]
    public class ProfilerEvent : LoggerEvent {

        [XmlAttribute("value")]
        public string Value {
            get { return Data.GetPropertyOrDefault("value", string.Empty); }
            set { Data.SetProperty("value", value); }
        }

        [XmlAttribute("name")]
        public string Name {
            get {return Data.GetPropertyOrDefault("name", string.Empty); }
            set { Data.SetProperty("name", value); }
        }

        internal override void FinalizeTimestamp(DateTime loggerTime) {
            // Don't use the logger's timestamp -- we guarantee our own
        }

        internal void SetProfilerTimeStamp(DateTime endTime) {
            var diff = endTime - TimeStamp;
            Value = ((long) diff.TotalMilliseconds).ToString(CultureInfo.InvariantCulture);
        }
    }
}
