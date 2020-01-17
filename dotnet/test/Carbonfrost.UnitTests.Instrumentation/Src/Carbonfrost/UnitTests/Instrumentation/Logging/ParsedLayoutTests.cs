//
// - ParsedLayoutTests.cs -
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
using Carbonfrost.Commons.Instrumentation;
using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Instrumentation.Logging {

    public class ParsedLayoutTests : ParsedLayoutTestBase {

        [Fact]
        public void time_variable_implies_time() {
            ParsedLayout pl = ParsedLayout.Parse("{Time}", LayoutMode.Default);
            string text = Body(pl, new LoggerEvent { TimeStamp = new DateTime(2000, 1, 1) });
            Assert.Equal("00:00:00", text);
        }

        [Fact]
        public void use_escaped_braces() {
            ParsedLayout pl = ParsedLayout.Parse("{{{Level:MMM}}} {TimeStamp:u} [{Thread,-10}] {Message}{NL}", LayoutMode.Default);
            string text = Body(pl, new LoggerEvent { TimeStamp = new DateTime(2000, 1, 1), Level = LoggerLevel.Warn, Message = "This is my message", ThreadName = "Main" });
            Assert.Equal("{Warn} 2000-01-01 00:00:00Z [Main      ] This is my message" + Environment.NewLine, text);
        }

        [Fact]
        public void use_bad_formats_ignored() {
            // TODO Should also apply to others
            // TODO SHould also parse bad string formats and just ignore them ( something {)
            ParsedLayout pl = ParsedLayout.Parse("{Level:MMM} {TimeStamp:u} [{Thread,-10}] {Message}{NL}", LayoutMode.Default);
            string text = Body(pl, new LoggerEvent { TimeStamp = new DateTime(2000, 1, 1), Level = LoggerLevel.Warn, Message = "This is my message", ThreadName = "Main" });
            Assert.Equal("Warn 2000-01-01 00:00:00Z [Main      ] This is my message" + Environment.NewLine, text);
        }

        [Fact]
        public void apply_formating() {
            ParsedLayout pl = ParsedLayout.Parse("{Level:C} {TimeStamp:u} [{Thread,-10}] {Message}{NL}", LayoutMode.Default);
            string text = Body(pl, new LoggerEvent { TimeStamp = new DateTime(2000, 1, 1), Level = LoggerLevel.Warn, Message = "This is my message", ThreadName = "Main" });
            Assert.Equal("W 2000-01-01 00:00:00Z [Main      ] This is my message" + Environment.NewLine, text);
        }

        [Fact]
        public void apply_simple_padding() {
            ParsedLayout pl = ParsedLayout.Parse("{Level,-5} {Thread} {Message}{NL}", LayoutMode.Default);
            string text = Body(pl, new LoggerEvent { Level = LoggerLevel.Warn, Message = "This is my message", ThreadName = "Main" });
            Assert.Equal("Warn  Main This is my message" + Environment.NewLine, text);
        }
    }
}
