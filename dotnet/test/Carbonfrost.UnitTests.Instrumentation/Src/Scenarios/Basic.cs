//
// - Basic.cs -
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
using Carbonfrost.Commons.Instrumentation;
using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.Spec;

namespace Scenarios {

    public class Basic {

        // TODO Develop additional scenarios
        // TODO Test configuration
        //    - when there are no loggers defined (targets null)
        //    - when multiple loggers are defined
        //    - retrieval via name

        [Fact]
        public void create_logger_in_code() {
            Logger logger = Logger.Create(Target.Null);
            logger.Debug("This is a debug message");
            logger.Error("This is an error message");
            logger.Fatal("This is a fatal message");
        }

        [Fact]
        public void write_to_all_loggers_configured() {
            Log.Debug("This is a debug message");
            Log.Error("This is an error message");
            Log.Fatal("This is a fatal message");
        }

    }
}
