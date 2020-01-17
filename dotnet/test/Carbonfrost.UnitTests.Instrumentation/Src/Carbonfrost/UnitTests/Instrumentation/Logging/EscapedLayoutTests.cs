//
// - EscapedLayoutTests.cs -
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
using Carbonfrost.Commons.Instrumentation;
using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Instrumentation.Logging {

    public class EscapedLayoutTests : ParsedLayoutTests {

        [Fact]
        public void escaped_layout_nominal() {
            ParsedLayout pl = ParsedLayout.Parse("{Level:MMM} {Time} [{Thread}] {Message}", LayoutMode.Escaped);
            string text = Body(pl, new LoggerEvent { TimeStamp = new DateTime(2000, 1, 1), Level = LoggerLevel.Warn, Message = "This is my message", ThreadName = "Main" });
            Assert.Equal(@"Warn 00:00:00 [Main] ""This is my message""", text);
        }

        [Fact]
        public void escaped_layout_condense_whitespace() {
            ParsedLayout pl = ParsedLayout.Parse("{Level:MMM} \r\n{Time} \t\t\r[{Thread}] {Message}", LayoutMode.Escaped);
            string text = Body(pl, new LoggerEvent { TimeStamp = new DateTime(2000, 1, 1), Level = LoggerLevel.Warn, Message = "This is my message", ThreadName = "Main" });
            Assert.Equal(@"Warn 00:00:00 [Main] ""This is my message""", text);
        }

    }
}
