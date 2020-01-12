//
// - ProfilerTests.cs -
//
// Copyright 2013 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Threading;
using Carbonfrost.Commons.Instrumentation;
using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Instrumentation.Logging {

    public class ProfilerTests {

        [Fact]
        public void AddMetric_should_emit_at_info_level() {
            var now = DateTime.UtcNow;
            var evt = RunProfiler(pro => pro.AddMetric("renderIslandCount", 832, now));

            Assert.Equal("832", evt.Value);
            Assert.Equal("renderIslandCount", evt.Name);
            Assert.Equal(now, evt.TimeStamp);
        }

        [Fact]
        public void AddMetric_name_time_nominal() {
            var now = DateTime.UtcNow;
            var evt = RunProfiler(pro => pro.AddMetric("renderIslandCount", 832, now));

            Assert.Equal("832", evt.Value);
            Assert.Equal("renderIslandCount", evt.Name);
            Assert.Equal(now, evt.TimeStamp);
        }

        [Fact]
        public void Timing_should_emit_timing_and_name() {
            DateTime date = DateTime.MinValue;
            var evt = RunProfiler(
                pro => {

                    date = DateTime.UtcNow;
                    using (pro.Timing("compile")) {
                        Thread.Sleep(1000);
                    }
                });

            // The TimeStamp should be set equal to the starting time
            Assert.GreaterThanOrEqualTo(1000, Int32.Parse(evt.Value));
            Assert.Equal("compile", evt.Name);
            // TODO Assert.Equal(date, evt.TimeStamp).Within(100ms);
        }

        static ProfilerEvent RunProfiler(Action<Profiler> thunk) {
            var target = new MemoryTarget(null);
            var pro = Profiler.ForLogger(Logger.Create(target));
            thunk(pro);

            return (ProfilerEvent) target.Events[0];

        }
    }
}
