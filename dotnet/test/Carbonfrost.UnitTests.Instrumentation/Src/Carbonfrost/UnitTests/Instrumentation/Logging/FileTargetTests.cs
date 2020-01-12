//
// - FileTargetTests.cs -
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
using System.IO;
using Carbonfrost.Commons.Instrumentation;
using Carbonfrost.Commons.Instrumentation.Configuration;
using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Instrumentation.Logging {

    public class FileTargetTests {

        // TODO This test fails - we can't and don't hook on appdomain shutdown except for loggers created
        // through Logger.All

#if NET
        [Fact, Skip]
        public void should_flush_on_appdomain_close() {
            // TODO Flushing only happens when target is in a logger, so this test
            // doesn't work...

            // When this app domain exits, all file items should have been
            // written out despite an implicit Flush and even when finalizing
            AppDomain cd = App;
            AppDomain appDomain = AppDomain.CreateDomain(
                "a", cd.Evidence, new AppDomainSetup {
                    ApplicationBase = cd.BaseDirectory,
                });

            // This is done because otherwise filename would have to be captured in a
            // closure that isn't serializable (and couldn't be marshalled)
            string fileName = Path.GetTempFileName();
            appDomain.SetData("fileName", fileName);

            CrossAppDomainDelegate d = () => {
                FileTargetBuilder ft = new FileTargetBuilder {
                    FileName = (string) App.GetData("fileName"),
                    EventBufferSize = 100, // Sufficiently large buffer
                };

                FileTarget fileTarget = (FileTarget) ft.Build(null);
                fileTarget.Write(CreateEvent());
                fileTarget.Write(CreateEvent());
                fileTarget.Write(CreateEvent());
                // Intentionally not disposing - fileTarget.Dispose();
            };
            appDomain.DoCallBack(d);
            AppDomain.Unload(appDomain);
            Assert.That(File.ReadAllLines(fileName).Length,
                        Is.EqualTo(3),
                        "Should have implicitly flushed file log data when the domain was unloaded: {0}", fileName);
        }
#endif

        static LoggerEvent CreateEvent() {
            return new LoggerEvent {
                Level = LoggerLevel.Error,
                Source = "source",
                Message = "Loggerevent text - Loggerevent text - Loggerevent text",
                TimeStamp = DateTime.Now,
            };
        }
    }
}
