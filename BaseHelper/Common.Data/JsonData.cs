using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public class JsonData
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public int StatueCode { get; set; }
        public object Data { get; set; }
        [Description("数据的json串")]
        public string DataJsonString { get; set; }
        [Description("额外附加数据")]
        public object AttachData { get; set; }
        public int Total { get; set; }
        public void Init() 
        {
            Result = false;
            Message = string.Empty;
            StatueCode = 0;
            Data=new object();
        }
    }
}
