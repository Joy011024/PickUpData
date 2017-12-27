using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public class DataItem
    {
        public object Value { get; set; }
        public string Name { get; set; }
        public DataType ValueType { get; set; }
        public object Tag { get; set; }
    }
}
