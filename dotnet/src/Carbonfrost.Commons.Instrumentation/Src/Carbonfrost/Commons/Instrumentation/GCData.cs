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
using System.Runtime;
using System.Xml;
using System.Xml.Serialization;

namespace Carbonfrost.Commons.Instrumentation {

    [XmlRoot("GCData", Namespace = Xmlns.Instrumentation2008)]
    public class GCData {

        [XmlAttribute("isServerGC")]
        public bool IsServerGC { get; set; }

        [XmlAttribute("maxGeneration")]
        public int MaxGeneration { get; set; }

        [XmlAttribute("latencyMode")]
        public GCLatencyMode LatencyMode { get; set; }

        internal static GCData Create() {
            return new GCData {
                IsServerGC = GCSettings.IsServerGC,
                LatencyMode = GCSettings.LatencyMode,
                MaxGeneration = GC.MaxGeneration,
            };
        }
    }
}
