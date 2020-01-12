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

namespace Carbonfrost.Commons.Instrumentation {

    public class InstrumentationConfiguration {

        private InstrumentCollection instruments = new InstrumentCollection();
        private LoggerBuilderCollection loggers = new LoggerBuilderCollection();

        public static InstrumentationConfiguration Current {
            get {
                // UNDONE Actually support f-configuration
                return new InstrumentationConfiguration();
            }
        }

        public ProfilerBuilder Profiler {
            get {
                return loggers["root"].Profiler;
            }
        }

        public LoggerBuilderCollection Loggers {
            get { return loggers; } }

        public InstrumentCollection Instruments {
            get { return instruments; } }

    }
}
