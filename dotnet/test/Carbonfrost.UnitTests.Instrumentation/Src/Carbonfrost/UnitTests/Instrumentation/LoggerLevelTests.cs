//
// Copyright 2012, 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.ComponentModel;
using System.Linq;
using Carbonfrost.Commons.Instrumentation;
using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Instrumentation.Logging {

    public class LoggerLevelTests {

        [Fact]
        public void test_equality_nominal() {
            Assert.True(LoggerLevel.Debug == LoggerLevel.Parse("Debug"));
            Assert.True(LoggerLevel.Debug.Equals(LoggerLevel.Debug));
        }

        [Fact]
        public void test_inequality_nominal() {
            Assert.True(LoggerLevel.Debug != LoggerLevel.Off);
        }

        [Fact]
        public void test_greater_than_nominal() {
            Assert.True(LoggerLevel.Trace > LoggerLevel.Debug);
            Assert.True(LoggerLevel.Error > LoggerLevel.Warn);
            Assert.True(LoggerLevel.Trace >= LoggerLevel.Debug);
            Assert.True(LoggerLevel.Error >= LoggerLevel.Warn);

            Assert.GreaterThan(0, LoggerLevel.Trace.CompareTo(LoggerLevel.Debug));
            Assert.GreaterThan(0, LoggerLevel.Error.CompareTo(LoggerLevel.Warn));
        }

        [Fact]
        public void test_less_than_nominal() {
            Assert.True(LoggerLevel.Verbose < LoggerLevel.Debug);
            Assert.True(LoggerLevel.Warn < LoggerLevel.Error);
            Assert.True(LoggerLevel.Verbose <= LoggerLevel.Debug);
            Assert.True(LoggerLevel.Warn <= LoggerLevel.Error);

            Assert.LessThan(0, LoggerLevel.Debug.CompareTo(LoggerLevel.Trace));
            Assert.LessThan(0, LoggerLevel.Trace.CompareTo(LoggerLevel.Error));
        }

        [Fact]
        public void test_to_string_formats() {
            Assert.Equal("Trace", LoggerLevel.Trace.ToString("g"));
            Assert.Equal("TRACE", LoggerLevel.Trace.ToString("G"));
            Assert.Equal("T", LoggerLevel.Trace.ToString("c"));
            Assert.Equal("T", LoggerLevel.Trace.ToString("C"));
            Assert.Equal("2", LoggerLevel.Trace.ToString("V"));
            Assert.Equal("2", LoggerLevel.Trace.ToString("v"));
        }

        [Fact]
        public void test_parse_string() {
            Assert.Equal(LoggerLevel.Off, LoggerLevel.Parse("Off"));
            Assert.Equal(LoggerLevel.Debug, LoggerLevel.Parse("Debug"));
            Assert.Equal(LoggerLevel.Trace, LoggerLevel.Parse("Trace"));
            Assert.Equal(LoggerLevel.Info, LoggerLevel.Parse("Info"));
            Assert.Equal(LoggerLevel.Warn, LoggerLevel.Parse("Warn"));
            Assert.Equal(LoggerLevel.Error, LoggerLevel.Parse("Error"));
            Assert.Equal(LoggerLevel.Fatal, LoggerLevel.Parse("Fatal"));
            Assert.Equal(LoggerLevel.Verbose, LoggerLevel.Parse("Verbose"));
        }
    }
}
