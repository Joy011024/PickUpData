using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.CommonData
{
    public  class Account:GuidPrimaryKey
    {
        [DescriptionSort("名称")]
        public string Name { get; set; }
        [DescriptionSort("头像,如果没有设置头HasHeadImage=0,否则HasHeadImage=1")]
        public string HeadImgUrl { get; set; }
        public bool HasHeadImage { get; set; }
        public string UserName { get; set; }
        [DescriptionSort("是否第三方账户")]
        public bool IsThirdAccount { get; set; }
        public short Statue { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreateDayInt { get; set; }
    }
}
