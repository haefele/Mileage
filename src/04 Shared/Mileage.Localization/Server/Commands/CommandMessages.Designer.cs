﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mileage.Localization.Server.Commands {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class CommandMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CommandMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Mileage.Localization.Server.Commands.CommandMessages", typeof(CommandMessages).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The admin user was already created..
        /// </summary>
        public static string AdminUserAlreadyCreated {
            get {
                return ResourceManager.GetString("AdminUserAlreadyCreated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your session has expired..
        /// </summary>
        public static string AuthenticationTokenExpired {
            get {
                return ResourceManager.GetString("AuthenticationTokenExpired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The email address is already in use..
        /// </summary>
        public static string EmailIsNotAvailable {
            get {
                return ResourceManager.GetString("EmailIsNotAvailable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The requested layout was not found..
        /// </summary>
        public static string LayoutNotFound {
            get {
                return ResourceManager.GetString("LayoutNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No session informations was given..
        /// </summary>
        public static string NoAuthenticationTokenGiven {
            get {
                return ResourceManager.GetString("NoAuthenticationTokenGiven", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The password is incorrect..
        /// </summary>
        public static string PasswordIncorrect {
            get {
                return ResourceManager.GetString("PasswordIncorrect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This user is deactivated..
        /// </summary>
        public static string UserIsDeactivated {
            get {
                return ResourceManager.GetString("UserIsDeactivated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No user was found with your username..
        /// </summary>
        public static string UserNotFound {
            get {
                return ResourceManager.GetString("UserNotFound", resourceCulture);
            }
        }
    }
}
