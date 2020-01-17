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
using Carbonfrost.Commons.Instrumentation.Logging;
using Carbonfrost.Commons.Instrumentation.Resources;

namespace Carbonfrost.Commons.Instrumentation {

    static class Traceables {

        public static void RootLoggerDoneInitializing() {
            LogLog.Write(LoggerLevel.Trace, LogErrorCode.Initialized, SR.RootLoggerDoneInitializing());
        }

        public static void RootLoggerInitializing(int loggerCount) {
            LogLog.Write(LoggerLevel.Trace, LogErrorCode.Initializing, SR.RootLoggerInitializing(loggerCount));
        }

        public static void FailedToBuildLogger(string name, Exception ex) {
            LogLog.Write(LoggerLevel.Error, LogErrorCode.FailureOnBuildLogger, SR.FailedToBuildLogger(name, ex));
        }

        public static void InstrumentCaptureFailed(string name, Exception ex) {
            LogLog.Write(LoggerLevel.Error, LogErrorCode.FailureOnInstrumentCapture, SR.InstrumentCaptureFailed(name, ex));
        }

        public static void FinalCaptureFailed(Exception ex) {
            LogLog.Write(LoggerLevel.Error, LogErrorCode.FailureOnFinalCapture, SR.FinalCaptureFailed(ex));
        }

        public static void FileTargetCouldNotCreateOutputStream(Exception ex, string name) {
            LogLog.Write(LoggerLevel.Error, LogErrorCode.FailureCouldNotCreateOutputStream, SR.FileTargetCouldNotCreateOutputStream(name, ex));
        }

        public static void DetectedCycleInLogParents(string name) {
            LogLog.Write(LoggerLevel.Error, LogErrorCode.FailureDetectedCycleForwardingLogger, SR.DetectedCycleInLogParents(name));
        }

        public static void MissingParentLoggerReference(string name) {
            LogLog.Write(LoggerLevel.Error, LogErrorCode.FailureParentReferenceNotFound, SR.MissingParentLoggerReference(name));
        }

        public static void RootCannotHaveParent() {
        LogLog.Write(LoggerLevel.Error, LogErrorCode.FailureRootCannotHaveParent, SR.RootCannotHaveParent());
        }

        public static void CannotSpecifyParentAndTarget(string name) {
        LogLog.Write(LoggerLevel.Error, LogErrorCode.FailureCannotSpecifyParentAndTarget, SR.CannotSpecifyParentAndTarget(name));
        }
    }
}
