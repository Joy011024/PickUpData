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
        [Key] //新增为了兼容sqlite
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
     
    public class CategoryGroup 
    {
        public CategoryData Root { get; set; }
        public List<CategoryGroup> Childrens = new List<CategoryGroup>();
    }
    public class CategoryGourpHelper 
    {
        public CategoryGroup DataGroup(List<CategoryData> datas)
        {
            CategoryGroup parents = new CategoryGroup();
            List<CategoryData> roots = datas.Where(s => !s.ParentId.HasValue).ToList();
            List<int> ids = roots.Select(s => s.Id).ToList();
            foreach (CategoryData item in roots)
            {
                CategoryGroup root = new CategoryGroup()
                {
                    Root = item
                };
                List<CategoryData> childrens = datas.Where(d => !ids.Contains(d.Id)).ToList();
                CategoryGroup group = ForeachData(childrens, item.Id);
                root.Childrens.AddRange(group.Childrens.ToArray());
                parents.Childrens.Add(root);
            }
            return parents;
        }
        public CategoryGroup ForeachData(List<CategoryData> items, int parentId)
        {
            CategoryGroup node = new CategoryGroup();
            List<CategoryData> childrens= items.Where(s => s.ParentId == parentId).ToList();
            if (childrens.Count == 0)
            { //没有相关子节点
                return node;
            }
            List<int> pids = childrens.Select(s => s.Id).ToList();
            List<CategoryData> chs= items.Where(p => !pids.Contains(p.Id)).ToList();//剔除当前节点的子节点数据
           
            foreach (CategoryData item in childrens)
            {
                List<CategoryData> cds = chs.Where(c => c.ParentId == item.Id).ToList();
                CategoryGroup group = new CategoryGroup() { Root = item };
                if (cds.Count == 0)
                {
                    node.Childrens.Add(group);
                    continue;
                }
               
                List<int> cdsId = cds.Select(s => s.Id).ToList();
                List<CategoryData> levels = items.Where(s => !cdsId.Contains(s.Id)).ToList();
                foreach (CategoryData cs in cds)
                {
                    CategoryGroup levelnode = new CategoryGroup() { Root=cs};
                    CategoryGroup childern = ForeachData(levels, cs.Id);
                   levelnode.Childrens.AddRange(childern.Childrens.ToArray());
                   group.Childrens.Add(levelnode);
                }
                node.Childrens.Add(group);
            }
            return node;
        }
    }
}
