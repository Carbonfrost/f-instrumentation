//
// - ProfilerEventTest.cs -
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
using Carbonfrost.Commons.Instrumentation;
using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Instrumentation.Logging {

    public class ProfilerEventTests : ParsedLayoutTests {

        [Fact]
        public void Body_should_format_event_names() {
            ParsedLayout pl = ParsedLayout.Parse("{name}={Data.value}", LayoutMode.Default);
            string text = Body(pl, new ProfilerEvent {
                                   TimeStamp = new DateTime(2000, 1, 1),
                                   Level = LoggerLevel.Warn,
                                   ThreadName = "Main",
                                   Name = "cool",
                                   Value = "beans=420;down=20"
                               });
            Assert.Equal(@"cool=beans=420;down=20", text);
        }
    }
}
