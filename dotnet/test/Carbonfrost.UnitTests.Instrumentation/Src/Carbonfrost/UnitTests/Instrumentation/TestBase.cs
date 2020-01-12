//
// Copyright 2012, 2013, 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;

using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.UnitTests.Instrumentation {

    public abstract class TestBase : TestClass {

        protected static void AssertDefaultValues(object unit) {
            foreach (var pd in unit.GetType().GetTypeInfo().GetProperties()) {
                if (pd.SetMethod == null || !pd.SetMethod.IsPublic) {
                    continue;
                }
                if (!pd.IsDefined(typeof(DefaultValueAttribute))) {
                    continue;
                }
                var initValue = pd.GetValue(unit);
                var resetValue = pd.GetDefaultValue();
                Assert.Equal(resetValue, initValue);
            }
        }
    }
}
