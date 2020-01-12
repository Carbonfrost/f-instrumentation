//
// - RendererBuilder.cs -
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

using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Instrumentation.Configuration {

    public abstract class RendererBuilder : Builder<Renderer> {

        public static readonly RendererBuilder Null = new NullRendererBuilder();

        sealed class NullRendererBuilder : RendererBuilder {

            public override Renderer Build(IServiceProvider serviceProvider) {
                return Renderer.Null;
            }

            public override TypeReference Type {
                get { return TypeReference.FromType(typeof(NullRenderer)); }
                set { throw Failure.ReadOnlyProperty("Type"); }
            }
        }
    }
}