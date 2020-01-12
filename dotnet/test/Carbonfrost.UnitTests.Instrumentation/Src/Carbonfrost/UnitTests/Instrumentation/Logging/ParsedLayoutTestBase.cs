//
// - ParsedLayoutTestBase.cs -
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
using System.IO;
using Carbonfrost.Commons.Instrumentation;
using Carbonfrost.Commons.Instrumentation.Logging;

namespace Carbonfrost.UnitTests.Instrumentation.Logging {

    public abstract class ParsedLayoutTestBase : TestBase {

        internal static string Body(ParsedLayout pl, LoggerEvent evt) {
            StringWriter sw = new StringWriter();
            pl.WriteBody(sw, evt);
            return sw.ToString();
        }
    }
}
