using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRApp.Web
{
    public class BaseRequestParam
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class NodeRequestParam : BaseRequestParam
    {
        public string ParentCode { get; set; }
    }
    public class QueryRequestParam:BaseRequestParam
    {
        public int BeginRow { get; set; }
        public int EndRow { get; set; }
    }
}