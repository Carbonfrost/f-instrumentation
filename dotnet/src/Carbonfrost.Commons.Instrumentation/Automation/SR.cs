
// This file was automatically generated.  DO NOT EDIT or else
// your changes could be lost!

#pragma warning disable 1570

using System;
using System.Globalization;
using System.Resources;
using System.Reflection;

namespace Carbonfrost.Commons.Instrumentation.Resources {

    /// <summary>
    /// Contains strongly-typed string resources.
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("srgen", "1.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
    internal static partial class SR {

        private static global::System.Resources.ResourceManager _resources;
        private static global::System.Globalization.CultureInfo _currentCulture;
        private static global::System.Func<string, string> _resourceFinder;

        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(_resources, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Carbonfrost.Commons.Instrumentation.Automation.SR", typeof(SR).GetTypeInfo().Assembly);
                    _resources = temp;
                }
                return _resources;
            }
        }

        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return _currentCulture;
            }
            set {
                _currentCulture = value;
            }
        }

        private static global::System.Func<string, string> ResourceFinder {
            get {
                if (object.ReferenceEquals(_resourceFinder, null)) {
                    try {
                        global::System.Resources.ResourceManager rm = ResourceManager;
                        _resourceFinder = delegate (string s) {
                            return rm.GetString(s);
                        };
                    } catch (global::System.Exception ex) {
                        _resourceFinder = delegate (string s) {
                            return string.Format("localization error! {0}: {1} ({2})", s, ex.GetType(), ex.Message);
                        };
                    }
                }
                return _resourceFinder;
            }
        }


  /// <summary>Cannot remove the root logger from the collection.</summary>
    internal static string CannotRemoveRoot(
    
    ) {
        return string.Format(Culture, ResourceFinder("CannotRemoveRoot") );
    }

  /// <summary>Cannot specify a target if the logger is has also specified a parent logger for redirection: ${name}</summary>
    internal static string CannotSpecifyParentAndTarget(
    object @name
    ) {
        return string.Format(Culture, ResourceFinder("CannotSpecifyParentAndTarget") , @name);
    }

  /// <summary>The specified connection string name refers to a connection string that does not exist or is blank: `${name}'.</summary>
    internal static string ConnectionStringEmpty(
    object @name
    ) {
        return string.Format(Culture, ResourceFinder("ConnectionStringEmpty") , @name);
    }

  /// <summary>A cycle was detected in forwarding the logger `${name}'.</summary>
    internal static string DetectedCycleInLogParents(
    object @name
    ) {
        return string.Format(Culture, ResourceFinder("DetectedCycleInLogParents") , @name);
    }

  /// <summary>Failed to initialize the logger `${loggerName}': ${exceptionMessage}</summary>
    internal static string FailedToBuildLogger(
    object @loggerName, object @exceptionMessage
    ) {
        return string.Format(Culture, ResourceFinder("FailedToBuildLogger") , @loggerName, @exceptionMessage);
    }

  /// <summary>Could not create the output stream for the file target `${name}': ${error}</summary>
    internal static string FileTargetCouldNotCreateOutputStream(
    object @name, object @error
    ) {
        return string.Format(Culture, ResourceFinder("FileTargetCouldNotCreateOutputStream") , @name, @error);
    }

  /// <summary>Writing to the logger failed.  Subsequent writes will be ignored: ${error}</summary>
    internal static string FinalCaptureFailed(
    object @error
    ) {
        return string.Format(Culture, ResourceFinder("FinalCaptureFailed") , @error);
    }

  /// <summary>Capturing from the instrument `${instrumentName}' failed.  Subsequent captures will be ignored: ${error}</summary>
    internal static string InstrumentCaptureFailed(
    object @instrumentName, object @error
    ) {
        return string.Format(Culture, ResourceFinder("InstrumentCaptureFailed") , @instrumentName, @error);
    }

  /// <summary>Log level value must be 0 - ${logLevelUpper}, inclusive</summary>
    internal static string LogLevelValueOutOfRange(
    object @logLevelUpper
    ) {
        return string.Format(Culture, ResourceFinder("LogLevelValueOutOfRange") , @logLevelUpper);
    }

  /// <summary>Parent logger reference could not be found for logger `${name}'.</summary>
    internal static string MissingParentLoggerReference(
    object @name
    ) {
        return string.Format(Culture, ResourceFinder("MissingParentLoggerReference") , @name);
    }

  /// <summary>Cannot specify a parent logger for the root logger.</summary>
    internal static string RootCannotHaveParent(
    
    ) {
        return string.Format(Culture, ResourceFinder("RootCannotHaveParent") );
    }

  /// <summary>Root logger initialization complete.</summary>
    internal static string RootLoggerDoneInitializing(
    
    ) {
        return string.Format(Culture, ResourceFinder("RootLoggerDoneInitializing") );
    }

  /// <summary>Root logger initializing -- ${loggerCount} loggers</summary>
    internal static string RootLoggerInitializing(
    object @loggerCount
    ) {
        return string.Format(Culture, ResourceFinder("RootLoggerInitializing") , @loggerCount);
    }

    }
}
