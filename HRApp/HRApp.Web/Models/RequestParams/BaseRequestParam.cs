﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace HRApp.Web
{
    public class BaseRequestParam
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
    }
    public class NodeRequestParam : SampleRequestParam
    {
        public string ParentCode { get; set; }
        public int ParentId { get; set; }
       
    }
    public class QueryRequestParam:BaseRequestParam
    {
        public int BeginRow { get; set; }
        public int EndRow { get; set; }
        //[Description("第一排序字段")]
        public string OrderField { get; set; }
        //[Description("并联排序自断后，第一排序字段非空才有效")]
        public List<string> ThenOrderFields { get; set; }
        //[Description("排序方向")]
        public bool OrderAsc { get; set; }//升序
        public string LimitCode { get; set; }
        public List<string> LimitIds { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
    public class SampleRequestParam : BaseRequestParam
    {
        public string Value { get; set; }
    }
    public class QueryContactParam : QueryRequestParam
    {
        public int ReportEnum { get; set; }
    }
    public class EnumRequestParam : SampleRequestParam
    {
        [Description("枚举成员")]
        public string EnumMember { get; set; }
    }
    public class FilterParam
    {
        [Description("忽略的项")]
        public List<string> Ignores { get; set; }
        [Description("是否包含被删除的")]
        public bool ContainerDelete { get; set; }
    }
}