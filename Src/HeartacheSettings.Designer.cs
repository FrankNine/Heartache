﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Heartache {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class HeartacheSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static HeartacheSettings defaultInstance = ((HeartacheSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new HeartacheSettings())));
        
        public static HeartacheSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string DataWinPath {
            get {
                return ((string)(this["DataWinPath"]));
            }
            set {
                this["DataWinPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string DisassembledDataPath {
            get {
                return ((string)(this["DisassembledDataPath"]));
            }
            set {
                this["DisassembledDataPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string TranslatedDataWinPath {
            get {
                return ((string)(this["TranslatedDataWinPath"]));
            }
            set {
                this["TranslatedDataWinPath"] = value;
            }
        }
    }
}
