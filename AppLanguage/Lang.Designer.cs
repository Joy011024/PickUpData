﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppLanguage {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Lang {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Lang() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AppLanguage.Lang", typeof(Lang).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
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
        ///   查找类似 助记码 的本地化字符串。
        /// </summary>
        public static string LblCode {
            get {
                return ResourceManager.GetString("LblCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 说明 的本地化字符串。
        /// </summary>
        public static string LblDescription {
            get {
                return ResourceManager.GetString("LblDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 姓名 的本地化字符串。
        /// </summary>
        public static string LblFullName {
            get {
                return ResourceManager.GetString("LblFullName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 名称 的本地化字符串。
        /// </summary>
        public static string LblName {
            get {
                return ResourceManager.GetString("LblName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户名 的本地化字符串。
        /// </summary>
        public static string LblUserName {
            get {
                return ResourceManager.GetString("LblUserName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 出发地 的本地化字符串。
        /// </summary>
        public static string Tip_12306_GoStation {
            get {
                return ResourceManager.GetString("Tip_12306_GoStation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 查询车票时，必须提供的信息有：【{0}】,当前未输入项有:【{1}】 的本地化字符串。
        /// </summary>
        public static string Tip_12306_QueryTicketIsRequired {
            get {
                return ResourceManager.GetString("Tip_12306_QueryTicketIsRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 目的地 的本地化字符串。
        /// </summary>
        public static string Tip_12306_ToStation {
            get {
                return ResourceManager.GetString("Tip_12306_ToStation", resourceCulture);
            }
        }
    }
}
