//
// - MemoryUsageData.cs -
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
using System.Diagnostics;
using System.Xml.Serialization;

namespace Carbonfrost.Commons.Instrumentation.Diagnostics {

    public class MemoryUsageData {

        [XmlAttribute("maxWorkingSet")]
        public long MaxWorkingSet { get; set; }

        [XmlAttribute("minWorkingSet")]
        public long MinWorkingSet { get; set; }

        [XmlAttribute("nonpagedSystemMemorySize")]
        public long NonpagedSystemMemorySize { get; set; }

        [XmlAttribute("pagedMemorySize")]
        public long PagedMemorySize { get; set; }

        [XmlAttribute("pagedSystemMemorySize")]
        public long PagedSystemMemorySize { get; set; }

        [XmlAttribute("peakPagedMemorySize")]
        public long PeakPagedMemorySize { get; set; }

        [XmlAttribute("peakVirtualMemorySize")]
        public long PeakVirtualMemorySize { get; set; }

        [XmlAttribute("peakWorkingSet")]
        public long PeakWorkingSet { get; set; }

        [XmlAttribute("privateMemorySize")]
        public long PrivateMemorySize { get; set; }

        [XmlAttribute("virtualMemorySize")]
        public long VirtualMemorySize { get; set; }

        [XmlAttribute("workingSet")]
        public long WorkingSet { get; set; }

        internal static MemoryUsageData Create(Process p) {
            if (p == null)
                return null;

            return new MemoryUsageData {
                MaxWorkingSet = p.MaxWorkingSet.ToInt64(),
                MinWorkingSet = p.MinWorkingSet.ToInt64(),
                NonpagedSystemMemorySize = p.NonpagedSystemMemorySize64,
                PagedMemorySize = p.PagedMemorySize64,
                PagedSystemMemorySize = p.PagedSystemMemorySize64,

                PeakVirtualMemorySize = p.PeakVirtualMemorySize64,
                PeakWorkingSet = p.PeakWorkingSet64,
                PrivateMemorySize = p.PrivateMemorySize64,
                VirtualMemorySize = p.VirtualMemorySize64,
                WorkingSet = p.WorkingSet64,

            };
        }
    }
}
