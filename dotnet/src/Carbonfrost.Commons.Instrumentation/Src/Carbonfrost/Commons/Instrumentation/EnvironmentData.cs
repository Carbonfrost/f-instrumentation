//
// Copyright 2010, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Xml.Serialization;

using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Instrumentation {

    [XmlRoot(Xmlns.Instrumentation2008)]
    public class EnvironmentData {

        private static bool stopDrives;
        private static bool stopVariables;
        private ICollection<string> _logicalDrives = new List<string>();
        private IDictionary<string, string> environmentVariables = new SortedDictionary<string, string>();

        [XmlAttribute("commandLine")]
        public string CommandLine { get; set; }

        [XmlAttribute("hasShutdownStarted")]
        public bool HasShutdownStarted { get; set; }

        [XmlAttribute("machineName")]
        public string MachineName { get; set; }

        [XmlElement("osVersion")]
        public OperatingSystem OSVersion { get; set; }

        [XmlAttribute("processorCount")]
        public int ProcessorCount { get; set; }

        [XmlAttribute("systemDirectory")]
        public string SystemDirectory { get; set; }

        [XmlAttribute("userDomainName")]
        public string UserDomainName { get; set; }

        [XmlAttribute("userInteractive")]
        public bool UserInteractive { get; set; }

        [XmlAttribute("userName")]
        public string UserName { get; set; }

        [XmlAttribute("version")]
        public Version Version { get; set; }

        [XmlAttribute("isMono")]
        public bool IsMono { get; set; }

        [XmlAttribute("environmentVariables")]
        public IDictionary<string, string> EnvironmentVariables { get { return environmentVariables; } }

        [XmlAttribute("frameworkVersion")]
        public string FrameworkVersion { get; set; }

        [XmlArray("logicalDrives")]
        public ICollection<string> LogicalDrives { get { return _logicalDrives; } }

        internal static EnvironmentData Create(Verbosity verbosity) {
            var result = new EnvironmentData {
                CommandLine = Environment.CommandLine,
                FrameworkVersion = RuntimeEnvironment.GetSystemVersion(),
                MachineName = Environment.MachineName,
                UserDomainName = Environment.UserDomainName,
                UserInteractive = Environment.UserInteractive,
                UserName = Environment.UserName,
                SystemDirectory = Environment.SystemDirectory,
                OSVersion = Environment.OSVersion,
                Version = Environment.Version,
                HasShutdownStarted = Environment.HasShutdownStarted,
                ProcessorCount = Environment.ProcessorCount,
                IsMono = Platform.Current.IsMono,
            };

            if (verbosity > Verbosity.Normal) {
                CaptureEnvironmentVariables(result);
                CaptureLogicalDrives(result);
            }

            return result;
        }

        static void CaptureLogicalDrives(EnvironmentData result) {
            if (stopDrives)
                return;

            try {
                result.LogicalDrives.AddMany(Environment.GetLogicalDrives());
                return;

            } catch (SecurityException) {
            } catch (IOException) {
            }

            stopDrives = true;
        }

        static void CaptureEnvironmentVariables(EnvironmentData result) {
            if (stopVariables)
                return;

            try {
                foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
                    result.EnvironmentVariables.Add((string)de.Key, (string)de.Value);

            } catch (SecurityException) {
                // TODO Log this?
                stopVariables = true;
            }
        }

    }
}
