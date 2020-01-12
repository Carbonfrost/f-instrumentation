//
// - FileTargetBuilderTests.cs -
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

using Carbonfrost.Commons.Instrumentation.Configuration;
using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Instrumentation.Configuration {

    public class FileTargetBuilderTests : TestBase {

        [Fact]
        public void check_default_values() {
            FileTargetBuilder unit = new FileTargetBuilder();
            AssertDefaultValues(unit);
        }

        [Fact]
        public void get_implied_renderer_type_log() {
            FileTargetBuilder unit = new FileTargetBuilder();
            unit.FileName = "file.log";

            Assert.IsInstanceOf<TextRenderer>(unit.DefaultRenderer);
        }

        [Fact]
        public void get_implied_renderer_using_layout() {
            FileTargetBuilder unit = new FileTargetBuilder();
            unit.Layout = "{Timestamp}";

            Assert.IsInstanceOf<TextRenderer>(unit.DefaultRenderer);
        }

        [Fact]
        public void get_implied_renderer_type_unknown() {
            FileTargetBuilder unit = new FileTargetBuilder();
            unit.FileName = "file.zzz";

            Assert.IsInstanceOf<NullRenderer>(unit.DefaultRenderer);
        }

    }

}
