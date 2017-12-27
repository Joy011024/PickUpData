using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CommonHelperEntity
{
    /// <summary>
    /// 邮件分类
    /// </summary>
    public enum EEmail 
    {
        /// <summary>
        /// QQ邮箱
        /// </summary>
        [Description("QQ邮箱")]
        [AttachInfor("Host", "smtp.qq.com")]
        E_QQ=1,
        /// <summary>
        ///  微软
        /// </summary>
        E_Outlook=2,
        /// <summary>
        ///谷歌邮箱
        /// </summary>
        E_Gmail
    }
}
