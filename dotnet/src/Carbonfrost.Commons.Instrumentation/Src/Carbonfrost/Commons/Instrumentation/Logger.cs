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
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Carbonfrost.Commons.Instrumentation.Configuration;
using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Core;

namespace Carbonfrost.Commons.Instrumentation {

    [Builder(typeof(LoggerBuilder))]
    public abstract partial class Logger : DisposableObject {

        // Only set if Logger.FromName, FromThread is used, respectively
        public virtual string Name { get { return string.Empty; } }
        internal Thread Thread { get; set; }

        internal bool IsDisposedInternal {
            get {
                return this.IsDisposed;
            }
        }

        public abstract bool IsSuspended { get; }

        // TODO Optimize this value based upon whether the logger will actually capture it
        internal bool NeedStackFrame { get { return true; } }
        internal virtual FileTarget File { get { return null; } }
        internal virtual Logger Parent { get { return null; } }
        internal bool ForwardToParentBlocked { get; set; }
        internal abstract Profiler Profiler { get; }

        // No extenders outside this assembly
        internal Logger() {}

        public bool Enabled(LoggerLevel level = null) {
            return ShouldLog(level);
        }

        internal virtual void Initialize() {}

        public void Inspect(object value) {
            Inspect(null, value);
        }

        public void Inspect(string name, object value) {
            InspectionEvent evt = new InspectionEvent { Name = name };
            evt.Value.Value = value;

            Log(LoggerLevel.Trace, evt);
        }

        public InspectionContext Inspecting(string name) {
            return new InspectionContext(this, name);
        }

        public IProfilerScope Profiling(string name = null) {
            return Profiler.Profiling(name);
        }

        public void Log(LoggerLevel level, object value, object data = null) {
            CoreLog(value, level, null, data);
        }

        public void Log(LoggerLevel level, string message, object data = null) {
            CoreLog(message, null, level, null, data);
        }

        public void Log(LoggerLevel level, string message, Exception exception, object data = null) {
            CoreLog(message, null, level, exception, data);
        }

        public void LogEvent(LoggerLevel level, Func<LoggerEvent> eventFactory) {
            if (Enabled(level))
                CoreLog(eventFactory(), level, null, null);
        }

        public void LogFormat(LoggerLevel level, IFormatProvider formatProvider, string format, params object[] args) {
            CoreLog(format, args, level, null, null);
        }

        public abstract void Resume();
        public abstract void Suspend();

        protected virtual bool ShouldLog(LoggerLevel loggerLevel) {
            return !IsSuspended;
        }

        protected virtual bool ShouldCapture(LoggerEvent loggerEvent) {
            return true;
        }

        void CoreLog(
            string message, object[] args, LoggerLevel level, Exception exception, object data)
        {
            if (this.ShouldLog(level)) {

                LoggerEvent evt = new LoggerEvent {
                    Message = message,
                };
                if (args != null && args.Length > 0) {
                    evt.Arguments.AddMany(args);
                    exception = exception ?? args.OfType<Exception>().FirstOrDefault();
                }

                FinalizeCapture(evt, level, exception, data);
            }
        }

        void CoreLog(
            object message, LoggerLevel level, Exception exception, object data)
        {
            if (ShouldLog(level)) {
                LoggerEvent le = message as LoggerEvent;
                if (le == null) {
                    Exception e = message as Exception;
                    string messageText;
                    if (e == null) {
                        messageText = Convert.ToString(message);
                    } else {
                        messageText = e.Message;
                        exception = e;
                    }

                    le = new LoggerEvent {
                        Message = messageText,
                    };

                }

                FinalizeCapture(le, level, exception, data);
            }
        }

        private void FinalizeCapture(
            LoggerEvent evt,
            LoggerLevel level,
            Exception exception,
            object data)
        {
            StackFrame sf = null;
            if (this.NeedStackFrame) {
                sf = Utility.FindStackFrame(true);
            }

            evt.Exception = ExceptionData.Create(exception);
            evt.Level = level ?? evt.Level ?? LoggerLevel.Debug;
            evt.StackFrame = sf;
            evt.ThreadName = Thread.CurrentThread.Name ?? Thread.CurrentThread.ManagedThreadId.ToString();
            evt.FinalizeTimestamp(DateTime.UtcNow);
            evt.Source = this.Name;

            if (evt.SourceLogger == null)
                evt.SourceLogger = this;

            Capturing(data, evt);
            Capture(evt);
        }

        internal void CaptureForwarded_(LoggerEvent loggerEvent) {
            if (ShouldLog(loggerEvent.Level)) {
                Capturing(null, loggerEvent);
                Capture(loggerEvent);
            }
        }

        internal abstract IEnumerable<string> FindOutputDataKeys();

        protected abstract void Capture(LoggerEvent loggerEvent);

        void Capturing(object data, LoggerEvent evt) {
            // TODO Parse these values for wildcards and props
            var keys = FindOutputDataKeys();
            Func<string, bool> filter = (k) => keys.Contains(k);
            evt.Capturing_(data, filter);
        }
    }
}
