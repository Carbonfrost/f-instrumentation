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
using System.ComponentModel;
using System.Linq;

using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Instrumentation.Configuration {

    public class LoggerBuilder : Builder<Logger> {

        private Logger buildCache;
        private string generatedName;
        private string computedParent;
        private readonly ProfilerBuilder profiler = new ProfilerBuilder();
        private LoggerLevels levels = LoggerLevels.All;

        public virtual Target Target {
            get; set;
        }

        [DefaultValue(true)]
        public bool Enabled { get; set; }
        public string Parent { get; set; }
        public ProfilerBuilder Profiler { get { return profiler; } }

        [DefaultValue(typeof(LoggerLevels), "*")]
        public LoggerLevels Levels {
            get { return levels ?? LoggerLevels.All; }
            set { levels = value; }
        }

        public LoggerBuilder() {
            this.Enabled = true;
        }

        // IObjectWithName
        public string Name { get; set; }

        internal string ComputeName() {
            string n = (Name ?? string.Empty).Trim();
            if (n.Length == 0) {
                n = generatedName = Guid.NewGuid().ToString("N");
            }
            return n;
        }

        internal string ComputeParent(LoggerBuilderCollection builders) {
            // Choose the parent - either an explicit name or implicitly depending upon the events being forwarded
            string parentName = (Parent ?? string.Empty).Trim();
            if (parentName.Length == 0) {
                if (builders != null && computedParent == null) {
                    LoggerBuilder builder = PickSourceForward(builders);
                    computedParent = builder == null ? string.Empty : builder.Name;
                }

                return computedParent;
            }
            return parentName;
        }

        internal static LoggerBuilder PickSourceForward(LoggerBuilderCollection builders, string name) {
            if (name == "root") {
                return null;
            }

            // UNDONE Explicit includes are required
            // LoggerBuilder builder = builders.FirstOrDefault(t => t.Sources != null && t.Sources.Includes != null && t.Sources.IsMatch(name));
            return null;
        }

        private LoggerBuilder PickSourceForward(LoggerBuilderCollection builders) {
            return PickSourceForward(builders, this.Name);
        }

        protected override Logger CreateInstanceCore(Type activatedType, IServiceProvider serviceProvider) {
            if (buildCache == null) {
                serviceProvider = serviceProvider ?? ServiceProvider.Null;
                Target target = BuildTarget();

                Logger l = new DefaultLogger(this.Name, levels, LoggerFilter.All, target, this.profiler);
                if (!this.Enabled) {
                    l.Suspend();
                }
                buildCache = l;
            }

            return buildCache;
        }

        private Target BuildTarget() {
            if (this.Target != null)
                return this.Target;

            string parent = ComputeParent(null);
            if (string.IsNullOrWhiteSpace(parent))
                return Target.Null;

            return new ForwardingTarget(parent);
        }

    }
}
