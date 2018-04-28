using Domain.CommonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRApp.Model
{
    public class RelyTable : IntPrimaryKey
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string Note { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsDelete { get; set; }
    }
}
