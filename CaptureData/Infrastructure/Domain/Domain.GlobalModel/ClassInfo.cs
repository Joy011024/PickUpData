using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.GlobalModel
{
    public class ClassInfo
    {
        public string AssemblyName { get; set; }//程序集名称
        public string ClassName { get; set; }//类名称
        public string Display { get; set; }//显示名
        public string Remark { get; set; }//备注
        public string Version { get; set; }//版本信息
    }
}
