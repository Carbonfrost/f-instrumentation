//
// - VerbositySampleRates.cs -
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
using System.ComponentModel;

namespace Carbonfrost.Commons.Instrumentation.Configuration {

    public class VerbositySampleRates : VerbosityMap<double> {

        public new double All {
            get { return base.All; }
            set { base.All = value; }
        }

        public new double Quiet {
            get { return base.Quiet; }
            set { base.Quiet = value; }
        }

        public new double Minimal {
            get { return base.Minimal; }
            set { base.Minimal = value; }
        }

        public new double Normal {
            get { return base.Normal; }
            set { base.Normal = value; }
        }

        public new double Detailed {
            get { return base.Detailed; }
            set { base.Detailed = value; }
        }

        public new double Diagnostic {
            get { return this[Verbosity.Diagnostic]; }
            set { base.Diagnostic = value; }
        }

    }

}
