//
// - Logger.Helpers.cs -
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
using System.Diagnostics;
using System.Globalization;

namespace Carbonfrost.Commons.Instrumentation {

    partial class Logger {

        public void LogFormat(LoggerLevel level, string format, params object[] args) {
            LogFormat(level, CultureInfo.CurrentCulture, format, args);
        }

        public void LogFormat(LoggerLevel level, string format, object arg0) {
            LogFormat(level, CultureInfo.CurrentCulture, format, new object[] { arg0 });
        }

        public void LogFormat(LoggerLevel level, IFormatProvider formatProvider, string format, object arg0) {
            LogFormat(level, formatProvider, format, new object[] { arg0 });
        }

        public void LogFormat(LoggerLevel level, string format, object arg0, object arg1) {
            LogFormat(level, CultureInfo.CurrentCulture, format, new object[] { arg0, arg1 });
        }

        public void LogFormat(LoggerLevel level, IFormatProvider formatProvider, string format, object arg0, object arg1) {
            LogFormat(level, formatProvider, format, new object[] { arg0, arg1 });
        }

        public void LogFormat(LoggerLevel level, string format, object arg0, object arg1, object arg2) {
            LogFormat(level, CultureInfo.CurrentCulture, format, new object[] { arg0, arg1, arg2 });
        }

        public void LogFormat(LoggerLevel level, IFormatProvider formatProvider, string format, object arg0, object arg1, object arg2) {
            LogFormat(level, formatProvider, format, new object[] { arg0, arg1, arg2 });
        }

        public IDisposable SuspendLogging() {
            Suspend();
            return new DisposableAction(Resume);
        }
    }

}
