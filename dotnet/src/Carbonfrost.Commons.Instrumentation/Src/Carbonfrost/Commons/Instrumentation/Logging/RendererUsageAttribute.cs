//
// - RendererUsageAttribute.cs -
//
// Copyright 2013 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Collections.Generic;
using System.Linq;

using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Instrumentation.Logging {

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class RendererUsageAttribute : ProviderAttribute {

        private string[] extensionCache;

        public string Extensions { get; set; }

        public RendererUsageAttribute() : base(typeof(Renderer)) {}

        public IEnumerable<string> EnumerateExtensions() {
            return _SplitText(ref this.extensionCache, this.Extensions);
        }

        private static string[] _SplitText(ref string[] cache, string text) {
            if (cache == null) {
                cache = (text ?? string.Empty).Split(new char[] { '\t', '\r', '\n', ' ', ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
            }

            return (string[]) cache.Clone();
        }

        protected override int MatchCriteriaCore(object criteria) {
            if (criteria == null)
                return 0;

            int score = 0;
            IPropertyProvider source = PropertyProvider.FromValue(criteria);
            score += this.EnumerateExtensions().Contains(source.GetString("Extension")) ? 1 : 0;
            return score;
        }

    }
}
