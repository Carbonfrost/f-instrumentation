//
// - LogLog.cs -
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
using System.Security;

namespace Carbonfrost.Commons.Instrumentation.Logging {

    static class LogLog {

        private static readonly LoggerLevels Levels = LoggerLevels.All;
        private static readonly SuspendTracker _suspend = new SuspendTracker();

        public static bool IsSuspended { get { return _suspend.IsSuspended; } }

        static LogLog() {
            try {

                var level =
                    Environment.GetEnvironmentVariable("FF_LOGLOG_LEVEL") ?? string.Empty;
                level = level.Trim();

                LoggerLevel myLevel;
                if (level.Length == 0) {
                    // nop

                } else if (LoggerLevel.TryParse(level, out myLevel)) {
                    WriteCore(LoggerLevel.Info,
                              LogErrorCode.InfoChangedLogLevel,
                              "Set log level: " + myLevel);
                    Levels = LoggerLevels.Above(myLevel);

                } else {
                    WriteCore(LoggerLevel.Warn,
                              LogErrorCode.InfoUnrecognizedLogLevel,
                              "Unrecognized log level: " + myLevel);
                }

            } catch (PlatformNotSupportedException) {
            } catch (SecurityException) {
            }
        }

        internal static void Write(LoggerLevel level,
                                   LogErrorCode lec,
                                   string message) {

            if (!IsSuspended && Levels.ContainsKey(level)) {
                WriteCore(level, lec, message);
            }
        }

        internal static void Resume() {
            _suspend.Resume();
        }

        internal static void Suspend() {
            _suspend.Suspend();
        }

        static void WriteCore(LoggerLevel level,
                              LogErrorCode lec,
                              string message)
        {
            Console.WriteLine("LOG: {3:u} {0,5} - {1} - {2}",
                              level.ToString().ToUpper(),
                              lec,
                              message,
                              DateTime.Now);
        }
    }
}
