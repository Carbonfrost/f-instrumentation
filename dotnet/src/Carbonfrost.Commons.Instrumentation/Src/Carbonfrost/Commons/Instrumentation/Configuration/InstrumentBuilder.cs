//
// - InstrumentBuilder.cs -
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
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Instrumentation.Configuration {

    public class InstrumentBuilder : Builder<Instrument> {

        private VerbositySampleRates sampleRates = new VerbositySampleRates();
        private InstrumentAutoCapture _autoCapture = new InstrumentAutoCapture();

        public InstrumentAutoCapture AutoCapture { get { return _autoCapture; } }

        public string Name { get; set; }

        public Verbosity Verbosity { get; set; }

        public double SampleRate { get; set; }

        public VerbositySampleRates SampleRates {
            get { return sampleRates; }
        }

        public InstrumentBuilder() {
            this.Verbosity = Verbosity.Normal;
        }

        public override Instrument Build(IServiceProvider serviceProvider = null) {
            Instrument result = base.Build(serviceProvider);
            result.Name = this.Name;
            result.Freeze(this);
            return result;
        }
    }

}
