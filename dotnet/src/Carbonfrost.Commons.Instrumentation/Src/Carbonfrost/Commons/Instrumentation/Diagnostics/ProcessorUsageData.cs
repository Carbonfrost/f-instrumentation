//
// - ProcessorUsageData.cs -
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
using System.Diagnostics;
using System.Xml.Serialization;

namespace Carbonfrost.Commons.Instrumentation.Diagnostics {

    [XmlRoot(Xmlns.Instrumentation2008)]
    public class ProcessorUsageData {

        [XmlAttribute("privilegedProcessorTime")]
        public TimeSpan PrivilegedProcessorTime { get; set; }

        [XmlAttribute("processorAffinity")]
        public long ProcessorAffinity { get; set; }

        [XmlAttribute("totalProcessorTime")]
        public TimeSpan TotalProcessorTime { get; set; }

        [XmlAttribute("userProcessorTime")]
        public TimeSpan UserProcessorTime { get; set; }

        internal static ProcessorUsageData Create(Process process) {
            ProcessorUsageData result = new ProcessorUsageData {
                ProcessorAffinity = process.ProcessorAffinity.ToInt64(),
                PrivilegedProcessorTime = process.PrivilegedProcessorTime,
                TotalProcessorTime = process.TotalProcessorTime,
                UserProcessorTime = process.UserProcessorTime,
            };
            return result;
        }
    }
}
