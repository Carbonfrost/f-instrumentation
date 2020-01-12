//
// - NullRenderer.cs -
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

namespace Carbonfrost.Commons.Instrumentation.Logging {

    // N.B. Public because this type is sometimes referenced in configuration
    [RendererUsage(Name = "null")]
    public sealed class NullRenderer : Renderer {
        public override void StartDocument() {}
        public override void Render(LoggerEvent loggerEvent) {}
        public override void Flush() {}
        public override void EndDocument() {}
        public override void ChangeStream(Stream stream, bool autoFlush) {}
    }
}
