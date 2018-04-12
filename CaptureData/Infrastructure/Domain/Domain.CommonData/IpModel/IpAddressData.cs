using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.CommonData
{
    /// <summary>
    /// 地址数据
    /// </summary>
    public  class AddressData
    {
        /// <summary>
        /// IP地址所在的大洲（如亚洲，南美洲）
        /// </summary>
        public string Continent { get; set; }
        /// <summary>
        /// 所属国家地区
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区县
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// 区域编号（类似邮政编码）
        /// </summary>
        public string AreaCode { get; set; }
        /// <summary>
        /// 英文名称
        /// </summary>
        public string En { get; set; }
        /// <summary>
        /// 简称【设置长度最大为20】
        /// </summary>
        public string CC { get; set; }
    }
}
