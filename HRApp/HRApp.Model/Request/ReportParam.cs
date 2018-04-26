using System;
using System.Collections.Generic;
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
}
