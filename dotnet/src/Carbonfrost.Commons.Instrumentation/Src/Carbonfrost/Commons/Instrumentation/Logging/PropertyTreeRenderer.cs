//
// - PropertyTreeRenderer.cs -
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
using System.IO;
using Carbonfrost.Commons.Instrumentation.Configuration;
using Carbonfrost.Commons.PropertyTrees;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Instrumentation.Logging {

    [Builder(typeof(PropertyTreeRendererBuilder))]
    public sealed class PropertyTreeRenderer : Renderer {

        private PropertyTreeWriter writer;

        internal PropertyTreeRenderer(PropertyTreeRendererBuilder builder) {

        }

        public override void Render(LoggerEvent loggerEvent) {
            if (loggerEvent == null)
                throw new ArgumentNullException("loggerEvent");

            writer.WriteObject(loggerEvent);
        }

        public override void Flush() {
            writer.Flush();
        }

        public override void ChangeStream(Stream stream, bool autoFlush) {
            this.writer = PropertyTreeWriter.CreateXml(stream);
        }

        public override void StartDocument() {
            writer.WriteStartTree("log");
            writer.WriteProperty("formatVersion", "1.0");
        }

        public override void EndDocument() {
            writer.WriteEndTree();
        }
    }

}
