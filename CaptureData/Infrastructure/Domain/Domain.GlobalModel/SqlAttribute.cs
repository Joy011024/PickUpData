using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.ComponentModel;
namespace Domain.GlobalModel
{
    /// <summary>
    /// 基于System.Data.Linq.Mapping 扩展特性
    /// </summary>
    public class TableFieldAttribute :Attribute
    {
        [Description("数据库生成字段列表")]
        public string[] DbGeneratedFields;
        public string TableName { get; set; }
        [Description("忽略字段")]
        public string[] IgnoreProperty { get; set; }
    }
    [Description("属性忽略匹配字段")]
    public class PropertyIgnoreFieldAttribute : Attribute
    { 
        
    }
}
