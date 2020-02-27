//
// - DataExprTests.cs -
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
using Carbonfrost.Commons.PropertyTrees;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Instrumentation.Logging {

    public class DataExprTests : ParsedLayoutTestBase {

        [Fact]
        public void vertical_format_express_all_nodes() {
            ParsedLayout pl = ParsedLayout.Parse("{Data}", LayoutMode.Vertical);
            LoggerEvent evt = new LoggerEvent();
            CreateData(evt.Data);

            string text = Body(pl, evt);

            // TODO Improve this assertion
            Assert.Contains("Data.environment.memory=", text);
        }


        [Fact, Skip]
        public void apply_data_property_filtering() {
            ParsedLayout pl = ParsedLayout.Parse("{Data.environment.*}", LayoutMode.Vertical);
            LoggerEvent evt = new LoggerEvent();
            CreateData(evt.Data);

            string text = Body(pl, evt);

            // TODO Improve this assertion
            Assert.Contains("Data.environment.memory=", text);
            Assert.DoesNotContain("Data.memory=", text);
        }

        private void CreateData(PropertyTree result) {
            var env = LoadTree("EnvironmentData/nominal.xml", "environment");
            var mem = LoadTree("MemoryData/nominal.xml", "memory");

            // Adding for the sake of nesting
            env.Append(mem);
            result.Append(env);
        }

        private PropertyTree LoadTree(string path, string root) {
            string file = TestContext.GetFullPath(Path.Combine("Fixtures", path));
            var node = PropertyTree.FromFile(file);
            var result = new PropertyTree(root);
            node.CopyContentsTo(result);
            return result;
        }
    }
}
