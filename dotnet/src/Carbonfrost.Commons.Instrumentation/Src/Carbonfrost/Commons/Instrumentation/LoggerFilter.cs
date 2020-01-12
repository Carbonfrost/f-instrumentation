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
using System.Collections.Generic;
using System.Linq;

using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Instrumentation {

    [Composable, Providers]
    public abstract class LoggerFilter {

        public static readonly LoggerFilter All = new InvariantLoggerFilter(true);
        public static readonly LoggerFilter None = new InvariantLoggerFilter(false);

        [LoggerFilterUsage]
        public static LoggerFilter Compose(IEnumerable<LoggerFilter> filters) {
            if (filters == null)
                throw new ArgumentNullException("filters");

            return Compose(filters.ToArray());
        }

        public static LoggerFilter Compose(params LoggerFilter[] filters) {
            if (filters == null)
                return All;

            if (filters.Length == 0)
                return All;

            if (filters.Length == 1)
                return filters[0];

            return new CompositeFilter(filters);
        }

        public virtual bool ShouldCaptureInspectors(LoggerEvent loggerEvent) {
            return loggerEvent is InspectionEvent;
        }

        public virtual bool ShouldCapture(LoggerEvent loggerEvent) {
            return true;
        }

        sealed class InvariantLoggerFilter : LoggerFilter {

            private readonly bool _result;

            public InvariantLoggerFilter(bool _result) {
                this._result = _result;
            }

            public override bool ShouldCapture(LoggerEvent loggerEvent) {
                return _result;
            }

            public override bool ShouldCaptureInspectors(LoggerEvent loggerEvent) {
                return _result;
            }
        }
    }
}
