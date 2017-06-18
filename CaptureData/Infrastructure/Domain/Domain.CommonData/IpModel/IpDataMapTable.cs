using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.CommonData
{
    /// <summary>
    /// 映射表的IP数据
    /// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("IPAddress")]
    public class IpDataMapTable:CommonIp
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 数据录入时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 版本时间戳
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 粗略地址
        /// </summary>
        public string RoughlyAddress { get; set; }
        public void InitData() 
        {
            DateTime now = DateTime.Now;
            CreateTime = now;
            Id = Guid.NewGuid();
            Version = now.ToString("yyyyMMdd");
            RoughlyAddress = Continent + " " + Country + " " + Province + " " + City;
        }
    }
}
