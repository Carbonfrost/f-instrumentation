//
// Copyright 2010, 2012, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Collections.ObjectModel;

using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.PropertyTrees;
using Carbonfrost.Commons.Core;

namespace Carbonfrost.Commons.Instrumentation.Configuration {

    public class ConsoleColorMappingCollection : Collection<ConsoleColorMapping> {

        public ConsoleColor? DefaultForeground {
            get;
            set;
        }

        public ConsoleColor? DefaultBackground {
            get;
            set;
        }

        [Add]
        public ConsoleColorMapping AddNew(LoggerLevels level) {
            ConsoleColorMapping result = new ConsoleColorMapping { Level = level };
            this.Add(result);
            return result;
        }

        internal LoggerLevelMap<ConsoleColors> GetEffectiveColors() {
            LoggerLevelMap<ConsoleColors> result
                = new LoggerLevelMap<ConsoleColors>(
                    new ConsoleColors {
                        Foreground = null,
                        Background = null
                    });

            foreach (var mapping in this) {
                foreach (var level in mapping.Level.TrueValues()) {
                    ConsoleColors cc = result[level];
                    result[level] =
                        new ConsoleColors(mapping.Foreground, mapping.Background);
                }
            }

            return result;
        }

        protected override void ClearItems() {
            ThrowIfReadOnly();
            base.ClearItems();
        }

        protected override void InsertItem(int index, ConsoleColorMapping item) {
            ThrowIfReadOnly();
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index) {
            ThrowIfReadOnly();
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, ConsoleColorMapping item) {
            ThrowIfReadOnly();
            base.SetItem(index, item);
        }

        private void ThrowIfReadOnly() {
            if (IsReadOnly)
                throw Failure.ReadOnlyCollection();
        }

        public bool IsReadOnly {
            get; private set;
        }

        internal void MakeReadOnly() {
            IsReadOnly = true;
        }
    }
}
