//
// - Log.cs -
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

namespace Carbonfrost.Commons.Instrumentation {

    public static partial class Log {

        static readonly SuspendTracker _globalSuspension = new SuspendTracker();

        public static bool IsGloballySuspended {
            get {
                return _globalSuspension.IsSuspended;
            }
        }

        public static bool Enabled(LoggerLevel level = null) {
            return Logger.Root.Enabled(level);
        }

        public static void Inspect(object value) {
            Logger.Root.Inspect(value);
        }

        public static void Inspect(string name, object value) {
            Logger.Root.Inspect(name, value);
        }

        public static InspectionContext Inspecting(string name) {
            return Logger.Root.Inspecting(name);
        }

        public static void Write(LoggerLevel level, object value, object data = null) {
            Logger.Root.Log(level, value, data);
        }

        public static void Write(LoggerLevel level, string message, object data = null) {
            Logger.Root.Log(level, message, data);
        }

        public static void Write(LoggerLevel level, string message, Exception exception, object data = null) {
            Logger.Root.Log(level, message, exception, data);
        }

        public static void WriteEvent(LoggerLevel level, Func<LoggerEvent> eventFactory) {
            Logger.Root.LogEvent(level, eventFactory);
        }

        public static void WriteFormat(LoggerLevel level, string format, params object[] args) {
            Logger.Root.LogFormat(level, format, args);
        }

        public static void WriteFormat(LoggerLevel level, string format, object arg0) {
            Logger.Root.LogFormat(level, format, arg0);
        }

        public static void WriteFormat(LoggerLevel level, IFormatProvider formatProvider, string format, params object[] args) {
            Logger.Root.LogFormat(level, formatProvider, format, args);
        }

        public static void WriteFormat(LoggerLevel level, IFormatProvider formatProvider, string format, object arg0) {
            Logger.Root.LogFormat(level, formatProvider, format, arg0);
        }

        public static void WriteFormat(LoggerLevel level, string format, object arg0, object arg1) {
            Logger.Root.LogFormat(level, format, arg0, arg1);
        }

        public static void WriteFormat(LoggerLevel level, IFormatProvider formatProvider, string format, object arg0, object arg1) {
            Logger.Root.LogFormat(level, formatProvider, format, arg0, arg1);
        }

        public static void WriteFormat(LoggerLevel level, string format, object arg0, object arg1, object arg2) {
            Logger.Root.LogFormat(level, format, arg0, arg1, arg2);
        }

        public static void WriteFormat(LoggerLevel level, IFormatProvider formatProvider, string format, object arg0, object arg1, object arg2) {
            Logger.Root.LogFormat(level, formatProvider, format, arg0, arg1, arg2);
        }

        public static void Resume() {
            _globalSuspension.Resume();
        }

        public static void Suspend() {
            _globalSuspension.Suspend();
        }

        public static IDisposable SuspendLogging() {
            return _globalSuspension.Suspending();
        }
    }
}
