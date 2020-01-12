//
// - IProfilerScope.cs -
//
// Copyright 2012 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Linq;

namespace Carbonfrost.Commons.Instrumentation {

    public interface IProfilerScope : IDisposable {

        IDisposable Timing(string name = null);
        void MarkStart(string name = null);
        void MarkEnd();

        IProfilerScope NewScope(string name = null);

        // Metrics must be int, real, or text

        void AddTime(string name,
                     TimeSpan duration,
                     DateTime? time = null);

        void AddTime(string name,
                     long durationMillis,
                     DateTime? time = null);

        void AddMetric(string name,
                       int value,
                       DateTime? time = null);

        void AddMetric(string name,
                       bool value,
                       DateTime? time = null);

        void AddMetric(string name,
                       decimal value,
                       DateTime? time = null);

        void AddMetric(string name,
                       double value,
                       DateTime? time = null);

        void AddMetric(string name,
                       string value,
                       DateTime? time = null);

        void AddMetric(string name,
                       Enum value,
                       DateTime? time = null);
    }
}
