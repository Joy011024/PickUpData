using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.CommonData;
namespace TicketData.Model
{
    public class SubwaySiteData : ModelFieldContainTimeWithGuid
    {
        public int BelongLine { get; set; }
        public int TurnLine { get; set; }
        public string SubwaySiteName { get; set; }
        public short SubwaySiteTimeLong { get; set; }//时长 short  最大值  32767
    }
}
