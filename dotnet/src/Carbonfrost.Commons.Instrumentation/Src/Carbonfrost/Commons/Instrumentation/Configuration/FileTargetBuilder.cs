//
// - FileTargetBuilder.cs -
//
// Copyright 2010, 2013 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.IO;

using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Validation;

namespace Carbonfrost.Commons.Instrumentation.Configuration {

    public class FileTargetBuilder : StreamingTargetBuilder, IUriContext {

        private Uri baseUri;
        private FileRollover rollover = new FileRollover();

        [DefaultValue(FileMode.Append)]
        public FileMode Mode { get; set; }

        public Renderer Renderer { get; set; }

        public FileRollover Rollover { get { return rollover; } }

        [DefaultValue(true)]
        public bool CreateDirectories { get; set; }

        public string Layout { get; set; }
        public LayoutMode LayoutMode { get; set; }

        [Required]
        [DefaultValue(FileTarget.DEFAULT_FILE_NAME)]
        public string FileName { get; set; }

        public FileTargetBuilder() {
            this.Mode = FileMode.Append;
            this.CreateDirectories = true;
            this.FileName = FileTarget.DEFAULT_FILE_NAME;
        }

        internal Renderer DefaultRenderer {
            get {
                if (!string.IsNullOrWhiteSpace(Layout))
                    return (new TextRendererBuilder {
                                Layout = this.Layout,
                                LayoutMode = this.LayoutMode,
                            }).Build(null);

                string ext = Path.GetExtension(FileName);
                return App.GetProvider<Renderer>(new { Extension = ext })
                    ?? Renderer.Null;
            }
        }

        // BufferedTargetBuilder overrides
        public override TypeReference Type {
            get { return TypeReference.FromType(typeof(FileTarget)); }
            set { throw Failure.ReadOnlyProperty(); }
        }

        public override Target Build(IServiceProvider serviceProvider = null) {
            return new FileTarget(this, serviceProvider);
        }

        // IUriContext implementation
        Uri IUriContext.BaseUri {
            get { return baseUri; }
            set { baseUri = value; }
        }
    }
}
