using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.ResponseData
{
    public class NpkResponseData
    {
        /// <summary>
        /// 提交人
        /// </summary>
        public string Submiter { get; set; }
        /// <summary>
        /// 是否修改数据库结构
        /// </summary>
        public bool IsDbStructChange { get; set; }
        /// <summary>
        /// 管理多个补丁命令作用一个软件包
        /// </summary>
        public string EffectVersion { get; set; }
    }
}
