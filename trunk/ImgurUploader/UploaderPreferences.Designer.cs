﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ImgurUploader {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class UploaderPreferences : global::System.Configuration.ApplicationSettingsBase {
        
        private static UploaderPreferences defaultInstance = ((UploaderPreferences)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new UploaderPreferences())));
        
        public static UploaderPreferences Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string DefaultExport {
            get {
                return ((string)(this["DefaultExport"]));
            }
            set {
                this["DefaultExport"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool IncludeLinks {
            get {
                return ((bool)(this["IncludeLinks"]));
            }
            set {
                this["IncludeLinks"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string ImgurAccountName {
            get {
                return ((string)(this["ImgurAccountName"]));
            }
            set {
                this["ImgurAccountName"] = value;
            }
        }
    }
}
