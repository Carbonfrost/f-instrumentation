//
// Copyright 2010, 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Text;

using Carbonfrost.Commons.Instrumentation;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Instrumentation {

    static class TestUtil {

#if NET
        // Helper to create a cofigured domain using a config file excerpt
        public static AppDomain CreateDomain(string configFile) {
            AppDomain current = App;
            AppDomainSetup ads = new AppDomainSetup {
                ApplicationBase = current.BaseDirectory,
            };

            string configExcerpt = File.ReadAllText(Path.Combine("Content", "Configuration", configFile));
            string configTemplate = File.ReadAllText(Path.Combine("Content", "Configuration", "configuration-template.xml"));
            byte[] bytes = Encoding.ASCII.GetBytes(configTemplate.Replace("{UNIVERSAL_CONFIGURATION}", configExcerpt));
            ads.SetConfigurationBytes(bytes);
            File.WriteAllBytes(ads.ConfigurationFile = Path.GetTempFileName(), bytes);

            return AppDomain.CreateDomain(configFile, current.Evidence, ads);
        }
#endif

    }
}
