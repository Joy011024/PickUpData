using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.CommonData
{
    [System.ComponentModel.DataAnnotations.Schema.Table("TecentQQData")]
    public class FindQQDataTable
    {
        public Guid ID { get; set; }
        public DateTime CreateTime { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int Distance { get; set; }
        public int Face { get; set; }
        public string Gender { get; set; }
        public string Nick { get; set; }
        public string Province { get; set; }
        public int Stat { get; set; }
        public string Uin { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.Column("HeadImageUrl")]
        public string Url { get; set; }
        /// <summary>
        /// 头像类型【默认0 当没有采集到头像数据时设置该值为-1】
        /// </summary>
        public int ImgType { get; set; }
        #region 增加这字段是为了兼容数据库表创建的形式不符合采集数据
        public int GatherImageErrorNum { get; set; }
        public int IsGatherImage { get; set; }
        [DescriptionSort("数据采集日期数值（精确到天），这个字段在数据量大的时候很有用")]
        public int DayInt { get; set; }
        #endregion
    }
    public class QQFriend 
    {
        public Guid ID { get; set; }
        public string QQFriendSort { get; set; }
        public string Nick { get; set; }
        /// <summary>
        /// 昵称全拼
        /// </summary>
        public string NickSpell { get; set; }
        /// <summary>
        /// 邮箱【邮箱前缀可能和qq账户uin不一致】
        /// </summary>
        public string Email { get; set; }
        public string UinHashCode { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        /// <summary>
        /// qq号
        /// </summary>
        public string Uin { get; set; }
        public string Remark { get; set; }
        #region  --分组中的3个参数项
        /// <summary>
        /// qq好友数所属分组
        /// </summary>
        public string QQBelongClass { get; set; }
        /// <summary>
        /// qq分组的序号【序号是各自的组中的序号】
        /// </summary>
        public string QQBelongClassID { get; set; }
        public string QQBelongClassParam { get; set; }
        #endregion

    }
}
