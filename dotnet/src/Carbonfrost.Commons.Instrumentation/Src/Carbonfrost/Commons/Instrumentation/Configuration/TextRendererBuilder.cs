//
// - TextRendererBuilder.cs -
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
using System.ComponentModel;
using System.Text;

using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Instrumentation.Configuration {

    public class TextRendererBuilder : RendererBuilder {

        [DefaultValue(TextRenderer.DEFAULT_LAYOUT)]
        public string Layout { get; set; }

        public LayoutMode LayoutMode { get; set; }

        public string Header { get; set; }
        public string Footer { get; set; }

        [DefaultEncoding]
        public Encoding Encoding { get; set; }

        public TextRendererBuilder() {
            this.Layout = TextRenderer.DEFAULT_LAYOUT;
            this.Encoding = Encoding.UTF8;
        }

        public override Renderer Build(IServiceProvider serviceProvider = null) {
            var parsedLayout = ParsedLayout.Parse(this.Layout, this.LayoutMode);
            parsedLayout.Header = this.Header;
            parsedLayout.Footer = this.Footer;
            parsedLayout.LayoutMode = this.LayoutMode;
            return new TextRenderer(parsedLayout, this.Encoding);
        }

        public override TypeReference Type {
            get { return TypeReference.FromType(typeof(TextRenderer)); }
            set { }
        }

    }
}
