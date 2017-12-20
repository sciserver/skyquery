﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Jhu.SkyQuery.Tap.Client {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ExceptionMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExceptionMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Jhu.SkyQuery.Tap.Client.ExceptionMessages", typeof(ExceptionMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The TAP command has been aborted..
        /// </summary>
        internal static string CommandCancelled {
            get {
                return ResourceManager.GetString("CommandCancelled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Operation is invalid when command is executing..
        /// </summary>
        internal static string CommandExecuting {
            get {
                return ResourceManager.GetString("CommandExecuting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Operation is invalid when command is not executing..
        /// </summary>
        internal static string CommandNotExecuting {
            get {
                return ResourceManager.GetString("CommandNotExecuting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The TAP command has timed out..
        /// </summary>
        internal static string CommandTimeout {
            get {
                return ResourceManager.GetString("CommandTimeout", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A TAP communication exception has occured..
        /// </summary>
        internal static string CommunicationException {
            get {
                return ResourceManager.GetString("CommunicationException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Connection is not in the open state..
        /// </summary>
        internal static string ConnectionNotOpen {
            get {
                return ResourceManager.GetString("ConnectionNotOpen", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid command type. Only CommandType.Text supported..
        /// </summary>
        internal static string InvalidTapCommandType {
            get {
                return ResourceManager.GetString("InvalidTapCommandType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parameter &apos;{0}&apos; not found..
        /// </summary>
        internal static string ParameterNotFound {
            get {
                return ResourceManager.GetString("ParameterNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The TAP service is not available: {0}..
        /// </summary>
        internal static string ServiceNotAvailable {
            get {
                return ResourceManager.GetString("ServiceNotAvailable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unexpected HTTP status code: {0} {1}..
        /// </summary>
        internal static string UnexpectedHttpResponse {
            get {
                return ResourceManager.GetString("UnexpectedHttpResponse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unexpected TAP job phase: {0}..
        /// </summary>
        internal static string UnexpectedPhase {
            get {
                return ResourceManager.GetString("UnexpectedPhase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unsupported query language: {0}..
        /// </summary>
        internal static string UnsupportedQueryLanguage {
            get {
                return ResourceManager.GetString("UnsupportedQueryLanguage", resourceCulture);
            }
        }
    }
}
