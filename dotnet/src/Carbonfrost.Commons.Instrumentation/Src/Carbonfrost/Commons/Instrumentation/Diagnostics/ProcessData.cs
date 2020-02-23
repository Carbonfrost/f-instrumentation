//
// - ProcessData.cs -
//
// Copyright 2010, 2012 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    [XmlRoot("ProcessData", Namespace = Xmlns.Instrumentation2008)]
    public class ProcessData {

        [XmlAttribute("hasExited")]
        public bool HasExited { get; set; }

        [XmlAttribute("priorityBoostEnabled")]
        public bool PriorityBoostEnabled { get; set; }

        [XmlAttribute("responding")]
        public bool Responding { get; set; }

        [XmlAttribute("exitTime")]
        public DateTime ExitTime { get; set; }

        [XmlAttribute("startTime")]
        public DateTime StartTime { get; set; }

        [XmlAttribute("basePriority")]
        public int BasePriority { get; set; }

        [XmlAttribute("exitCode")]
        public int ExitCode { get; set; }

        [XmlAttribute("handleCount")]
        public int HandleCount { get; set; }

        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("sessionId")]
        public int SessionId { get; set; }

        [XmlAttribute("handle")]
        public IntPtr Handle { get; set; }

        [XmlElement("memoryUsage")]
        public MemoryUsageData MemoryUsage { get; set; }

        [XmlElement("processorUsage")]
        public ProcessorUsageData ProcessorUsage { get; set; }

        [XmlAttribute("mainWindowHandle")]
        public IntPtr MainWindowHandle { get; set; }

        [XmlAttribute("synchronizingObjectType")]
        public Type SynchronizingObjectType { get; set; }

        [XmlAttribute("priorityClass")]
        public ProcessPriorityClass PriorityClass { get; set; }

        [XmlAttribute("machineName")]
        public string MachineName { get; set; }

        [XmlAttribute("mainWindowTitle")]
        public string MainWindowTitle { get; set; }

        [XmlAttribute("processName")]
        public string ProcessName { get; set; }

        public ProcessData() {
        }

        internal static ProcessData Create(Process process) {
            if (process == null)
                return null;

            return new ProcessData {
                HasExited = process.HasExited,
                PriorityBoostEnabled = process.PriorityBoostEnabled,
                Responding = process.Responding,
                Handle = process.Handle,
                HandleCount = process.HandleCount,
                MainWindowHandle = process.MainWindowHandle,
                MainWindowTitle = process.MainWindowTitle,
                SynchronizingObjectType = (process.SynchronizingObject == null) ? null : process.SynchronizingObject.GetType(),
                ExitTime = process.ExitTime,
                StartTime = process.StartTime,
                BasePriority = process.BasePriority,
                ExitCode = process.ExitCode,
                Id = process.Id,
                SessionId = process.SessionId,
                ProcessorUsage = ProcessorUsageData.Create(process),
                MemoryUsage = MemoryUsageData.Create(process),
                MachineName = process.MachineName,
                ProcessName = process.ProcessName,
            };
        }

    }
}
