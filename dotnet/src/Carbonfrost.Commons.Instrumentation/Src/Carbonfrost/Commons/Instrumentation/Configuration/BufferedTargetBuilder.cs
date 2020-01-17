//
// - BufferedTargetBuilder.cs -
//
// Copyright 2010, 2013 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.ComponentModel;
using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.Validation;

namespace Carbonfrost.Commons.Instrumentation.Configuration {

    public abstract class BufferedTargetBuilder : TargetBuilder {

        [DefaultValue(true)]
        public bool AutoFlush { get; set; }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DefaultValue(typeof(TimeSpan), "0:00:15")]
        [NonNegative]
        public TimeSpan AutoFlushLatency { get; set; }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [NonNegative]
        [DefaultValue(BufferedTarget.DefaultBufferSize)]
        public int EventBufferSize { get; set; }

        protected BufferedTargetBuilder() {
            this.EventBufferSize = BufferedTarget.DefaultBufferSize;
            this.AutoFlushLatency = TimeSpan.FromSeconds(15);
        }

    }
}
