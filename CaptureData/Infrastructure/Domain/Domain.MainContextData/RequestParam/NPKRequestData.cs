using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.RequestParam
{
    public class NPKRequestData
    {
        /// <summary>
        /// 提交记录号
        /// </summary>
        public string Svn { get; set; }
        /// <summary>
        /// 影响的版本号
        /// </summary>
        public string EffectVersion { get; set; }
        /// <summary>
        /// 提交补丁内容
        /// </summary>
        public string NpkCmd { get; set; }
        /// <summary>
        /// 补丁备注
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 补丁创建日期【精确到天】
        /// </summary>
        public string  NpkCreateTime { get; set; }
        /// <summary>
        /// 补丁创建人
        /// </summary>
        public string NpkAuthor { get; set; }
        /// <summary>
        /// 补丁提交人
        /// </summary>
        public string NpkSubmiter { get; set; }
        /// <summary>
        /// 补丁作用的数据库
        /// </summary>
        public string NPKEffectDataBase { get; set; }
    }
}
