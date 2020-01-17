//
// - TextRenderer.cs -
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
using System.Text;

using Carbonfrost.Commons.Instrumentation.Configuration;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Instrumentation.Logging {

    [Builder(typeof(TextRendererBuilder))]
    [RendererUsage(Name = "text", Extensions = ".log")]
    public class TextRenderer : Renderer {

        internal const string DEFAULT_LAYOUT = "{TimeStamp} [{Thread}] {Source} {Level,-5:G} - {Message}{NL}";

        private TextWriter writer;
        private ParsedLayout parsedLayout;

        public string Footer { get { return this.parsedLayout.Footer; } }
        public string Header { get { return this.parsedLayout.Header; } }
        public string Layout { get { return this.parsedLayout.Layout; } }
        public LayoutMode LayoutMode { get { return this.parsedLayout.LayoutMode; } }

        internal ParsedLayout ParsedLayout {
            get {
                return parsedLayout;
            }
        }

        public Encoding Encoding { get; private set; }

        public TextRenderer()
            : this(ParsedLayout.Default, Encoding.UTF8) {
        }

        internal TextRenderer(ParsedLayout layout,
                              Encoding encoding) {
            this.parsedLayout = layout;
            this.Encoding = encoding;
        }

        // Renderer overrides
        public override void ChangeStream(Stream stream, bool autoFlush) {
            if (stream == null)
                throw new ArgumentNullException("stream");

            DisposeWriter();
            writer = new StreamWriter(stream, this.Encoding ?? Encoding.UTF8) {
                AutoFlush = true,
            };
        }

        protected override void Dispose(bool manualDispose) {
            if (manualDispose) {
                DisposeWriter();
            }

            base.Dispose(manualDispose);
        }

        public override void StartDocument() {
            parsedLayout.WriteHeader(SafeWriter());
        }

        public override void EndDocument() {
            parsedLayout.WriteFooter(SafeWriter());
        }

        public override void Render(LoggerEvent loggerEvent) {
            parsedLayout.WriteBody(SafeWriter(), loggerEvent);
        }

        public override void Flush() {
            SafeWriter().Flush();
        }

        private void DisposeWriter() {
            try {
                if (this.writer != null)
                    this.writer.Dispose();
            } catch (IOException) {}

            this.writer = null;
        }

        private TextWriter SafeWriter() {
            return writer ?? TextWriter.Null;
        }
    }
}
