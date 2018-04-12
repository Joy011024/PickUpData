using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Domain.CommonData;
namespace TicketData.Model
{
    /// <summary>
    /// 该实体类库中需要调度System.Data.Linq
    /// </summary>
    [Table(Name = "sw")]
    public class CitySubway : GuidPrimaryKey
    {
        [Description("城市名")]
        [Column(Name = "cid")]//这是在进行xml中的数据匹配
        public  string Name { get; set; }
        [Column(Name = "n")]
        public short Code { get; set; }
        [Column(Name = "c")]
        public string City { get; set; }
    }
    [Table(Name = "l")]
    public class SubwayLine : GuidPrimaryKey
    {
        [Column(Name = "lid")]
        public  string Name { get; set; }
        [Column(Name = "lb")]
        public string LineName { get; set; }
        [Column(Name="i")]
        public short Code { get; set; }
        [Column(Name = "slb")] 
        public string LineDesc { get; set; } //如机场线，昌平线
        [Column(Name = "n")] //序号
        public int Number { get; set; }
        [Column(Name = "loop")]//该条地铁线是否环线【进行字符串之间的转换处理】
        public bool IsLoop { get; set; }
        [Column(Name = "lbx")]//最后一站的x坐标（最右边一站）
        public int StepX { get; set; }
        [Column(Name = "lby")]//最右侧一站的坐标
        public int StepY { get; set; }
        [Column(Name = "lbr")]//这个字段目前值都为 0.0
        public string lbr { get; set; }
        /// <summary>
        /// 地体线的颜色
        /// </summary>
        public string LineColor { get; set; }
        [Column(Name = "lnub")]
        public int LineId { get; set; }
    }
}
