//
// - InspectionEvent.cs -
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
using System.Xml.Serialization;
using Carbonfrost.Commons.PropertyTrees;

namespace Carbonfrost.Commons.Instrumentation {

    [XmlRoot(Xmlns.Instrumentation2008)]
    public class InspectionEvent : LoggerEvent {

        private const string INSTRUMENT_DATA = "instrumentData";

        [XmlIgnore]
        public PropertyTree InstrumentData {
            get {
                return (PropertyTree) this.Data.Children[INSTRUMENT_DATA];
            }
        }

        [XmlAttribute("name")]
        public string Name {
            get { return Data.GetPropertyOrDefault("name", string.Empty); }
            set { Data.SetProperty("name", value); }
        }

        public PropertyTree Value {
            get {
                PropertyTree tree = Data["value"] as PropertyTree;

                if (tree == null) {
                    Data.AppendPropertyTree("value");
                    tree = (PropertyTree) Data["value"];
                }

                return tree;
            }
        }
    }
}
