//
// - TargetTests.cs -
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
using System.Collections.Generic;
using System.Linq;

using System.Runtime.InteropServices;
using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Instrumentation.Logging {

    public class TargetTests {

        [Fact]
        public void integration_with_providers() {
            var providers = new Dictionary<string, Type> {
                { "file", typeof(FileTarget) },
                { "memory", typeof(MemoryTarget) },
                { "console", typeof(ConsoleTarget) },
                { "null", typeof(NullTarget) },
            };

            Assert.True(App.GetProviderTypes().Contains(typeof(Target)));

            foreach (var kvp in providers) {
                Assert.Equal(kvp.Value, App.GetProviderType(typeof(Target), kvp.Key));
            }
        }
    }
}
