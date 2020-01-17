//
// - FileNameLayoutTests.cs -
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
using Carbonfrost.Commons.Instrumentation;
using Carbonfrost.Commons.Instrumentation.Configuration;
using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.Spec;
using Carbonfrost.UnitTests.Instrumentation.Logging;

namespace Carbonfrost.UnitTests.Instrumentation.Logging {

    public class FileNameLayoutTests : ParsedLayoutTestBase {

        [Fact]
        public void should_escape_invalid_filename_chars() {
            ParsedLayout pl = ParsedLayout.ParseFileName("/var/logs/{TimeStamp}{NL}.log");

            FileTarget file = (new FileTargetBuilder()).Build() as FileTarget;
            Logger logger = Logger.Create(file);

            string text = Body(pl, new LoggerEvent { SourceLogger = logger });

            Assert.Equal("/var/logs/0001-01-01_00_00_00Z_.log", text);
        }

        [Fact]
        public void should_escape_invalid_filename_chars_windows() {
            ParsedLayout pl = ParsedLayout.ParseFileName("C:\\temp\\logs\\{TimeStamp}{NL}.log");

            FileTarget file = (new FileTargetBuilder()).Build() as FileTarget;
            Logger logger = Logger.Create(file);

            string text = Body(pl, new LoggerEvent { SourceLogger = logger });

            Assert.Equal("C:\\temp\\logs\\0001-01-01_00_00_00Z_.log", text);
        }

        [Fact]
        public void Body_should_expand_environment_variables() {
            Environment.SetEnvironmentVariable("MYHOME", "Hello");
            ParsedLayout pl = ParsedLayout.ParseFileName("%MYHOME%/the.log");

            FileTarget file = (new FileTargetBuilder()).Build() as FileTarget;
            Logger logger = Logger.Create(file);

            string text = Body(pl, new LoggerEvent { SourceLogger = logger });

            Assert.Equal("Hello/the.log", text);
        }
    }
}
