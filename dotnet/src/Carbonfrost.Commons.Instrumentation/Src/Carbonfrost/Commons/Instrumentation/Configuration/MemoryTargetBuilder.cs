//
// - MemoryTargetBuilder.cs -
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
using System.ComponentModel;
using Carbonfrost.Commons.Instrumentation.Logging;

namespace Carbonfrost.Commons.Instrumentation.Configuration {

    public class MemoryTargetBuilder : TargetBuilder {

        [DefaultValue(100 * 1000)]
        public int MaxEventCount { get; set; }

        [DefaultValue(10 * 1024 * 1024)]
        public long MaxMemory { get; set; }

        public MemoryTargetBuilder() {
            this.MaxEventCount = 100 * 1000;
            this.MaxMemory = 10 * 1024 * 1024;
        }

        public override Target Build(IServiceProvider serviceProvider) {
            return new MemoryTarget(this);
        }
    }
}
