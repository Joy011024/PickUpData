using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.CommonData
{
    /// <summary>
    /// 数据采集统计
    /// </summary>
    public class PickUpStatic
    {
        /// <summary>
        /// 统计日期
        /// </summary>
        public string StaticDay { get; set; }
        /// <summary>
        /// 有效数目
        /// </summary>
        public int IdTotal { get; set; }
        /// <summary>
        /// 总数目
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// 表中全部数量
        /// </summary>
        public int DBTotal { get; set; }
        /// <summary>
        /// 表中有效数量
        /// </summary>
        public int DBPrimaryTotal { get; set; }
    }
    public class WaitGatherImage 
    {
        public Guid Id { get; set; }
        public string Uin { get; set; }
        public string HeadImageUrl { get; set; }
        public string LocalCity { get; set; }
    }
}
