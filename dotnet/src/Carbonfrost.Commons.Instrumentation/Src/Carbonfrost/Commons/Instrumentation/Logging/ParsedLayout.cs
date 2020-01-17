//
// Copyright 2010, 2012, 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Instrumentation.Logging {

    internal partial class ParsedLayout {

        private readonly Expr[] expressions;

        internal static readonly ParsedLayout Default
            = ParsedLayout.Parse(TextRenderer.DEFAULT_LAYOUT, LayoutMode.Default, new DefaultFactory());

        static readonly HashSet<Type> NOT_ESCAPED = new HashSet<Type>() {
            typeof(Int32), typeof(Int64), typeof(Int16),
            typeof(UInt32), typeof(UInt64), typeof(UInt16),
            typeof(Boolean), typeof(Char), typeof(SByte), typeof(Byte),

            // These are included since they don't contain quotes
            typeof(IPAddress),
            typeof(Uri),
            typeof(DateTime),
            typeof(TimeSpan),
        };

        public string Header { get; set; }
        public string Layout { get; private set; }
        public string Footer { get; set; }
        public LayoutMode LayoutMode { get; set; }

        public IEnumerable<string> DataExpressions {
            get {
                return expressions.OfType<DataExprBase>().Select(t => t.VarName);
            }
        }

        public ParsedLayout(Expr[] expressions) {
            this.expressions = expressions;
        }

        internal static ParsedLayout ParseFileName(string text) {
            var factory = new FileNameGeneratorFactory();
            var exprs = new LayoutParser(factory).Parse(text);
            var result = new FileNameLayout(exprs);
            result.Layout = text;

            return result;
        }

        static ParsedLayout Parse(string text, LayoutMode mode, ExprFactory factory) {
            var exprs = new LayoutParser(factory).Parse(text);
            var result = ParseCore(exprs, mode);
            result.Layout = text;

            return result;
        }

        internal static ParsedLayout Parse(string text, LayoutMode mode) {
            return Parse(text, mode, new DefaultFactory());
        }

        static ParsedLayout ParseCore(IList<Expr> exprs, LayoutMode mode) {
            switch (mode) {
                case LayoutMode.Vertical:
                    return new VerticalLayout(exprs);
                case LayoutMode.Default:
                    return new ParsedLayout(exprs.ToArray());
                case LayoutMode.Escaped:
                default:
                    return new EscapedLayout(exprs);
            }
        }

        static bool IsEscapedType(Type type) {
            return !NOT_ESCAPED.Contains(type);
        }

        public string Expand(LoggerEvent evt) {
            StringWriter sw = new StringWriter();
            WriteBody(sw, evt);
            return sw.ToString();
        }

        public void WriteHeader(TextWriter writer) {
            writer.Write(Header);
        }

        public void WriteFooter(TextWriter writer) {
            writer.Write(Footer);
        }

        public void WriteBody(TextWriter writer, LoggerEvent evt) {
            foreach (Expr e in this.expressions) {
                e.Write(this, writer, evt);
            }
        }

        internal virtual void WriteVar(TextWriter writer, string name, string value, bool escape) {
            writer.Write(value);
        }

        internal virtual void WriteLiteral(TextWriter writer, string value) {
            writer.Write(value);
        }

        internal virtual void WriteLine(TextWriter writer) {
            writer.WriteLine();
        }

        sealed class FileNameLayout : ParsedLayout {

            public FileNameLayout(IList<Expr> expressions)
                : base(expressions.ToArray()) {
            }

            internal override void WriteLiteral(TextWriter writer, string value) {
                writer.Write(Environment.ExpandEnvironmentVariables(value));
            }

            internal override void WriteVar(TextWriter writer, string name, string value, bool escape) {
                WriteSafe(writer, value);
            }

            internal override void WriteLine(TextWriter writer) {
                writer.Write("_");
            }

            void WriteSafe(TextWriter writer, string value) {
                value = Regex.Replace(value, @"[^\w_\.-]+", "_");
                writer.Write(value);
            }
        }

        sealed class VerticalLayout : ParsedLayout {


            public VerticalLayout(IList<Expr> expressions)
                : base(FilterLiterals(expressions).ToArray()) {
            }

            static IEnumerable<Expr> FilterLiterals(IEnumerable<Expr> e) {
                Expr last = e.LastOrDefault();
                Expr retain = last.IsStatic() ? last : Expr.DefaultSeparator;

                return e.Where(t => !t.IsStatic()).Concat(new [] { retain, Expr.NewLine });
            }

            internal override void WriteVar(TextWriter writer, string name, string value, bool escape) {
                writer.Write(name);
                writer.Write("=");
                writer.WriteLine(value);
            }
        }

        sealed class EscapedLayout : ParsedLayout {

            public EscapedLayout(IList<Expr> expressions)
                : base(FilterLiterals(expressions).ToArray()) {
            }

            static IEnumerable<Expr> FilterLiterals(IList<Expr> e) {
                foreach (var s in e) {
                    LiteralExpr le = s as LiteralExpr;
                    if (le != null) {
                        yield return le.Trim();
                        continue;
                    }

                    yield return s;
                }
            }

            internal override void WriteVar(TextWriter writer, string name, string value, bool escape) {
                if (escape)
                    writer.Write('"');

                string eval = new StringBuilder(value)
                    .Replace("\"", "\\\"")
                    .Replace("\n", "\\n")
                    .Replace("\r", "\\r")
                    .ToString();
                writer.Write(eval);

                if (escape)
                    writer.Write('"');
            }

        }
    }
}
