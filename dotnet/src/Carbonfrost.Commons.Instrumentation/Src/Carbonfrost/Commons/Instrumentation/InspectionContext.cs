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

using Carbonfrost.Commons.PropertyTrees;
using Carbonfrost.Commons.Core;

namespace Carbonfrost.Commons.Instrumentation {

    public class InspectionContext : DisposableObject {

        private Logger logger;
        private InspectionEvent evt;
        private bool closed;

        internal InspectionContext(Logger logger, string name) {
            this.logger = logger;
            this.evt = new InspectionEvent { Name = name };
        }

        public void AddData(string name, object value) {
            if (name == null)
                throw new ArgumentNullException("name");

            if (name.Trim().Length == 0)
                throw Failure.EmptyString("name");

            ThrowIfClosed();

            if (value != null)
                this.evt.Value.Append(ConvertToNode(name, value));
        }

        private void Close() {
            if (!closed && !this.logger.IsDisposedInternal) {
                this.closed = true;
                this.logger.Log(LoggerLevel.Trace, evt);
            }
        }

        private void ThrowIfClosed() {
            if (closed)
                InstrumentationWarning.ClosedInspection(this.logger);
        }

        static PropertyNode ConvertToNode(string name, object value) {
            if (value == null) {
                return new Property(name);
            }
            var converter = ValueSerializer.GetValueSerializer(value.GetType());
            if (converter != ValueSerializer.Invalid)
                return new Property(name) { Value = converter.ConvertToString(value, null) };

            PropertyTree node = PropertyTree.FromValue(value);
            var result = new PropertyTree(name);
            node.CopyContentsTo(result);
            return result;
        }

        protected override void Dispose(bool manualDispose) {
            Close();
            base.Dispose(manualDispose);
        }
    }
}
