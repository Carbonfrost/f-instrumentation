//
// - ProfilerTimingHelper.cs -
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
using System.Globalization;
using System.Linq;

namespace Carbonfrost.Commons.Instrumentation {

    class ProfilerTimingHelper {

        private readonly Stack<ProfilerEvent> _timings = new Stack<ProfilerEvent>();

        public void MarkStart(string name) {
            var evt = new ProfilerEvent {
                TimeStamp = DateTime.UtcNow,
                Name = name,
            };
            _timings.Push(evt);
        }

        public LoggerEvent MarkEnd() {
            if (_timings.Count == 0) {
                return null;
            }

            var evt = _timings.Pop();
            evt.SetProfilerTimeStamp(DateTime.UtcNow);
            return evt;
        }

        public IEnumerable<LoggerEvent> MarkEndAll() {
            // TODO Timestamps will slip across multiple items
            while (_timings.Count > 0) {
                yield return MarkEnd();
            }
        }
    }

}
