//
// - InstrumentContext.cs -
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
using System.Collections.Generic;
using System.Threading;

using Carbonfrost.Commons.Instrumentation.Logging;

namespace Carbonfrost.Commons.Instrumentation {

    class InstrumentContext {

        private readonly InstrumentCollection instruments = new InstrumentCollection();

        internal InstrumentCollection Instruments { get { return instruments; } }

        public InstrumentContext(IEnumerable<Instrument> instruments) {
            if (instruments != null) {
                foreach (var i in instruments) {
                    this.instruments.Add(i);
                }
            }
        }

        internal void RunInstruments(LoggerEvent evt) {
            foreach (var i in this.Instruments) {
                if (i.ShouldSkipThis) {
                    continue;
                }

                try {
                    object result = i.Capture(evt);
                    if (result != null)
                        evt.Data[i.Name].Value = evt;

                } catch (Exception ex) {
                    i.SkipThis();
                    Traceables.InstrumentCaptureFailed(i.Name, ex);
                }
            }
        }
    }
}
