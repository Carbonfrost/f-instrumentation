//
// - ConsoleTarget.cs -
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

using Carbonfrost.Commons.Instrumentation.Configuration;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Instrumentation.Logging {

    [Builder(typeof(ConsoleTargetBuilder))]
    [TargetUsage(Name = "console")]
    public class ConsoleTarget : Target, IHasParsedLayout {

        private readonly ParsedLayout pattern;
        private readonly IConsoleWrapper console;
        private readonly ConsoleColorMappingCollection colorMappings = new ConsoleColorMappingCollection();
        private readonly LoggerLevelMap<ConsoleColors> colorMappingsCache;

        ParsedLayout IHasParsedLayout.ParsedLayout {
            get {
                return pattern;
            }
        }

        internal IConsoleWrapper Console {
            get {
                return console;
            }
        }

        public string Layout { get; private set; }
        public LayoutMode LayoutMode { get; private set; }

        public ConsoleColorMappingCollection ColorMappings {
            get { return colorMappings; }
        }

        internal ConsoleTarget(ConsoleTargetBuilder builder) {
            this.pattern = ParsedLayout.Parse(builder.Layout, builder.LayoutMode);
            this.Layout = builder.Layout;
            this.LayoutMode = builder.LayoutMode;
            this.colorMappings.AddMany(builder.ColorMappings);
            this.colorMappings.MakeReadOnly();
            this.colorMappingsCache = this.colorMappings.GetEffectiveColors();
            this.console = new BclConsoleWrapper();
        }

        public override void Initialize() {
            pattern.WriteHeader(console.Out);
        }

        public override void Write(LoggerEvent loggerEvent) {
            UpdateColors(loggerEvent.Level);

            pattern.WriteBody(console.Out, loggerEvent);
            console.ResetColor();
        }

        public override void Write(LoggerEvent[] buffer, int index, int length) {
            Utility.CheckBuffer(buffer, index, length);

            // Only reset color once
            for (int i = 0; i < length; i++, index++) {
                var loggerEvent = buffer[index];
                pattern.WriteBody(console.Out, loggerEvent);
            }
            console.ResetColor();
        }

        protected override void Dispose(bool manualDispose) {
            if (manualDispose) {
                pattern.WriteFooter(console.Out);
            }
            base.Dispose(manualDispose);
        }

        public override void Flush(int timeout) {
        }

        private void UpdateColors(LoggerLevel level) {
            ConsoleColors colors = this.colorMappingsCache[level];
            console.SetColors(colors);
        }

    }
}
