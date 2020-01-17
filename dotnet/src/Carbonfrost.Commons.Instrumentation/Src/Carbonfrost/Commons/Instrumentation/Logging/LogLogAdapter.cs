//
// - LogLogAdapter.cs -
//
// Copyright 2014 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Instrumentation.Logging {

    sealed class LogLogAdapter : Logger {

        public static readonly Logger Instance = new LogLogAdapter();

        public override string Name { get { return "<system>"; } }

        internal override Profiler Profiler {
            get { return Profiler.Null; }
        }

        protected override bool ShouldCapture(LoggerEvent loggerEvent) {
            return loggerEvent is LogLogEvent;
        }

        protected override bool ShouldLog(LoggerLevel loggerLevel) {
            return true;
        }

        protected override void Capture(LoggerEvent loggerEvent) {
            var evt = loggerEvent as LogLogEvent;
            if (evt != null) {
                LogLog.Write(evt.Level, evt.ErrorCode, evt.Message);
            }
        }

        public override void Resume() {
            LogLog.Resume();
        }

        public override void Suspend() {
            LogLog.Suspend();
        }

        internal override IEnumerable<string> FindOutputDataKeys() {
            return Empty<string>.Array;
        }

        public override bool IsSuspended { get { return LogLog.IsSuspended; } }

    }
}
