using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.CommonData
{
   
    public class ItemData 
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
    public class NodeData : ItemData
    {
        public string ParentCode { get; set; }
        public int NodeLevel { get;set;}
    }
    /// <summary>
    /// 带有 ID的节点
    /// </summary>
    public class NodeItem :NodeData
    {
        public int Id { get; set; }
    }
    /// <summary>
    /// 分类
    /// </summary>
    public class CategoryData : NodeData
    {
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? ParentId { get; set; }
       
        /// <summary>
        /// 类型
        /// </summary>
        public string ItemType { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? CreateTime { get; set; }
        public int Sort { get; set; }
    }
}
