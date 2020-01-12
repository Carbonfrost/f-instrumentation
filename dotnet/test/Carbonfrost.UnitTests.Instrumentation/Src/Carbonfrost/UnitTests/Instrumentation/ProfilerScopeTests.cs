//
// Copyright 2013, 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Instrumentation.Logging {

    public class ProfilerScopeTests {

        [Fact]
        public void LoggerProfiling_should_generate_nested_scope() {
            MemoryTarget target = new MemoryTarget(null);

            using (var ps = Logger.Create(target).Profiling("child")) {
                ps.AddMetric("cool", 420);
            }

            // Only one metric implies that child.cool=420 should be
            // emitted
            var evt = (ProfilerEvent) target.Events[0];
            Assert.Equal("child.cool", evt.Name);
            Assert.Equal("420", evt.Value);
        }

        [Fact]
        public void Dispose_should_generate_timing() {
            MemoryTarget target = new MemoryTarget(null);

            using (var ps = Logger.Create(target).Profiling("child")) {
            }

            var evt = (ProfilerEvent) target.Events[0];
            Assert.Equal("child.duration", evt.Name);
            Assert.Matches(@"\d+", evt.Value);
        }

        [Fact]
        public void Should_consolidate_multiple_metrics_into_one() {
            MemoryTarget target = new MemoryTarget(null);

            using (var ps = Logger.Create(target).Profiling("child")) {
                ps.AddMetric("cool", 420);
                ps.AddMetric("beans", 20);
            }

            var evt = (ProfilerEvent) target.Events[0];
            Assert.Equal("child", evt.Name);
            Assert.Equal("cool=420;beans=20", evt.Value);
        }

        [Fact]
        public void Should_emit_multiple_events_per_metric_with_different_times() {
            MemoryTarget target = new MemoryTarget(null);

            using (var ps = Logger.Create(target).Profiling("child")) {
                var now = DateTime.UtcNow;
                ps.AddMetric("cool", 420, now);
                ps.AddMetric("beans", 20, now.AddMilliseconds(20));
            }

            // Duration element is implicitly emitted
            Assert.Equal(3, target.Events.Count);

            // Should contain both (but we don't know which order)
            var values = string.Join(", ",
                target.Events.Cast<ProfilerEvent>().Select(t => string.Format("{0}={1}", t.Name, t.Value)));

            Assert.Contains("child.cool=420", values);
            Assert.Contains("child.beans=20", values);
        }
    }

}
