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
    public class ValidationMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ValidationMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Common.Web.Mvc.Resources.ValidationMessages", typeof(ValidationMessages).Assembly);
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
        ///   Looks up a localized string similar to Разрешаются файлы только в следующих форматах: {1}..
        /// </summary>
        public static string AllowedFileExtensions {
            get {
                return ResourceManager.GetString("AllowedFileExtensions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Текст с картинки введён не верно..
        /// </summary>
        public static string CaptchaIncorrect {
            get {
                return ResourceManager.GetString("CaptchaIncorrect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Необходимо ввести текст с картинки..
        /// </summary>
        public static string CaptchaRequired {
            get {
                return ResourceManager.GetString("CaptchaRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to E-mail адрес введен некорректно..
        /// </summary>
        public static string EmailIncorrect {
            get {
                return ResourceManager.GetString("EmailIncorrect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Необходимо принять условия обработки персональных данных..
        /// </summary>
        public static string PersonalDataProcessingRequired {
            get {
                return ResourceManager.GetString("PersonalDataProcessingRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Номер телефона введен некорректно..
        /// </summary>
        public static string PhoneIncorrect {
            get {
                return ResourceManager.GetString("PhoneIncorrect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Необходимо заполнить поле {0}..
        /// </summary>
        public static string PropertyValueRequired {
            get {
                return ResourceManager.GetString("PropertyValueRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Значение поля должно быть от {1} до {2}..
        /// </summary>
        public static string RangeIncorrect {
            get {
                return ResourceManager.GetString("RangeIncorrect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Поле может содержать только латинские буквы, цифры и символы дефиса и нижнего подчеркивания..
        /// </summary>
        public static string SafeNameIncorrect {
            get {
                return ResourceManager.GetString("SafeNameIncorrect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Длина поля не должна привышать {1} символов..
        /// </summary>
        public static string StringLengthIncorrect {
            get {
                return ResourceManager.GetString("StringLengthIncorrect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Максимальный размер загружаемых файлов: {1} мб..
        /// </summary>
        public static string UploadedFilesSizeIncorrect {
            get {
                return ResourceManager.GetString("UploadedFilesSizeIncorrect", resourceCulture);
            }
        }
    }
}
