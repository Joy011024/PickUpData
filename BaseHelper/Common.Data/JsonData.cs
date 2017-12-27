using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public class JsonData
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public int? StatueCode { get; set; }
        public object Data { get; set; }
        public int Total { get; set; }
        public void Init() 
        {
            Result = false;
            Message = string.Empty;
            StatueCode=new int?();
            Data=new object();
        }
    }
}
