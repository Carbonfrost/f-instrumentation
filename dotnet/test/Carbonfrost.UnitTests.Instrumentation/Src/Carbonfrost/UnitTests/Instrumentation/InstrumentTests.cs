//
// - InstrumentTests.cs -
//
// Copyright 2010, 2013 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

using Carbonfrost.Commons.Instrumentation;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Instrumentation {

    public class InstrumentTests {

        // HACK These currently don't test much since only simple accessors
        // are used, but they do help with code coverage and sanity

        [Fact]
        public void environment_data_capture() {
            EnvironmentInstrument ins = new EnvironmentInstrument();
            ins.Capture(new LoggerEvent());
        }

        [Fact]
        public void gc_data_capture() {
            GCInstrument ins = new GCInstrument();
            ins.Capture(new LoggerEvent());
        }

        [Fact]
        public void integration_with_providers() {
            var providers = new Dictionary<string, Type> {
                { "gc", typeof(GCInstrument) },
                { "environment", typeof(EnvironmentInstrument) },
                { "memory", typeof(MemoryInstrument) },
            };

            Assert.True(App.GetProviderTypes().Contains(typeof(Instrument)));
            foreach (var kvp in providers) {

                Assert.Equal(kvp.Value, App.GetProviderType(typeof(Instrument), kvp.Key));
            }
        }

    }

}
