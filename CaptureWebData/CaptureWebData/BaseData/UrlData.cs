using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaptureWebData
{
    public class UrlData
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string UrlKey { get; set; }
        public string UrlDesc { get; set; }
        public string RequestMethod { get; set; }
        public string ParamList { get; set; }
        public string Cookie { get; set; }
        public string WebName { get; set; }
        public string WebKey { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
