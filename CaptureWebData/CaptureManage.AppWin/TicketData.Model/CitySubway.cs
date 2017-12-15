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
        public string LineDesc { get; set; } //如机场线，昌平线

    }
}
