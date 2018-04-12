using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Data
{
    public enum SelfCategory
    {
        [Description("数据库")]
        DataAttribute=1,
        [Description("表")]
        TableAttribute=2,
        [Description("列")]
        ColumnAttribute=3,
        [Description("不是数据库结构")]
        NoSqlTargetAttribute=-1
    }
    public class SelfAttribute:Attribute
    {
        /// <summary>
        /// 改特性修饰的对象父节点【如 对象为属性，则父节点对应于类】
        /// </summary>
        public string FatherNode { get; set; }
        /// <summary>
        /// 属性的数据类型
        /// </summary>
        public string TargetDataType { get; set; }
        /// <summary>
        /// 在数据库中对应的数据类型
        /// </summary>
        public string SqlDBType { get; set; }
        /// <summary>
        /// 是否为数据库的列
        /// </summary>
        public bool? IsSqlColumn { get; set; }
         
    }
}
