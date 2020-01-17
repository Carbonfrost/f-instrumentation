//
// - IdentityData.cs -
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
using System.Security.Principal;
using System.Xml;
using System.Xml.Serialization;

namespace Carbonfrost.Commons.Instrumentation.Security {

    [XmlRoot("IdentityData", Namespace = Xmlns.Instrumentation2008)]
    public class IdentityData {

        [XmlAttribute("authenticationType")]
        public string AuthenticationType { get; set; }

        [XmlAttribute("identityType")]
        public Type IdentityType { get; set; }

        [XmlAttribute("isAuthenticated")]
        public bool IsAuthenticated { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        public static IdentityData Create(IIdentity identity) {
            if (identity == null)
                return null;

            return new IdentityData {
                AuthenticationType = identity.AuthenticationType,
                IdentityType = identity.GetType(),
                IsAuthenticated = identity.IsAuthenticated,
                Name = identity.Name,
            };
        }

    }
}
