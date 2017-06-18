using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.CommonData
{
    public class CommonIp:AddressData
    {
        /// <summary>
        /// IP
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 经度【东西方向】
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 纬度【南北方向】
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// 运营商
        /// </summary>
        public string Operator { get; set; }
    }
}
