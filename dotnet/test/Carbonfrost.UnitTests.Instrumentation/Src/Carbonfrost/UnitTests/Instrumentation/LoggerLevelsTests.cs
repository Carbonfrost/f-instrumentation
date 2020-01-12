//
// - LoggerLevelsTests.cs -
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
using System.Linq;
using Carbonfrost.Commons.Instrumentation;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Instrumentation.Logging {

    public class LoggerLevelsTests {

        [Fact]
        public void test_parse_wildcard() {
            Assert.Equal(LoggerLevels.All, LoggerLevels.Parse("*"));
        }

        [Fact]
        public void test_parse_from_all_none() {
            Assert.Equal(LoggerLevels.All, LoggerLevels.Parse("All"));
            Assert.Equal(LoggerLevels.None, LoggerLevels.Parse("None"));
        }

        [Fact]
        public void test_parse_range_unbounded_below() {
            Assert.Equal(LoggerLevels.Below(LoggerLevel.Error), LoggerLevels.Parse(":Error"));
        }

        [Fact]
        public void test_parse_range_unbounded_above() {
            Assert.Equal(LoggerLevels.Above(LoggerLevel.Error), LoggerLevels.Parse("Error:"));
        }

        [Fact]
        public void test_parse_star_per_pattern() {
            Assert.Equal(LoggerLevels.All, LoggerLevels.Parse("*"));
        }

        [Fact]
        public void test_parse_list() {
            LoggerLevels a = new LoggerLevels(LoggerLevel.Debug, LoggerLevel.Trace, LoggerLevel.Info);
            Assert.Equal(a, LoggerLevels.Parse("Debug, Trace, Info"));

            LoggerLevels b = new LoggerLevels(LoggerLevel.Debug, LoggerLevel.Trace, LoggerLevel.Info, LoggerLevel.Verbose);
            Assert.Equal(a, LoggerLevels.Parse("Debug\t Trace\t\r Info"));
        }

        [Fact]
        public void test_parser_parity() {
            foreach (LoggerLevel level in LoggerLevel.GetValues()) {
                LoggerLevels ll = null;
                var ex = Record.Exception(() => ll = LoggerLevels.Parse(level.ToString()));
                Assert.Null(ex);
            }
        }

        [Fact]
        public void test_nothing_matches_off_level() {
            foreach (LoggerLevel level in LoggerLevel.GetValues()) {
                Assert.False(LoggerLevels.Off.IsMatch(level));
            }
        }

        [Fact]
        public void test_everything_matches_off_level() {
            foreach (LoggerLevel level in LoggerLevel.GetValues()) {
                Assert.True(LoggerLevels.All.IsMatch(level));
            }
        }

        [Fact]
        public void test_above_range_contains() {
            Assert.True(LoggerLevels.Above(LoggerLevel.Error).IsMatch(LoggerLevel.Error));
            Assert.True(LoggerLevels.Above(LoggerLevel.Error).IsMatch(LoggerLevel.Fatal));
            Assert.False(LoggerLevels.Above(LoggerLevel.Error).IsMatch(LoggerLevel.Warn));
        }

        [Fact]
        public void test_below_range_contains() {
            Assert.True(LoggerLevels.Below(LoggerLevel.Error).IsMatch(LoggerLevel.Error));
            Assert.True(LoggerLevels.Below(LoggerLevel.Error).IsMatch(LoggerLevel.Info));
            Assert.False(LoggerLevels.Below(LoggerLevel.Error).IsMatch(LoggerLevel.Fatal));
        }

        [Fact]
        public void test_convert_range_to_string() {
            Assert.Equal("Verbose,Debug,Trace,Info,Warn,Error", LoggerLevels.Below(LoggerLevel.Error).ToString());
        }

        [Fact]
        public void should_parse_pseudo_levels_star() {
            LoggerLevels ll = LoggerLevels.Parse("*");
            // * implies all levels are set.
            Assert.True(ll.All(kvp => kvp.Value == true));
        }

        [Fact]
        public void should_parse_pseudo_levels_off() {
            LoggerLevels ll = LoggerLevels.Parse("Off");
            // Off implies no level is set.
            Assert.True(ll.All(kvp => kvp.Value == false));
        }
    }
}
