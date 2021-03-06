//
// - TextRendererTests.cs -
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
using System.Linq;
using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Instrumentation.Logging {

    public class TextRendererTests {

        [Fact]
        public void Class_should_be_a_provider_type() {
            Assert.Contains(typeof(TextRenderer), App.GetProviderTypes( typeof(Renderer)));
        }

        [Fact]
        public void Class_should_handle_log_extension_as_the_provider() {
            Assert.IsInstanceOf<TextRenderer>(App.GetProvider<Renderer>(
                new {
                    Extension = ".log"
                }));
        }
    }
}
