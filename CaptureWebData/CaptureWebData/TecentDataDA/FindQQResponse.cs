using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaptureWebData
{
    public class FindQQ
    {
        public int age { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public int distance { get; set; }
        public int face { get; set; }
        public string gender { get; set; }
        public string nick { get; set; }
        public string province { get; set; }
        public int stat { get; set; }
        public string uin { get; set; }
        public string url { get; set; }
    }
    public class FindQQData 
    {
        public List<FindQQ> info_list { get; set; }
        public int count { get; set; }
        public int endflag { get; set; }
        public int online { get; set; }
        public int page { get; set; }
        public int sessionid { get; set; }
        public int totalnum { get; set; }

    }
    public class FindQQResult 
    {
        public FindQQData buddy { get; set; }
        public int sret { get; set; }
    }
    public class FindQQResponse 
    {
        public FindQQResult result { get; set; }
        public int retcode { get; set; }
    }
}
