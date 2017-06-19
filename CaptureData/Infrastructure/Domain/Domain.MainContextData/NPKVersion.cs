using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
namespace Domain.MainContextData
{
    /// <summary>
    /// 补丁包
    /// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("DataBaseNPKVersion")]
    public class NPKVersion
    {
       [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 本次补丁有效作用版本号【非空】
        /// </summary>
        public string NPKEffectVersion { get; set; }
        /// <summary>
        /// 本次执行补丁后软件包的版本号
        /// </summary>
        public string GenerateNPKVersion { get; set; }
        /// <summary>
        /// 本次补丁内容提交记录号
        /// </summary>
        public string NPKSvn { get; set; }
        /// <summary>
        /// 本次补丁提交时间
        /// 1 表结构修改 2 业务数据变动 3 存储过程变动 4 函数变动
        /// </summary>
        public DateTime NPKSubmitTime { get; set; }
        /// <summary>
        ///补丁文件存放路径
        /// </summary>
        public string NPKPath { get; set; }
        /// <summary>
        /// 数据库结构发生改动的数据库命令语句
        /// </summary>
        public string DBStructCmd { get; set; }
        /// <summary>
        /// 补丁修改类型
        /// </summary>
        public int NPKType { get; set; }
        /// <summary>
        /// 补丁的备注
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 补丁数据存入数据库的时间【】
        /// </summary>
        public DateTime? InDBTime { get; set; }
        /// <summary>
        /// 补丁是否被删除【可能某一次补丁是内测版，不对外发布】
        /// </summary>
        public bool? IsDelete { get; set; }
        /// <summary>
        /// 补丁是否对外发布【对外发布的补丁不能进行删除操作】
        /// </summary>
        public bool? IsRelease { get; set; }
        /// <summary>
        /// 补丁许可使用人（默认提交补丁的操作人）
        /// </summary>
        public string NPKAuthor { get; set; }
        /// <summary>
        /// 补丁作用的数据库
        /// </summary>
        public string NPKDataBase { get; set; }
    }
    public enum ENPKCategory 
    {
        DBStruct=1,
        LogicData=2,
        SP=3,
        Fun=4
    }
    public enum ENPKErrorCategory
    { 
        [Description("没有提供备注")]
        NPKNoteIsEmpty=1,
        [Description("数据库变动但是没有提供变动脚本")]
        DBStructChangeNoCmd=2,
        [Description("数据库语句和脚本路径同时为空")]
        CmdOrNkpPathBothEmpty=3
    }
}
