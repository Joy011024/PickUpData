using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.CommonData
{
    public abstract class BaseField
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
    public class ModelFieldContainTimeWithGuid : ModelFieldContainTime
    {
        public Guid Id { get; set; }
    }
    public class ModelFieldContainTime
    {
        public int InDBDayInt { get; set; }//数据入库时间

        public DateTime CreateTime { get; set; }
          
    }
    public class FieldContainTime:BaseField
    { 
        
    }
}
