//
// - MemoryTarget.cs -
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
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Carbonfrost.Commons.Instrumentation.Configuration;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Instrumentation.Logging {

    [Builder(typeof(MemoryTargetBuilder))]
    [TargetUsage(Name = "memory")]
    public class MemoryTarget : Target {

        private List<LoggerEvent> events = new List<LoggerEvent>();

        public int MaxEventCount { get; private set; }
        public long MaxMemory { get; private set; }

        public ReadOnlyCollection<LoggerEvent> Events {
            get { return events.AsReadOnly(); }
        }

        // TODO Support memory target settings

        internal MemoryTarget(MemoryTargetBuilder builder) {
            if (builder != null) {
                this.MaxEventCount = builder.MaxEventCount;
                this.MaxMemory = builder.MaxMemory;
            }
        }

        public override void Write(LoggerEvent loggerEvent) {
            events.Add(loggerEvent);
        }

        public override void Flush(int timeout) {}

    }
}
