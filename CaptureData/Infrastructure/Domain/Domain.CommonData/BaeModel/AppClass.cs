using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Domain.CommonData
{
    [Description("int 主键的公共类")]
    public class BaseIntPrimary 
    {
        public int Id { get; set; }

    }
    public class BaseLogicWithIntPRimary:BaseIntPrimary
    {
        public int IsDelete { get; set; }
    }
    public class App : BaseLogicWithIntPRimary
    {
        public string AppName { get; set; }
        public string AppCode { get; set; }
    }
    public class AppVer:BaseLogicWithIntPRimary
    {
        public string AppVersion { get; set; }
        public string AppId { get; set; }
        public bool IsNowVersion { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
