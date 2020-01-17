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
using System.Collections.Generic;
using Carbonfrost.Commons.Core;

namespace Carbonfrost.Commons.Instrumentation.Logging {

    public abstract class Source {

        public abstract LoggerEvent Read();

        public virtual void Read(LoggerEvent[] buffer, int index, int length) {
            if (buffer == null) {
                throw new ArgumentNullException("buffer");
            }

            if (index < 0 || index >= buffer.Length) {
                throw Failure.IndexOutOfRange("index", index, 0, buffer.Length - 1);
            }

            if (index + length >= buffer.Length) {
                throw Failure.NotEnoughSpaceInArray("index", index);
            }

            for (int i = 0; i < length; i++) {
                buffer[index++] = Read();
            }
        }

        public virtual IEnumerable<LoggerEvent> ReadToEnd() {
            LoggerEvent le;
            while ((le = Read()) != null) {
                yield return le;
            }
        }
    }
}
