//
// - Log.Profile.cs -
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
using System.Diagnostics;
using System.Linq;

namespace Carbonfrost.Commons.Instrumentation {

    partial class Log {

        public static IDisposable SuspendProfiling() {
            throw new NotImplementedException();
        }

        [Conditional("TRACE")]
        public static void MarkEnd() {
            Profiler.Root.MarkEnd();
        }

        [Conditional("TRACE")]
        public static void MarkStart(string name = null) {
            Profiler.Root.MarkStart(name);
        }

        public static IProfilerScope Profiling(string name = null) {
            return Profiler.Root.Profiling(name);
        }

        public static IDisposable Timing(string name = null) {
            return Profiler.Root.Timing(name);
        }

        public static IProfilerScope NewScope(string name = null) {
            return Profiler.Root.NewScope(name);
        }

        [Conditional("TRACE")]
        public static void AddTime(string name,
                                   TimeSpan duration,
                                   DateTime? time = default(DateTime?)) {
            Profiler.Root.AddTime(name, duration, time);
        }

        [Conditional("TRACE")]
        public static void AddTime(string name, long durationMillis, DateTime? time = default(DateTime?)) {
            Profiler.Root.AddTime(name, durationMillis, time);
        }

        [Conditional("TRACE")]
        public static void AddMetric(string name, int value, DateTime? time = default(DateTime?)) {
            Profiler.Root.AddMetric(name, value, time);
        }

        [Conditional("TRACE")]
        public static void AddMetric(string name, bool value, DateTime? time = default(DateTime?)) {
            Profiler.Root.AddMetric(name, value, time);
        }

        [Conditional("TRACE")]
        public static void AddMetric(string name, decimal value, DateTime? time = default(DateTime?)) {
            Profiler.Root.AddMetric(name, value, time);
        }

        [Conditional("TRACE")]
        public static void AddMetric(string name, double value, DateTime? time = default(DateTime?)) {
            Profiler.Root.AddMetric(name, value, time);
        }

        [Conditional("TRACE")]
        public static void AddMetric(string name, string value, DateTime? time = default(DateTime?)) {
            Profiler.Root.AddMetric(name, value, time);
        }

        [Conditional("TRACE")]
        public static void AddMetric(string name, Enum value, DateTime? time = default(DateTime?)) {
            Profiler.Root.AddMetric(name, value, time);
        }

    }
}
