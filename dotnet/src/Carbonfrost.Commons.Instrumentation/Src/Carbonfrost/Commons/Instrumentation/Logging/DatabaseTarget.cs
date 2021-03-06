//
// - DatabaseTarget.cs -
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
using Carbonfrost.Commons.Instrumentation.Configuration;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Instrumentation.Logging {

    [Builder(typeof(DatabaseTargetBuilder))]
    public abstract class DatabaseTarget : BufferedTarget {

        public string ConnectionStringName { get; private set; }
        public string AppName { get; private set; }
        public int SchemaVersion { get; private set; }

        protected DatabaseTarget(DatabaseTargetBuilder builder) : base(builder) {
            this.ConnectionStringName = builder.ConnectionStringName;
            this.SchemaVersion = builder.SchemaVersion;
            this.AppName = builder.AppName;
        }
    }

}
