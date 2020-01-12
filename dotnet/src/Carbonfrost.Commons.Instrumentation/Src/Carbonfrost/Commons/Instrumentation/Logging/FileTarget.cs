//
// - FileTarget.cs -
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
using System.Security;
using Carbonfrost.Commons.Instrumentation.Configuration;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Instrumentation.Logging {

    [Builder(typeof(FileTargetBuilder))]
    [TargetUsage(Name = "file")]
    public class FileTarget : StreamingTarget, IHasParsedLayout {

        internal const string DEFAULT_FILE_NAME = "{Application}.{Date}.log";

        private Stream currentOutput;
        private ParsedLayout fileNamePattern;
        private Uri baseUri;

        public bool CreateDirectories { get; private set; }

        public string FileName { get; private set; }
        public FileMode Mode { get; private set; }
        public FileRolloverMode RolloverMode { get; private set; }
        public int RolloverMaxFileSize { get; private set; }
        public TimeSpan RolloverMaxAge { get; private set; }
        public Renderer Renderer { get; private set; }

        internal int Index { get; private set; }
        internal DateTime IndexTimeStamp { get; private set; }

        // IHasParsedLayout implementation
        ParsedLayout IHasParsedLayout.ParsedLayout {
            get {
                var text = Renderer as TextRenderer;
                if (text != null) {
                    return text.ParsedLayout;
                }
                return null;
            }
        }

        internal FileTarget(FileTargetBuilder builder, IServiceProvider sp)
            : base(builder) {

            this.CreateDirectories = builder.CreateDirectories;
            this.FileName = builder.FileName;
            this.Mode = builder.Mode;
            this.RolloverMode = builder.Rollover.Mode;
            this.RolloverMaxFileSize = builder.Rollover.MaxFileSize;
            this.RolloverMaxAge = builder.Rollover.MaxAge;
            this.Renderer = builder.Renderer ?? builder.DefaultRenderer;
            this.Index = -1;
            this.baseUri = ((IUriContext) builder).BaseUri;
            this.fileNamePattern = ParsedLayout.ParseFileName(this.FileName);
        }

        private string GetFullName() {
            // A little tacky, but we need an event
            LoggerEvent dummy = new LoggerEvent();
            dummy.Level = LoggerLevel.Trace;
            dummy.SourceLogger = this.Logger;
            string fileName = Utility.SafeFilePath(this.fileNamePattern.Expand(dummy));

            // Remote configuration cannot specify a URI context
            try {
                if (baseUri == null || !baseUri.IsFile)
                    return Path.GetFullPath(fileName);
                else
                    return Path.Combine(baseUri.LocalPath, fileName);
            } catch (Exception) {
                throw new NotImplementedException(fileName);
            }
        }

        // Target overrides
        protected override Stream CreateOutputStream() {
            this.Index++;
            this.IndexTimeStamp = DateTime.Now;

            if (currentOutput != null) {
                this.Renderer.EndDocument();
                this.Renderer.Flush();
                currentOutput.Flush();
                currentOutput.Dispose();
            }

            Stream s = CreateStreamHelper();
            this.currentOutput = s;
            this.Renderer.ChangeStream(s, this.AutoFlush);
            this.Renderer.StartDocument();

            return s;
        }

        Stream CreateStreamHelper() {
            try {
                string fileName = GetFullName();
                if (this.CreateDirectories) {
                    string str = Path.GetDirectoryName(fileName);
                    if (!string.IsNullOrEmpty(str) && !Directory.Exists(str)) {
                        Directory.CreateDirectory(str);
                    }
                }

                FileStream s;
                if (this.BufferSize == 0) {
                    s = new FileStream(fileName, this.Mode, FileAccess.Write, FileShare.Read);
                } else {
                    s = new FileStream(fileName, this.Mode, FileAccess.Write, FileShare.Read, this.BufferSize);
                }

                return s;

            } catch (UnauthorizedAccessException a) {
                Traceables.FileTargetCouldNotCreateOutputStream(a, this.Name);

            } catch (SecurityException e) {
                Traceables.FileTargetCouldNotCreateOutputStream(e, this.Name);

            } catch (IOException ie) {
                Traceables.FileTargetCouldNotCreateOutputStream(ie, this.Name);
            }

            return Stream.Null;
        }

        protected override void FlushCore(int timeout) {
            this.Renderer.Flush();
            base.FlushCore(timeout);
        }

        public override void Initialize() {
            this.CreateOutputStream();
        }

        protected override void Dispose(bool manualDispose) {
            if (manualDispose) {
                this.Renderer.Flush();
                this.Renderer.Dispose();
            }

            base.Dispose(manualDispose);
        }

        protected override void WriteCore(LoggerEvent[] buffer, int index, int length) {
            // TODO Need to control size for rollover, so probably write each one
            this.Renderer.Render(buffer, index, length);
            this.Renderer.Flush();

            if (!this.KeepOpen) {
                // TODO Support keep file open, AutoFlush
            }
        }

    }
}
