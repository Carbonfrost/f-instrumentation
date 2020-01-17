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

using System.IO;

using Carbonfrost.Commons.Core;

namespace Carbonfrost.Commons.Instrumentation.Logging {

    public abstract class Renderer : DisposableObject {

        public static new readonly Renderer Null = new NullRenderer();

        public abstract void ChangeStream(Stream stream, bool autoFlush);
        public abstract void StartDocument();
        public abstract void EndDocument();
        public abstract void Render(LoggerEvent loggerEvent);
        public abstract void Flush();

        public virtual void Render(
            LoggerEvent[] buffer, int index, int length) {

            if (index < 0 || index >= buffer.Length)
                throw Failure.IndexOutOfRange("index", 0, buffer.Length - 1);

            for (int i = 0; i < length; i++)
                Render(buffer[index++]);
        }
    }

}
