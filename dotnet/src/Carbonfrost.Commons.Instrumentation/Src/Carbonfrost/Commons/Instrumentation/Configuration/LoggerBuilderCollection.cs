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
using System.Linq;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.PropertyTrees;

namespace Carbonfrost.Commons.Instrumentation.Configuration {

    public class LoggerBuilderCollection : NamedObjectCollection<LoggerBuilder> {

        public LoggerBuilderCollection() {
            AddRootLogger();
        }

        private void AddRootLogger() {
            var root = new LoggerBuilder { Name = "root" };
            Add(root);
        }

        [Add(Name = "logger")]
        public LoggerBuilder AddNew(string name) {
            LoggerBuilder builder = new LoggerBuilder { Name = name };
            Add(builder);
            return builder;
        }

        protected override void ClearItems() {
            base.ClearItems();
            AddRootLogger();
        }

        protected override void RemoveItem(int index) {
            LoggerBuilder item = this[index];

            if (item.Name == "root") {
                throw InstrumentationFailure.CannotRemoveRoot();
            }

            base.RemoveItem(index);
        }

        protected override string GetNameForItem(LoggerBuilder item) {
            return item.Name;
        }
    }
}
