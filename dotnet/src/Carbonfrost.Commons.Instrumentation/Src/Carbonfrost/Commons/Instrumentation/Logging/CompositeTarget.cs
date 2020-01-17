//
// - CompositeTarget.cs -
//
// Copyright 2015 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Carbonfrost.Commons.Instrumentation.Logging {

    class CompositeTarget : Target {

        private readonly Target[] _all;

        public CompositeTarget(Target[] all) {
            _all = all;
        }

        public override void Flush(int timeout) {
            var errors = new List<Exception>();

            if (timeout == Timeout.Infinite) {
                foreach (var item in _all) {
                    try {
                        item.Flush(Timeout.Infinite);
                    } catch (TimeoutException ex) {
                    } catch (Exception ex) {
                        errors.Add(ex);
                    }
                }
                ExitFlush(errors, false);
                return;
            }

            var stop = Stopwatch.StartNew();
            bool timedOut = false;

            foreach (var item in _all) {
                int actualTimeout = (int) (timeout - stop.ElapsedMilliseconds);
                try {
                    item.Flush(Math.Max(actualTimeout, 1));
                } catch (TimeoutException) {
                    timedOut = true;
                } catch (Exception ex) {
                    errors.Add(ex);
                }
            }

            ExitFlush(errors, timedOut || stop.ElapsedMilliseconds > timeout);
        }

        void ExitFlush(List<Exception> errors, bool timedOut) {
            if (errors.Count > 0) {
                throw InstrumentationFailure.FlushErrors(errors);
            }
            if (timedOut) {
                throw InstrumentationFailure.FlushTimedOut();
            }
        }

        public override void Write(LoggerEvent loggerEvent) {
            foreach (var item in _all) {
                item.Write(loggerEvent);
            }
        }
    }
}
