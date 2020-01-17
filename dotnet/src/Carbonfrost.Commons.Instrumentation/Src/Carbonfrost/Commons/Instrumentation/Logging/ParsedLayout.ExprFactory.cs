//
// - ParsedLayout.ExprFactory.cs -
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

namespace Carbonfrost.Commons.Instrumentation.Logging {

    partial class ParsedLayout  {

        abstract class ExprFactory {

            public abstract VariableExprBase CreateVariable(string name, string property);

            public virtual ParsedLayout.Expr CreateExpr(string name, string property, string format, string length) {
                switch (name) {
                    case "NewLine":
                    case "NL":
                        return new NewLineExpr();

                    default:
                        VariableExprBase theVariable = CreateVariable(name, property) ?? VariableExprBase.Unknown;
                        theVariable.VarName = name;
                        int ilength;
                        if (Int32.TryParse(length, out ilength))
                            theVariable.Padding = ilength;
                        theVariable.Format = format ?? theVariable.Format;
                        return theVariable;
                }
            }
        }

        class FileNameGeneratorFactory : ExprFactory {

            public override VariableExprBase CreateVariable(string name, string property) {
                switch (name) {
                    case "Index":
                        return new VariableLogIndexExpr(false);
                    case "Number":
                        return new VariableLogIndexExpr(true);
                    case "Name":
                        return new VariableLogNameExpr();
                    case "TimeStamp":
                    case "Timestamp":
                    case "DateTime":
                        return new VariableLogTimeStampExpr { Format = "u" };

                    case "Date":
                        return new VariableLogTimeStampExpr { Format = "yyyy-MM-dd" };

                    case "Time":
                        return new VariableLogTimeStampExpr { Format = "HH:mm:ss" };

                    case "App":
                    case "Application":
                        return VariableExpr.App;

                    case "MachineName":
                    case "Machine":
                        return VariableExpr.Machine;

                    default:
                        return null;
                }
            }
        }

        class DefaultFactory : ExprFactory {

            public override VariableExprBase CreateVariable(string name, string property) {
                switch (name) {
                    case "AppDomain":
                        return new VariableAppDomainExpr();

                    case "Thread":
                    case "ThreadName":
                        return new VariableThreadNameExpr() { VarName = name };
                    case "Message":
                        return new VariableMessageExpr();
                    case "Level":
                        return new VariableLevelExpr();

                    case "StackFrame":
                        return new VariableStackFrameExpr();
                    case "TimeStamp":
                    case "Timestamp":
                        return new VariableTimeStampExpr { Format = "u" };

                    case "Date":
                        return new VariableTimeStampExpr { Format = "yyyy-MM-dd" };

                    case "Time":
                        return new VariableTimeStampExpr { Format = "HH:mm:ss" };

                    case "Source":
                        return new VariableSourceExpr();

                    case "Exception":
                        return new VariableExceptionExpr();

                    case "Data":
                        return new DataExpr(property);

                    default:
                        return new DataExpr(name);
                }
            }
        }
    }
}
