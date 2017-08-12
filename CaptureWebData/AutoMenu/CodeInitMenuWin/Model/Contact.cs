using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
namespace CodeInitMenuWin.Model
{
    /// <summary>
    /// 基础的联系人信息
    /// </summary>
    public class BaseContact
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; } 
    }
    /// <summary>
    /// 联系人关系描述
    /// </summary>
    public class ContactRelation : BaseContact
    {
        /// <summary>
        /// 关系描述
        /// </summary>
        public string Relation { get; set; }
        /// <summary>
        /// 称呼
        /// </summary>
        public string Call { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public string Birth { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 住址
        /// </summary>
        public string Local { get; set; }
    }
    public class ContactInfo : BaseContact
    {
        public string Uin { get; set; }
        public string UinNick { get; set; }

        public string Webchat { get; set; }
        public string WebchatNick { get; set; }
    }
    public class ContactData : ContactInfo
    {
        /// <summary>
        /// 标签
        /// </summary>
        public string Flag { get; set; }
    }
    /// <summary>
    /// 见面数据 
    /// </summary>
    public class FullContact : ContactData
    {
        public Guid ID { get; set; }
      
        public string PriceInfo { get; set; }
        public string ServiceInfo { get; set; }
        public string LocalInfo { get; set; }
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 见面时间
        /// </summary>
        public DateTime? MeetTime { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 本次见面评价
        /// </summary>
        public int? Score { get; set; }
        /// <summary>
        /// 留言描述
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 见面数据录入人员
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 是否系统录入数据
        /// </summary>
        public bool? IsSystemData { get; set; }
    }
}
