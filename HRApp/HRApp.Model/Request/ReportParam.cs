using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRApp.Model
{
    public class ReportParam
    {
        public string Table { get; set; }
        public List<Guid> Ids { get; set; }
        public int ReportType { get; set; }
        public string Note { get; set; }
    }
    public class RequestParam 
    {
        [Description("查询关键字")]
        public string QueryKey { get; set; }
        [Description("开始时间")]
        public string BeginTime { get; set; }
        [Description("结束时间")]
        public string EndTime { get; set; }
        [Description("行开始索引")]
        public int RowBeginIndex { get; set; }
        [Description("行结束索引")]
        public int RowEndIndex { get; set; }
    }
}
