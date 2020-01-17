//
// - StreamingTarget.cs -
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
using Carbonfrost.Commons.Core;

namespace Carbonfrost.Commons.Instrumentation.Logging {

    public abstract class StreamingTarget : BufferedTarget {

        private Stream currentOutput;

        public bool KeepOpen { get; private set; }
        public int BufferSize { get; private set; }
        public Encoding Encoding { get; private set; }

        public Stream Output {
            get {
                if (currentOutput == null) {
                    ChangeStream();
                }

                return currentOutput;
            }
        }

        protected abstract Stream CreateOutputStream();

        protected internal StreamingTarget(StreamingTargetBuilder builder)
            : base(builder) {

            this.BufferSize = builder.BufferSize;
            this.Encoding = builder.Encoding;
            this.KeepOpen = builder.KeepOpen;
        }

        private void ChangeStream() {
            if (currentOutput != null) {
                currentOutput.Flush();
                currentOutput.Dispose();
            }

            Stream stream;
            try {
                stream = CreateOutputStream() ?? Stream.Null;

            } catch (Exception ex) {
                if (Failure.IsCriticalException(ex)) {
                    throw;
                }

                stream = Stream.Null;
            }
            this.currentOutput = stream;
        }

        // Target overrides
        protected override void FlushCore(int timeout) {
            if (this.currentOutput != null) {
                this.currentOutput.Flush();
            }
        }

    }
}
