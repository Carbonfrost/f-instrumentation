//
// - LoggerTests.cs -
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
using Carbonfrost.Commons.Instrumentation;
using Carbonfrost.Commons.Instrumentation.Configuration;
using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Instrumentation.Logging {

    public class LoggerTests {

#if NET
        [Fact]
        public void detect_missing_parent() {
            AppDomain app = TestUtil.CreateDomain("invalid-logs-with-cycles.xml");
            app.DoCallBack(() => {
                               var it = (DefaultLogger) Logger.FromName("g");
                               Assert.True(it.Parent is NullLogger);
                           });
        }

        [Fact]
        public void cannot_use_both_target_and_parent() {
            AppDomain app = TestUtil.CreateDomain("invalid-both-target-parent.xml");
            app.DoCallBack(() => {
                               var it = Logger.FromName("a") as DefaultLogger;
                               Assert.NotNull(it.File);
                           });
        }

        [Fact]
        public void detect_parent_logger_cycles() {
            AppDomain app = TestUtil.CreateDomain("invalid-logs-with-cycles.xml");
            app.DoCallBack(() => {
                               var it = Logger.FromName("a");
                               Assert.True(it.ForwardToParentBlocked);

                               it = Logger.FromName("f");
                               Assert.False(it.ForwardToParentBlocked);
                           });
        }

        [Fact]
        public void implicit_source_aggregation_on_new_loggers() {
            AppDomain app = TestUtil.CreateDomain("source-forwarding.xml");
            app.DoCallBack(() => {
                               var loggers = InstrumentationConfiguration.Current.Loggers;

                               // Even though dx isn't defined explicitly, it is
                               // still forwarded to a
                               var it = (DefaultLogger) Logger.FromName("dx");
                               Assert.True(it.Parent.Name == "a");
                           });
        }

        [Fact]
        public void implicit_source_aggregation() {
            AppDomain app = TestUtil.CreateDomain("source-forwarding.xml");
            app.DoCallBack(() => {
                               var loggers = InstrumentationConfiguration.Current.Loggers;

                               var it = (DefaultLogger) Logger.FromName("d1");
                               Assert.True(it.Parent.Name == "a");

                               it = (DefaultLogger) Logger.FromName("d2");
                               Assert.True(it.Parent.Name == "a");
                           });
        }
#endif

        [Fact]
        public void is_suspended_test() {
            Logger l = Logger.Create(Target.Null);
            Assert.False(l.IsSuspended); //  Should start up not suspended

            l.Suspend();
            Assert.True(l.IsSuspended); // Should be suspended

            l.Resume();
            Assert.False(l.IsSuspended) ; // Should not be suspended

        }

        [Fact]
        public void nested_suspensions() {
            Logger l = Logger.Create(Target.Null);

            l.Suspend();
            l.Suspend();
            Assert.True(l.IsSuspended); // Should be suspended

            l.Resume();

            Assert.True(l.IsSuspended); // Should be suspended
            l.Resume();

            Assert.False(l.IsSuspended);
        }

        [Fact]
        public void log_error_nominal() {
            MemoryTarget target = new MemoryTarget(null);
            Logger l = Logger.Create(target);
            l.Log(LoggerLevel.Error, "Message", new Exception());

            LoggerEvent evt = target.Events[0];
            Assert.Matches(@"^System\.Exception, (mscorlib|System\.Private\.CoreLib)", evt.Exception.ExceptionType.ToString());
            Assert.Equal("Message", evt.Message);
            Assert.Equal(LoggerLevel.Error, evt.Level);
        }

        [Fact]
        public void log_format_capture_arguments() {
            MemoryTarget target = new MemoryTarget(null);
            Logger l = Logger.Create(target);
            l.LogFormat(LoggerLevel.Error, "Message: {0}", "Hello");

            LoggerEvent evt = target.Events[0];
            Assert.Equal("Message: {0}", evt.Message);
            Assert.Equal("Message: Hello", evt.FormatMessage());
        }

        [Fact]
        public void log_format_capture_arguments_two_items() {
            MemoryTarget target = new MemoryTarget(null);
            Logger l = Logger.Create(target);
            l.ErrorFormat("Message: {0}?  {1}", new object[] { "Hello", "Is it me you're looking for?" });

            LoggerEvent evt = target.Events[0];
            Assert.Equal("Message: {0}?  {1}", evt.Message);
            Assert.Equal("Message: Hello?  Is it me you're looking for?", evt.FormatMessage());
        }

        [Fact]
        public void logger_uses_null_adapter() {
            var type = typeof(Logger).GetAdapterType("Null");
            Assert.Equal("NullLogger", type.Name);
        }
    }
}
