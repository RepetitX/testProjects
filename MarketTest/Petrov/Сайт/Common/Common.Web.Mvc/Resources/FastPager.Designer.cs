﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Common.Web.Mvc.Resources {
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
    public class FastPager {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal FastPager() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Common.Web.Mvc.Resources.FastPager", typeof(FastPager).Assembly);
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
        ///   Looks up a localized string similar to first.
        /// </summary>
        public static string PaginationFirst {
            get {
                return ResourceManager.GetString("PaginationFirst", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Showing {0} - {1}.
        /// </summary>
        public static string PaginationFormat {
            get {
                return ResourceManager.GetString("PaginationFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to last.
        /// </summary>
        public static string PaginationLast {
            get {
                return ResourceManager.GetString("PaginationLast", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to next.
        /// </summary>
        public static string PaginationNext {
            get {
                return ResourceManager.GetString("PaginationNext", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to prev.
        /// </summary>
        public static string PaginationPrev {
            get {
                return ResourceManager.GetString("PaginationPrev", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Showing {0}.
        /// </summary>
        public static string PaginationSingleFormat {
            get {
                return ResourceManager.GetString("PaginationSingleFormat", resourceCulture);
            }
        }
    }
}
