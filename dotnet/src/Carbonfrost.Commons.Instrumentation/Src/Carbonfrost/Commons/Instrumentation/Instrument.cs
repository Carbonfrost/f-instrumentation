//
// - Instrument.cs -
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

using Carbonfrost.Commons.Instrumentation.Configuration;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Instrumentation {

    public abstract class Instrument {

        // TODO Filter on categories, threads, etc.

        // TODO capture once support

        private object data;
        private string name;
        private bool skipThis;
        private DateTime lastCapture;
        private InstrumentAutoCapture autoCapture = new InstrumentAutoCapture();
        private VerbositySampleRates sampleRates = new VerbositySampleRates();

        internal bool ShouldSkipThis { get { return skipThis; } }
        public bool Enabled { get; set; }

        public string Name {
            get {
                if (string.IsNullOrWhiteSpace(name)) {
                    QualifiedName f = App.GetProviderName(typeof(Instrument), this);
                    if (f == null)
                        return GetType().Name;
                    else
                        return f.LocalName;

                } else
                    return name;
            }
            internal set { name = value; }
        }

        internal void SkipThis() {
            this.skipThis = true;
        }

        public IDisposable Suspend() {
            return new SuspendContext(this);
        }

        public object Capture(LoggerEvent loggerEvent) {
            if (ShouldCapture()) {

                // TODO Get appropriate verbosity
                this.data = CaptureCore(data, loggerEvent, Verbosity.Normal);
                this.lastCapture = DateTime.Now;
            }

            // loggerEvent.InstrumentData[this.Name].Value = this.data;
            return this.data;
        }

        internal void Freeze(InstrumentBuilder builder) {
            this.autoCapture.CopyFrom(builder.AutoCapture);
            this.sampleRates.CopyFrom(builder.SampleRates);
        }

        private bool ShouldCapture() {
            return this.Enabled
                && (DateTime.Now - lastCapture) > this.autoCapture.Resolution;
        }

        protected abstract object CaptureCore(object previousData, LoggerEvent loggerEvent, Verbosity verbosity);

        private class SuspendContext : IDisposable {
            private Instrument ins;
            public SuspendContext(Instrument ins) {
                this.ins = ins;
                this.ins.Enabled = false;
            }
            public void Dispose() {
                this.ins.Enabled = true;
            }
        }

    }

}
