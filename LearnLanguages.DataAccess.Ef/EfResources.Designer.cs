﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.488
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LearnLanguages.DataAccess.Ef {
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
    public class EfResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal EfResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LearnLanguages.DataAccess.Ef.EfResources", typeof(EfResources).Assembly);
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
        ///   Looks up a localized string similar to The database name was not found ({0})..
        /// </summary>
        public static string DatabaseNameNotFound {
            get {
                return ResourceManager.GetString("DatabaseNameNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to English.
        /// </summary>
        public static string DefaultLanguageText {
            get {
                return ResourceManager.GetString("DefaultLanguageText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to False.
        /// </summary>
        public static string DeleteAllExistingDataAndStartNewSeedData {
            get {
                return ResourceManager.GetString("DeleteAllExistingDataAndStartNewSeedData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to One of the properties was not assigned in the DbContextManager..
        /// </summary>
        public static string ErrorMsgPropertyNotAssigned {
            get {
                return ResourceManager.GetString("ErrorMsgPropertyNotAssigned", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to LearnLanguagesContext.
        /// </summary>
        public static string LearnLanguagesConnectionStringKey {
            get {
                return ResourceManager.GetString("LearnLanguagesConnectionStringKey", resourceCulture);
            }
        }
    }
}
