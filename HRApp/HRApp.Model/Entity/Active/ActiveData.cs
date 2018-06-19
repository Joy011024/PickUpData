using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonData;
using Domain.GlobalModel;
namespace HRApp.Model
{
    [TableField(TableName = "ActiveData")]
    public class ActiveData:GuidBaseFieldContainTime
    {
        public DateTime ActiveBeginTime { get; set; }
        public DateTime ActiveEndTime { get; set; }
        public string ActiveAddress { get; set; }
        [DescriptionSort("地址详情")]
        public string AdressDetail { get; set; }
        [DescriptionSort("活动组织者")]
        public string ActiveOrganzer { get; set; }
        [DescriptionSort("活动状态")]
        public short ActiveStatue { get; set; }
        [DescriptionSort("活动详情")]
        public string ActiveDetail { get; set; }
        [DescriptionSort("活动举办状态")]
        public short ActiveExecuteStatue { get; set; }
    }
}
