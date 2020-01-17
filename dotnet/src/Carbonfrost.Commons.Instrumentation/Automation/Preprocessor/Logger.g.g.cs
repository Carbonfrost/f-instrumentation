
// File generated at 01/15/2020 22:36:55

using System;
using System.Diagnostics;
using System.Globalization;

namespace Carbonfrost.Commons.Instrumentation {


    partial class Log {

        public static bool DebugEnabled {
            get {
                return Enabled(LoggerLevel.Debug);
            }
        }

        [Conditional("DEBUG")] static
        public void Debug(object value, object data = null) {
            Write(LoggerLevel.Debug, value, data);
        }

        [Conditional("DEBUG")] static
        public void DebugEvent(Func<LoggerEvent> eventFactory) {
            WriteEvent(LoggerLevel.Debug, eventFactory);
        }

        [Conditional("DEBUG")] static
        public void Debug(string message, object data = null) {
            Write(LoggerLevel.Debug, message, data);
        }

        [Conditional("DEBUG")] static
        public void Debug(string message, Exception exception, object data = null) {
            Write(LoggerLevel.Debug, message, exception, data);
        }

        [Conditional("DEBUG")] static
        public void DebugFormat(string format, params object[] args) {
            WriteFormat(LoggerLevel.Debug, format, (object[]) args);
        }

        [Conditional("DEBUG")] static
        public void DebugFormat(string format, object arg0) {
            WriteFormat(LoggerLevel.Debug, format, arg0);
        }

        [Conditional("DEBUG")] static
        public void DebugFormat(IFormatProvider formatProvider, string format, object arg0) {
            WriteFormat(LoggerLevel.Debug, formatProvider, format, arg0);
        }

        [Conditional("DEBUG")] static
        public void DebugFormat(IFormatProvider formatProvider, string format, params object[] args) {
            WriteFormat(LoggerLevel.Debug, formatProvider, format, (object[]) args);
        }

        [Conditional("DEBUG")] static
        public void DebugFormat(string format, object arg0, object arg1) {
            WriteFormat(LoggerLevel.Debug, format, arg0, arg1);
        }

        [Conditional("DEBUG")] static
        public void DebugFormat(IFormatProvider formatProvider, string format, object arg0, object arg1)  {
            WriteFormat(LoggerLevel.Debug, formatProvider, format, arg0, arg1);
        }

        [Conditional("DEBUG")] static
        public void DebugFormat(string format, object arg0, object arg1, object arg2) {
            WriteFormat(LoggerLevel.Debug, format, arg0, arg1, arg2);
        }

        [Conditional("DEBUG")] static
        public void DebugFormat(IFormatProvider formatProvider, string format, object arg0, object arg1, object arg2) {
            WriteFormat(LoggerLevel.Debug, formatProvider, format, arg0, arg1, arg2);
        }
        public static bool TraceEnabled {
            get {
                return Enabled(LoggerLevel.Trace);
            }
        }

        [Conditional("TRACE")] static
        public void Trace(object value, object data = null) {
            Write(LoggerLevel.Trace, value, data);
        }

        [Conditional("TRACE")] static
        public void TraceEvent(Func<LoggerEvent> eventFactory) {
            WriteEvent(LoggerLevel.Trace, eventFactory);
        }

        [Conditional("TRACE")] static
        public void Trace(string message, object data = null) {
            Write(LoggerLevel.Trace, message, data);
        }

        [Conditional("TRACE")] static
        public void Trace(string message, Exception exception, object data = null) {
            Write(LoggerLevel.Trace, message, exception, data);
        }

        [Conditional("TRACE")] static
        public void TraceFormat(string format, params object[] args) {
            WriteFormat(LoggerLevel.Trace, format, (object[]) args);
        }

        [Conditional("TRACE")] static
        public void TraceFormat(string format, object arg0) {
            WriteFormat(LoggerLevel.Trace, format, arg0);
        }

        [Conditional("TRACE")] static
        public void TraceFormat(IFormatProvider formatProvider, string format, object arg0) {
            WriteFormat(LoggerLevel.Trace, formatProvider, format, arg0);
        }

        [Conditional("TRACE")] static
        public void TraceFormat(IFormatProvider formatProvider, string format, params object[] args) {
            WriteFormat(LoggerLevel.Trace, formatProvider, format, (object[]) args);
        }

        [Conditional("TRACE")] static
        public void TraceFormat(string format, object arg0, object arg1) {
            WriteFormat(LoggerLevel.Trace, format, arg0, arg1);
        }

        [Conditional("TRACE")] static
        public void TraceFormat(IFormatProvider formatProvider, string format, object arg0, object arg1)  {
            WriteFormat(LoggerLevel.Trace, formatProvider, format, arg0, arg1);
        }

        [Conditional("TRACE")] static
        public void TraceFormat(string format, object arg0, object arg1, object arg2) {
            WriteFormat(LoggerLevel.Trace, format, arg0, arg1, arg2);
        }

        [Conditional("TRACE")] static
        public void TraceFormat(IFormatProvider formatProvider, string format, object arg0, object arg1, object arg2) {
            WriteFormat(LoggerLevel.Trace, formatProvider, format, arg0, arg1, arg2);
        }
        public static bool InfoEnabled {
            get {
                return Enabled(LoggerLevel.Info);
            }
        }

         static
        public void Info(object value, object data = null) {
            Write(LoggerLevel.Info, value, data);
        }

         static
        public void InfoEvent(Func<LoggerEvent> eventFactory) {
            WriteEvent(LoggerLevel.Info, eventFactory);
        }

         static
        public void Info(string message, object data = null) {
            Write(LoggerLevel.Info, message, data);
        }

         static
        public void Info(string message, Exception exception, object data = null) {
            Write(LoggerLevel.Info, message, exception, data);
        }

         static
        public void InfoFormat(string format, params object[] args) {
            WriteFormat(LoggerLevel.Info, format, (object[]) args);
        }

         static
        public void InfoFormat(string format, object arg0) {
            WriteFormat(LoggerLevel.Info, format, arg0);
        }

         static
        public void InfoFormat(IFormatProvider formatProvider, string format, object arg0) {
            WriteFormat(LoggerLevel.Info, formatProvider, format, arg0);
        }

         static
        public void InfoFormat(IFormatProvider formatProvider, string format, params object[] args) {
            WriteFormat(LoggerLevel.Info, formatProvider, format, (object[]) args);
        }

         static
        public void InfoFormat(string format, object arg0, object arg1) {
            WriteFormat(LoggerLevel.Info, format, arg0, arg1);
        }

         static
        public void InfoFormat(IFormatProvider formatProvider, string format, object arg0, object arg1)  {
            WriteFormat(LoggerLevel.Info, formatProvider, format, arg0, arg1);
        }

         static
        public void InfoFormat(string format, object arg0, object arg1, object arg2) {
            WriteFormat(LoggerLevel.Info, format, arg0, arg1, arg2);
        }

         static
        public void InfoFormat(IFormatProvider formatProvider, string format, object arg0, object arg1, object arg2) {
            WriteFormat(LoggerLevel.Info, formatProvider, format, arg0, arg1, arg2);
        }
        public static bool WarnEnabled {
            get {
                return Enabled(LoggerLevel.Warn);
            }
        }

         static
        public void Warn(object value, object data = null) {
            Write(LoggerLevel.Warn, value, data);
        }

         static
        public void WarnEvent(Func<LoggerEvent> eventFactory) {
            WriteEvent(LoggerLevel.Warn, eventFactory);
        }

         static
        public void Warn(string message, object data = null) {
            Write(LoggerLevel.Warn, message, data);
        }

         static
        public void Warn(string message, Exception exception, object data = null) {
            Write(LoggerLevel.Warn, message, exception, data);
        }

         static
        public void WarnFormat(string format, params object[] args) {
            WriteFormat(LoggerLevel.Warn, format, (object[]) args);
        }

         static
        public void WarnFormat(string format, object arg0) {
            WriteFormat(LoggerLevel.Warn, format, arg0);
        }

         static
        public void WarnFormat(IFormatProvider formatProvider, string format, object arg0) {
            WriteFormat(LoggerLevel.Warn, formatProvider, format, arg0);
        }

         static
        public void WarnFormat(IFormatProvider formatProvider, string format, params object[] args) {
            WriteFormat(LoggerLevel.Warn, formatProvider, format, (object[]) args);
        }

         static
        public void WarnFormat(string format, object arg0, object arg1) {
            WriteFormat(LoggerLevel.Warn, format, arg0, arg1);
        }

         static
        public void WarnFormat(IFormatProvider formatProvider, string format, object arg0, object arg1)  {
            WriteFormat(LoggerLevel.Warn, formatProvider, format, arg0, arg1);
        }

         static
        public void WarnFormat(string format, object arg0, object arg1, object arg2) {
            WriteFormat(LoggerLevel.Warn, format, arg0, arg1, arg2);
        }

         static
        public void WarnFormat(IFormatProvider formatProvider, string format, object arg0, object arg1, object arg2) {
            WriteFormat(LoggerLevel.Warn, formatProvider, format, arg0, arg1, arg2);
        }
        public static bool ErrorEnabled {
            get {
                return Enabled(LoggerLevel.Error);
            }
        }

         static
        public void Error(object value, object data = null) {
            Write(LoggerLevel.Error, value, data);
        }

         static
        public void ErrorEvent(Func<LoggerEvent> eventFactory) {
            WriteEvent(LoggerLevel.Error, eventFactory);
        }

         static
        public void Error(string message, object data = null) {
            Write(LoggerLevel.Error, message, data);
        }

         static
        public void Error(string message, Exception exception, object data = null) {
            Write(LoggerLevel.Error, message, exception, data);
        }

         static
        public void ErrorFormat(string format, params object[] args) {
            WriteFormat(LoggerLevel.Error, format, (object[]) args);
        }

         static
        public void ErrorFormat(string format, object arg0) {
            WriteFormat(LoggerLevel.Error, format, arg0);
        }

         static
        public void ErrorFormat(IFormatProvider formatProvider, string format, object arg0) {
            WriteFormat(LoggerLevel.Error, formatProvider, format, arg0);
        }

         static
        public void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args) {
            WriteFormat(LoggerLevel.Error, formatProvider, format, (object[]) args);
        }

         static
        public void ErrorFormat(string format, object arg0, object arg1) {
            WriteFormat(LoggerLevel.Error, format, arg0, arg1);
        }

         static
        public void ErrorFormat(IFormatProvider formatProvider, string format, object arg0, object arg1)  {
            WriteFormat(LoggerLevel.Error, formatProvider, format, arg0, arg1);
        }

         static
        public void ErrorFormat(string format, object arg0, object arg1, object arg2) {
            WriteFormat(LoggerLevel.Error, format, arg0, arg1, arg2);
        }

         static
        public void ErrorFormat(IFormatProvider formatProvider, string format, object arg0, object arg1, object arg2) {
            WriteFormat(LoggerLevel.Error, formatProvider, format, arg0, arg1, arg2);
        }
        public static bool FatalEnabled {
            get {
                return Enabled(LoggerLevel.Fatal);
            }
        }

         static
        public void Fatal(object value, object data = null) {
            Write(LoggerLevel.Fatal, value, data);
        }

         static
        public void FatalEvent(Func<LoggerEvent> eventFactory) {
            WriteEvent(LoggerLevel.Fatal, eventFactory);
        }

         static
        public void Fatal(string message, object data = null) {
            Write(LoggerLevel.Fatal, message, data);
        }

         static
        public void Fatal(string message, Exception exception, object data = null) {
            Write(LoggerLevel.Fatal, message, exception, data);
        }

         static
        public void FatalFormat(string format, params object[] args) {
            WriteFormat(LoggerLevel.Fatal, format, (object[]) args);
        }

         static
        public void FatalFormat(string format, object arg0) {
            WriteFormat(LoggerLevel.Fatal, format, arg0);
        }

         static
        public void FatalFormat(IFormatProvider formatProvider, string format, object arg0) {
            WriteFormat(LoggerLevel.Fatal, formatProvider, format, arg0);
        }

         static
        public void FatalFormat(IFormatProvider formatProvider, string format, params object[] args) {
            WriteFormat(LoggerLevel.Fatal, formatProvider, format, (object[]) args);
        }

         static
        public void FatalFormat(string format, object arg0, object arg1) {
            WriteFormat(LoggerLevel.Fatal, format, arg0, arg1);
        }

         static
        public void FatalFormat(IFormatProvider formatProvider, string format, object arg0, object arg1)  {
            WriteFormat(LoggerLevel.Fatal, formatProvider, format, arg0, arg1);
        }

         static
        public void FatalFormat(string format, object arg0, object arg1, object arg2) {
            WriteFormat(LoggerLevel.Fatal, format, arg0, arg1, arg2);
        }

         static
        public void FatalFormat(IFormatProvider formatProvider, string format, object arg0, object arg1, object arg2) {
            WriteFormat(LoggerLevel.Fatal, formatProvider, format, arg0, arg1, arg2);
        }

    }


    partial class Logger {

        public bool DebugEnabled {
            get {
                return Enabled(LoggerLevel.Debug);
            }
        }

        [Conditional("DEBUG")]
        public void Debug(object value, object data = null) {
            Log(LoggerLevel.Debug, value, data);
        }

        [Conditional("DEBUG")]
        public void DebugEvent(Func<LoggerEvent> eventFactory) {
            LogEvent(LoggerLevel.Debug, eventFactory);
        }

        [Conditional("DEBUG")]
        public void Debug(string message, object data = null) {
            Log(LoggerLevel.Debug, message, data);
        }

        [Conditional("DEBUG")]
        public void Debug(string message, Exception exception, object data = null) {
            Log(LoggerLevel.Debug, message, exception, data);
        }

        [Conditional("DEBUG")]
        public void DebugFormat(string format, params object[] args) {
            LogFormat(LoggerLevel.Debug, format, (object[]) args);
        }

        [Conditional("DEBUG")]
        public void DebugFormat(string format, object arg0) {
            LogFormat(LoggerLevel.Debug, format, arg0);
        }

        [Conditional("DEBUG")]
        public void DebugFormat(IFormatProvider formatProvider, string format, object arg0) {
            LogFormat(LoggerLevel.Debug, formatProvider, format, arg0);
        }

        [Conditional("DEBUG")]
        public void DebugFormat(IFormatProvider formatProvider, string format, params object[] args) {
            LogFormat(LoggerLevel.Debug, formatProvider, format, (object[]) args);
        }

        [Conditional("DEBUG")]
        public void DebugFormat(string format, object arg0, object arg1) {
            LogFormat(LoggerLevel.Debug, format, arg0, arg1);
        }

        [Conditional("DEBUG")]
        public void DebugFormat(IFormatProvider formatProvider, string format, object arg0, object arg1)  {
            LogFormat(LoggerLevel.Debug, formatProvider, format, arg0, arg1);
        }

        [Conditional("DEBUG")]
        public void DebugFormat(string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Debug, format, arg0, arg1, arg2);
        }

        [Conditional("DEBUG")]
        public void DebugFormat(IFormatProvider formatProvider, string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Debug, formatProvider, format, arg0, arg1, arg2);
        }
        public bool TraceEnabled {
            get {
                return Enabled(LoggerLevel.Trace);
            }
        }

        [Conditional("TRACE")]
        public void Trace(object value, object data = null) {
            Log(LoggerLevel.Trace, value, data);
        }

        [Conditional("TRACE")]
        public void TraceEvent(Func<LoggerEvent> eventFactory) {
            LogEvent(LoggerLevel.Trace, eventFactory);
        }

        [Conditional("TRACE")]
        public void Trace(string message, object data = null) {
            Log(LoggerLevel.Trace, message, data);
        }

        [Conditional("TRACE")]
        public void Trace(string message, Exception exception, object data = null) {
            Log(LoggerLevel.Trace, message, exception, data);
        }

        [Conditional("TRACE")]
        public void TraceFormat(string format, params object[] args) {
            LogFormat(LoggerLevel.Trace, format, (object[]) args);
        }

        [Conditional("TRACE")]
        public void TraceFormat(string format, object arg0) {
            LogFormat(LoggerLevel.Trace, format, arg0);
        }

        [Conditional("TRACE")]
        public void TraceFormat(IFormatProvider formatProvider, string format, object arg0) {
            LogFormat(LoggerLevel.Trace, formatProvider, format, arg0);
        }

        [Conditional("TRACE")]
        public void TraceFormat(IFormatProvider formatProvider, string format, params object[] args) {
            LogFormat(LoggerLevel.Trace, formatProvider, format, (object[]) args);
        }

        [Conditional("TRACE")]
        public void TraceFormat(string format, object arg0, object arg1) {
            LogFormat(LoggerLevel.Trace, format, arg0, arg1);
        }

        [Conditional("TRACE")]
        public void TraceFormat(IFormatProvider formatProvider, string format, object arg0, object arg1)  {
            LogFormat(LoggerLevel.Trace, formatProvider, format, arg0, arg1);
        }

        [Conditional("TRACE")]
        public void TraceFormat(string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Trace, format, arg0, arg1, arg2);
        }

        [Conditional("TRACE")]
        public void TraceFormat(IFormatProvider formatProvider, string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Trace, formatProvider, format, arg0, arg1, arg2);
        }
        public bool InfoEnabled {
            get {
                return Enabled(LoggerLevel.Info);
            }
        }

        
        public void Info(object value, object data = null) {
            Log(LoggerLevel.Info, value, data);
        }

        
        public void InfoEvent(Func<LoggerEvent> eventFactory) {
            LogEvent(LoggerLevel.Info, eventFactory);
        }

        
        public void Info(string message, object data = null) {
            Log(LoggerLevel.Info, message, data);
        }

        
        public void Info(string message, Exception exception, object data = null) {
            Log(LoggerLevel.Info, message, exception, data);
        }

        
        public void InfoFormat(string format, params object[] args) {
            LogFormat(LoggerLevel.Info, format, (object[]) args);
        }

        
        public void InfoFormat(string format, object arg0) {
            LogFormat(LoggerLevel.Info, format, arg0);
        }

        
        public void InfoFormat(IFormatProvider formatProvider, string format, object arg0) {
            LogFormat(LoggerLevel.Info, formatProvider, format, arg0);
        }

        
        public void InfoFormat(IFormatProvider formatProvider, string format, params object[] args) {
            LogFormat(LoggerLevel.Info, formatProvider, format, (object[]) args);
        }

        
        public void InfoFormat(string format, object arg0, object arg1) {
            LogFormat(LoggerLevel.Info, format, arg0, arg1);
        }

        
        public void InfoFormat(IFormatProvider formatProvider, string format, object arg0, object arg1)  {
            LogFormat(LoggerLevel.Info, formatProvider, format, arg0, arg1);
        }

        
        public void InfoFormat(string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Info, format, arg0, arg1, arg2);
        }

        
        public void InfoFormat(IFormatProvider formatProvider, string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Info, formatProvider, format, arg0, arg1, arg2);
        }
        public bool WarnEnabled {
            get {
                return Enabled(LoggerLevel.Warn);
            }
        }

        
        public void Warn(object value, object data = null) {
            Log(LoggerLevel.Warn, value, data);
        }

        
        public void WarnEvent(Func<LoggerEvent> eventFactory) {
            LogEvent(LoggerLevel.Warn, eventFactory);
        }

        
        public void Warn(string message, object data = null) {
            Log(LoggerLevel.Warn, message, data);
        }

        
        public void Warn(string message, Exception exception, object data = null) {
            Log(LoggerLevel.Warn, message, exception, data);
        }

        
        public void WarnFormat(string format, params object[] args) {
            LogFormat(LoggerLevel.Warn, format, (object[]) args);
        }

        
        public void WarnFormat(string format, object arg0) {
            LogFormat(LoggerLevel.Warn, format, arg0);
        }

        
        public void WarnFormat(IFormatProvider formatProvider, string format, object arg0) {
            LogFormat(LoggerLevel.Warn, formatProvider, format, arg0);
        }

        
        public void WarnFormat(IFormatProvider formatProvider, string format, params object[] args) {
            LogFormat(LoggerLevel.Warn, formatProvider, format, (object[]) args);
        }

        
        public void WarnFormat(string format, object arg0, object arg1) {
            LogFormat(LoggerLevel.Warn, format, arg0, arg1);
        }

        
        public void WarnFormat(IFormatProvider formatProvider, string format, object arg0, object arg1)  {
            LogFormat(LoggerLevel.Warn, formatProvider, format, arg0, arg1);
        }

        
        public void WarnFormat(string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Warn, format, arg0, arg1, arg2);
        }

        
        public void WarnFormat(IFormatProvider formatProvider, string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Warn, formatProvider, format, arg0, arg1, arg2);
        }
        public bool ErrorEnabled {
            get {
                return Enabled(LoggerLevel.Error);
            }
        }

        
        public void Error(object value, object data = null) {
            Log(LoggerLevel.Error, value, data);
        }

        
        public void ErrorEvent(Func<LoggerEvent> eventFactory) {
            LogEvent(LoggerLevel.Error, eventFactory);
        }

        
        public void Error(string message, object data = null) {
            Log(LoggerLevel.Error, message, data);
        }

        
        public void Error(string message, Exception exception, object data = null) {
            Log(LoggerLevel.Error, message, exception, data);
        }

        
        public void ErrorFormat(string format, params object[] args) {
            LogFormat(LoggerLevel.Error, format, (object[]) args);
        }

        
        public void ErrorFormat(string format, object arg0) {
            LogFormat(LoggerLevel.Error, format, arg0);
        }

        
        public void ErrorFormat(IFormatProvider formatProvider, string format, object arg0) {
            LogFormat(LoggerLevel.Error, formatProvider, format, arg0);
        }

        
        public void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args) {
            LogFormat(LoggerLevel.Error, formatProvider, format, (object[]) args);
        }

        
        public void ErrorFormat(string format, object arg0, object arg1) {
            LogFormat(LoggerLevel.Error, format, arg0, arg1);
        }

        
        public void ErrorFormat(IFormatProvider formatProvider, string format, object arg0, object arg1)  {
            LogFormat(LoggerLevel.Error, formatProvider, format, arg0, arg1);
        }

        
        public void ErrorFormat(string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Error, format, arg0, arg1, arg2);
        }

        
        public void ErrorFormat(IFormatProvider formatProvider, string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Error, formatProvider, format, arg0, arg1, arg2);
        }
        public bool FatalEnabled {
            get {
                return Enabled(LoggerLevel.Fatal);
            }
        }

        
        public void Fatal(object value, object data = null) {
            Log(LoggerLevel.Fatal, value, data);
        }

        
        public void FatalEvent(Func<LoggerEvent> eventFactory) {
            LogEvent(LoggerLevel.Fatal, eventFactory);
        }

        
        public void Fatal(string message, object data = null) {
            Log(LoggerLevel.Fatal, message, data);
        }

        
        public void Fatal(string message, Exception exception, object data = null) {
            Log(LoggerLevel.Fatal, message, exception, data);
        }

        
        public void FatalFormat(string format, params object[] args) {
            LogFormat(LoggerLevel.Fatal, format, (object[]) args);
        }

        
        public void FatalFormat(string format, object arg0) {
            LogFormat(LoggerLevel.Fatal, format, arg0);
        }

        
        public void FatalFormat(IFormatProvider formatProvider, string format, object arg0) {
            LogFormat(LoggerLevel.Fatal, formatProvider, format, arg0);
        }

        
        public void FatalFormat(IFormatProvider formatProvider, string format, params object[] args) {
            LogFormat(LoggerLevel.Fatal, formatProvider, format, (object[]) args);
        }

        
        public void FatalFormat(string format, object arg0, object arg1) {
            LogFormat(LoggerLevel.Fatal, format, arg0, arg1);
        }

        
        public void FatalFormat(IFormatProvider formatProvider, string format, object arg0, object arg1)  {
            LogFormat(LoggerLevel.Fatal, formatProvider, format, arg0, arg1);
        }

        
        public void FatalFormat(string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Fatal, format, arg0, arg1, arg2);
        }

        
        public void FatalFormat(IFormatProvider formatProvider, string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Fatal, formatProvider, format, arg0, arg1, arg2);
        }

    }


}
