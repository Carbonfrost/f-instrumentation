//
// - LoggerEventTests.cs -
//
// Copyright 2015 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Linq;
using Carbonfrost.Commons.Instrumentation;
using Carbonfrost.Commons.Instrumentation.Configuration;
using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Instrumentation {

    public class LoggerEventTests {

        class MyEvent : LoggerEvent {

            public int Count { get; set; }

            protected override void Capturing(object data, Func<string, bool> capturePropertyFilter) {
                if (capturePropertyFilter("Data")) {
                    Count++;
                }
            }
        }

        [Fact]
        public void Capturing_should_allow_customization_of_output_data() {
            var target = new ConsoleTargetBuilder {
                Layout = "{Time} {Data}"
            }.Build();

            // Because {Data} is present
            DefaultLogger logger = new DefaultLogger(null, target);
            var evt = new MyEvent();
            logger.Info(evt);
            Assert.Equal(1, evt.Count);
        }

        [Fact]
        public void Capturing_should_allow_customization_of_output_data_not_specified() {
            var target = new ConsoleTargetBuilder {
                Layout = "{Time}"
            }.Build();

            DefaultLogger logger = new DefaultLogger(null, target);
            var evt = new MyEvent();
            logger.Info(evt);
            Assert.Equal(0, evt.Count);
        }
    }
}
