//
// - ConsoleTargetBuilder.cs -
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
using Carbonfrost.Commons.Instrumentation.Logging;

namespace Carbonfrost.Commons.Instrumentation.Configuration {

    public class ConsoleTargetBuilder : TargetBuilder {

        private ConsoleColorMappingCollection colors = new ConsoleColorMappingCollection();

        public ConsoleColorMappingCollection ColorMappings {
            get { return colors; }
        }

        public string Layout { get; set; }
        public LayoutMode LayoutMode { get; set; }

        public override Target Build(IServiceProvider serviceProvider = null) {
            return new ConsoleTarget(this);
        }
    }
}
