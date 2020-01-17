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
using System.IO;
using System.Text;

using System.Text.RegularExpressions;
using Carbonfrost.Commons.PropertyTrees;

namespace Carbonfrost.Commons.Instrumentation.Logging {

    partial class ParsedLayout {

        internal abstract class Expr {

            public static readonly Expr Error = new LiteralExpr("?");
            public static readonly Expr DefaultSeparator = new LiteralExpr(new String('-', 72));
            public static readonly Expr NewLine = new NewLineExpr();

            public abstract void Write(ParsedLayout layout, TextWriter tw, LoggerEvent evt);

            public virtual bool IsStatic() {
                return true;
            }
        }

        sealed class LiteralExpr : Expr {
            private readonly string _literal;

            public LiteralExpr(string literal) {
                _literal = literal;
            }
            public override void Write(ParsedLayout layout, TextWriter tw, LoggerEvent evt) {
                layout.WriteLiteral(tw, _literal);
            }

            public Expr Trim() {
                string t = Regex.Replace(_literal, @"\s+", " ");

                if (t.Length < _literal.Length) {
                    return new LiteralExpr(t);
                }
                return this;
            }
        }

        sealed class NewLineExpr : Expr {

            public override void Write(ParsedLayout layout, TextWriter tw, LoggerEvent evt) {
                layout.WriteLine(tw);
            }
        }

        abstract class VariableExprBase : Expr {

            public static readonly VariableExprBase Unknown = VariableExpr.UnknownImpl;

            public virtual string Format { get; set; }
            public string Property { get; private set; }
            public int Padding { get; set; }

            public virtual string VarName {
                get; set;
            }

            protected VariableExprBase(string property) {
                Property = property;
            }

            public sealed override bool IsStatic() {
                return false;
            }

        }

        sealed class ThunkVariableExpr : VariableExpr {

            private readonly Func<string> thunk;

            public ThunkVariableExpr(Func<string> thunk) {
                this.thunk = thunk;
            }

            protected override string Evaluate(LoggerEvent evt) {
                return thunk();
            }

        }

        abstract class VariableExpr : VariableExprBase {

            private string formatCache;

            public virtual bool Escape { get { return false; } }

            protected VariableExpr()
                : base(null) {}

            internal static VariableExpr App {
                get {
#if NET
                    Assembly a = Assembly.GetEntryAssembly();
                    if (a != null)
                        return Create(() => a.GetName().Name);
                    string app = AppDomain.CurrentDomain.FriendlyName;
                    if (!string.IsNullOrEmpty(app))
                        return Create(() => app);
#endif

                    string p = Process.GetCurrentProcess().ProcessName;
                    return Create(() => p);
                }
            }

            internal static VariableExpr Machine {
                get {
                    #if NET
                    return Create(() => Environment.MachineName);
                    #else
                    return Create(() => "Environment.MachineName");
                    #endif
                }
            }

            internal static VariableExpr UnknownImpl {
                get {
                    return Create(() => string.Empty);
                }
            }

            static VariableExpr Create(Func<string> thunk) {
                return new ThunkVariableExpr(thunk);
            }

            public sealed override void Write(ParsedLayout layout, TextWriter tw, LoggerEvent evt) {
                if (formatCache == null) {
                    formatCache = string.Concat('{', '0',
                                                Padding == 0 ? string.Empty : ("," + Padding),
                                                string.IsNullOrWhiteSpace(Format) ? string.Empty : (":" + Format),
                                                '}');
                }

                layout.WriteVar(tw, VarName,
                                string.Format(formatCache, Evaluate(evt)),
                                Escape);
            }

            protected abstract string Evaluate(LoggerEvent evt);

            protected string FormatDateTime(DateTime dt) {
                if (string.Equals(Property, "UTC", StringComparison.OrdinalIgnoreCase))
                    dt = dt.ToUniversalTime();

                return dt.ToString(this.Format);
            }
        }

        sealed class DataExpr : DataExprBase {

            public DataExpr(string property)
                : base(property) {}

            protected override PropertyTree GetDataTree(LoggerEvent evt) {
                return evt.Data;
            }
        }

        abstract class DataExprBase : VariableExprBase {

            protected DataExprBase(string property) : base(property) {
            }

            protected abstract PropertyTree GetDataTree(LoggerEvent evt);

            public override void Write(ParsedLayout layout, TextWriter tw, LoggerEvent evt) {
                PropertyTree data = this.GetDataTree(evt);

                IEnumerable<PropertyNode> nodes;
                if (string.IsNullOrWhiteSpace(this.Property)) {
                    nodes = new [] { data };

                } else {
                    nodes = ApplyDataFilter(data);
                }

                foreach (var node in nodes) {
                    if (node.IsPropertyTree)
                        WriteNode((PropertyTree) node, layout, this.Property, tw, 0);
                    else
                        WriteCore(layout, node.Value, Property, tw);
                }
            }

            IEnumerable<PropertyNode> ApplyDataFilter(PropertyTree data) {
                if (this.Property.Contains(".")) {
                    string query = Utility.CreatePropertyQuery(this.Property);
                    return data.SelectNodes(this.Property.Replace('.', '/'));

                } else {
                    // TODO Warn (once) when there is no matching property -- consider stopping output too
                    var result = data.SelectNode(Property);
                    return result == null ? Empty<PropertyNode>.Array : new [] { result };
                }
            }

            // TODO Generated output should be in prefix order
            void WriteNode(PropertyTree node,
                           ParsedLayout layout,
                           string prefix,
                           TextWriter tw,
                           int level) {
                if (level == 6 || !node.HasChildren)
                    return;

                string nodeName = node.Name;
                if (level == 0) {
                    nodeName = char.ToUpperInvariant(nodeName[0]) + nodeName.Substring(1);
                }
                string property = prefix + nodeName;

                StringBuilder s = new StringBuilder();

                foreach (var child in node.Children) {
                    if (child.IsPropertyTree) {
                        WriteNode((PropertyTree) child, layout, property + ".", tw, level + 1);

                    } else {
                        s.Append(child.Name)
                            .Append("=")
                            .Append(child.Value)
                            .Append(s.Length > 0 ? "; " : string.Empty);
                    }
                }

                if (s.Length > 0)
                    layout.WriteVar(tw, property, s.ToString(), false);
            }

            static void WriteCore(ParsedLayout layout, object value, string property, TextWriter tw) {
                if (value == null) {
                    layout.WriteVar(tw, property, "<null>", false);

                } else {
                    bool escape = IsEscapedType(value.GetType());
                    layout.WriteVar(tw, property, value.ToString(), escape);
                }
            }

        }

        sealed class VariableSourceExpr : VariableExpr {

            protected override string Evaluate(LoggerEvent evt) {
                return evt.Source;
            }
        }

        sealed class VariableExceptionExpr : VariableExpr {

            public override bool Escape { get { return true; } }

            protected override string Evaluate(LoggerEvent evt) {
                if (evt.Exception == null)
                    return string.Empty;
                else
                    return evt.Exception.ToString(this.Format);
            }
        }

        sealed class VariableTimeStampExpr : VariableExpr {

            protected override string Evaluate(LoggerEvent evt) {
                DateTime dt = evt.TimeStamp;
                return FormatDateTime(dt);
            }
        }

        sealed class VariableStackFrameExpr : VariableExpr {

            public override bool Escape { get { return true; } }

            protected override string Evaluate(LoggerEvent evt) {
                return evt.StackFrame.ToString();
            }
        }

        sealed class VariableLevelExpr : VariableExpr {

            public override string Format {
                get { return base.Format; }
                set {
                    if (!string.IsNullOrEmpty(value) && value.Length == 1
                        && Array.IndexOf(LoggerLevel.VALID_FORMATS, char.ToLowerInvariant(value[0])) >= 0)
                        base.Format = value;
                }
            }

            protected override string Evaluate(LoggerEvent evt) {
                if (evt.Level == null)
                    return string.Empty;

                return evt.Level.ToString(this.Format);
            }
        }

        sealed class VariableMessageExpr : VariableExpr {

            public override bool Escape { get { return true; } }

            protected override string Evaluate(LoggerEvent evt) {
                return evt.FormatMessage();
            }

        }

        sealed class VariableAppDomainExpr : VariableExpr {

            protected override string Evaluate(LoggerEvent evt) {
                return evt.AppDomainName;
            }
        }

        sealed class VariableThreadNameExpr : VariableExpr {

            protected override string Evaluate(LoggerEvent evt) {
                return evt.ThreadName;
            }

        }

        sealed class VariableLogNameExpr : VariableExpr {
            protected override string Evaluate(LoggerEvent evt) {
                return evt.SourceLogger.Name;
            }
        }

        sealed class VariableLogIndexExpr : VariableExpr {

            private readonly bool offset;

            public VariableLogIndexExpr(bool offset) {
                this.offset = offset;
            }

            protected override string Evaluate(LoggerEvent evt) {
                FileTarget ft = evt.SourceLogger.File;
                int index = 0;
                if (ft != null)
                    index = ft.Index + (offset ? 1 : 0);

                return index.ToString(this.Format);
            }
        }

        sealed class VariableLogTimeStampExpr : VariableExpr {

            protected override string Evaluate(LoggerEvent evt) {
                if (evt.SourceLogger == null || evt.SourceLogger.File == null)
                    return string.Empty;

                FileTarget ft = evt.SourceLogger.File;
                return FormatDateTime(ft.IndexTimeStamp);
            }

        }
    }
}
