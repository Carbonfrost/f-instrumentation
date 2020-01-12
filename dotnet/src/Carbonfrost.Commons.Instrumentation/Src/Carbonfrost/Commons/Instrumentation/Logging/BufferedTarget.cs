//
// - BufferedTarget.cs -
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

using Carbonfrost.Commons.Instrumentation.Configuration;

namespace Carbonfrost.Commons.Instrumentation.Logging {

    public abstract class BufferedTarget : Target {

        internal const int DefaultBufferSize = 8;
        private readonly List<LoggerEvent> _events;
        private Timer _timer;
        private DateTime _lastWrite;

        public bool AutoFlush { get; private set; }
        public TimeSpan AutoFlushLatency { get; private set; }

        [EditorBrowsableAttribute(EditorBrowsableState.Advanced)]
        public int EventBufferSize { get; private set; }

        protected BufferedTarget(BufferedTargetBuilder builder) {
            if (builder == null)
                throw new ArgumentNullException("builder");

            EventBufferSize = builder.EventBufferSize;
            _events = new List<LoggerEvent>(this.EventBufferSize);
            AutoFlushLatency = builder.AutoFlushLatency;
            AutoFlush = builder.AutoFlush;
        }

        public void CancelFlush() {
            if (_timer != null) {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        public void QueueFlush(int interval) {
            if (interval == 0 || this.EventBufferSize == 0) {
                Flush();
                return;
            }
            if (_timer == null) {
                _timer = new Timer(TimerElapsed, null, Timeout.Infinite, Timeout.Infinite);
            }

            _timer.Change(interval, Timeout.Infinite);
        }

        public void QueueFlush(TimeSpan interval) {
            QueueFlush((int) interval.TotalMilliseconds);
        }

        void TimerElapsed(object _) {
            Flush();
            CancelFlush();
        }

        // TODO Synchronize these because they could be unsafe
        protected abstract void FlushCore(int timeout);

        // Target overrides
        public sealed override void Flush(int timeout) {
            this.CancelFlush();
            FlushCore(timeout);
        }

        public sealed override void Write(LoggerEvent loggerEvent) {
            if (loggerEvent == null)
                throw new ArgumentNullException("loggerEvent");

            lock (_events) {
                _events.Add(loggerEvent);
            }

            if (ShouldAutoFlush(loggerEvent))
                WriteData();
            else
                this.QueueFlush(AutoFlushLatency);
        }

        public sealed override void Write(LoggerEvent[] buffer,
                                          int index,
                                          int length)
        {
            WriteCore(buffer, index, length);
            this._lastWrite = DateTime.Now;
        }

        protected abstract void WriteCore(LoggerEvent[] buffer, int index, int length);

        protected virtual bool ShouldAutoFlush(LoggerEvent loggerEvent) {
            return ((DateTime.Now - _lastWrite) > AutoFlushLatency)
                || (_events.Count >= EventBufferSize);
        }

        private void WriteData() {
            if (_events.Count > 0) {
                LoggerEvent[] items;
                lock (_events) {
                    items = _events.ToArray();
                    _events.Clear();
                }
                Write(items, 0, items.Length);
            }
        }
    }
}
