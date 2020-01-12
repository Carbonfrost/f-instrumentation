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
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Instrumentation.Logging {

    public abstract class Target : DisposableObject {

        public static new Target Null { get { return new NullTarget(); } }

        private string name;
        private bool isSealed;

        protected bool IsSealed {
            get {
                ThrowIfDisposed();
                return isSealed;
            }
            private set { isSealed = value; }
        }

        public Logger Logger { get; internal set; }

        public string Name {
            get {
                ThrowIfDisposed();
                return name;
            }
            set {
                ThrowIfSealed();
                name = value;
            }
        }

        protected Target() {}

        public void Flush() {
            Flush(Timeout.Infinite);
        }

        public abstract void Flush(int timeout);

        public void Flush(TimeSpan timeout) {
            Flush((int) timeout.TotalMilliseconds);
        }

        public virtual void Initialize() {}

        public virtual void Write(
            LoggerEvent[] buffer, int index, int length) {

            Utility.CheckBuffer(buffer, index, length);

            for (int i = 0; i < length; i++, index++) {
                Write(buffer[index]);
            }
        }

        public abstract void Write(LoggerEvent loggerEvent);

        protected void ThrowIfSealed() {
            if (IsSealed) {
                throw Failure.Sealed();
            }
        }

        public static Target Compose(params Target[] items) {
            return Compose((IEnumerable<Target>) items);
        }

        public static Target Compose(IEnumerable<Target> items) {
            if (items == null) {
                return Null;
            }
            var all = items.Where(t => t != null && !(t is NullTarget)).ToArray();
            if (all.Length == 0) {
                return Null;
            } else if (all.Length == 1) {
                return all[0];
            } else {
                return new CompositeTarget(all);
            }
        }

        public static Target FromName(string name) {
            return App.GetProvider<Target>(name);
        }
    }
}
