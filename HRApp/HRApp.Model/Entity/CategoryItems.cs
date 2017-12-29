using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonData;
using System.ComponentModel;
namespace HRApp.Model
{
    public class CategoryItems:CategoryData
    {
        [Description("配置项被引用次数")]
        public int ItemUsingSize { get; set; }
        [Description("内容项的描述")]
        public string ItemDesc { get; set; }
    }
}
