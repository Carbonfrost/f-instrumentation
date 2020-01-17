//
// - UtilityTests.cs -
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
using System.Linq;
using Carbonfrost.Commons.Instrumentation;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Instrumentation {

    public class UtilityTests {

        [Fact]
        public void test_create_property_query() {
            Assert.Equal("//a/b/c", Utility.CreatePropertyQuery("a.b.c"));
            Assert.Equal("//a/b/*", Utility.CreatePropertyQuery("a.b.*"));
        }

        // TODO Should also check for invalid chars
    }
}
