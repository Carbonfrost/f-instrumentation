//
// - FileOutput.cs -
//
// Copyright 2010 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec;
using Carbonfrost.UnitTests.Instrumentation;

namespace Scenarios {

    public class FileOutput {

#if NET
        [Fact]
        public void simple_file_output() {
            AppDomain app = TestUtil.CreateDomain("simple-file-output.xml");
            app.DoCallBack(() => {
                               Log.Write(LoggerLevel.Error, "Simple file output.");
                               Log.Write(LoggerLevel.Warn, "Simple file output.", new { additional_data = 3 });
                           });
        }
#endif
    }
}
