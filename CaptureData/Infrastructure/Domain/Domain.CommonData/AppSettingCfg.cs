using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.CommonData
{
    /// <summary>
    /// 系统配置项
    /// </summary>
    public class AppSettingCfg : ModelFieldContainTimeWithGuid
    {
        public Guid Id { get; set; }
        public string CfgName { get; set; }
        public string CfgCode { get; set; }
        public string CfgValue { get; set; }
        public Guid ParentId { get; set; }
        public string Note { get; set; }
        public short IsDelete { get; set; }
    }
}
