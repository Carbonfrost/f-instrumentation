//
// - ParsedLayout.LayoutParser.cs -
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
using System.Text;

namespace Carbonfrost.Commons.Instrumentation.Logging {

    partial class ParsedLayout {

        sealed class LayoutParser {

            // TODO Escaping \r\n
            // {index[,length][:formatString]}
            // {Thread,15:C}

            private readonly List<Expr> _result = new List<Expr>();
            private readonly ExprFactory _factory = new DefaultFactory();

            public LayoutParser() : this(new DefaultFactory()) {}

            public LayoutParser(ExprFactory factory) {
                this._factory = factory;
            }

            public IList<Expr> Parse(string format) {
                if (String.IsNullOrEmpty(format))
                    return _result;

                int length = format.Length;
                int index = 0;

                StringBuilder sb = new StringBuilder();
                while (index < length) {
                    char ch = format[index++];
                    char la = (index < length) ? format[index] : '\0';

                    if (ch == '}') {
                        if (la == '}') {
                            index++;
                        } else
                            return new Expr[0]; // an error
                    }
                    if (ch == '{') {
                        if (la == '{') {
                            index++;

                        } else {
                            index--;
                            PushLiteral(sb);
                            while (index < length) {
                                char cha = format[index++];
                                sb.Append(cha);
                                if (cha == '}') break;
                            }
                            PushVariable(sb);

                            continue;
                        }
                    }
                    sb.Append(ch);
                }

                PushLiteral(sb);
                return this._result;
            }

            void PushLiteral(StringBuilder sb) {
                if (sb.Length > 0) {
                    this._result.Add(new LiteralExpr(sb.ToString()));
                    sb.Length = 0;
                }
            }

            void PushVariable(StringBuilder sb) {
                string nameLength;
                string nameProperty;

                string format;
                string length;
                string name;
                string property;
                Split(sb.ToString(1, sb.Length - 2), ':', out nameLength, out format);
                Split(nameLength, ',', out nameProperty, out length);
                Split(nameProperty, '.', out name, out property);

                Expr result = _factory.CreateExpr(name, property, format, length);
                this._result.Add(result ?? Expr.Error);
                sb.Length = 0;
            }

            static void Split(string s, char c, out string first, out string second) {
                int i = s.IndexOf(c);
                if (i < 0) {
                    first = s;
                    second = null;
                } else {
                    first = s.Substring(0, i);
                    second = s.Substring(i + 1);
                }
            }

        }
    }
}
