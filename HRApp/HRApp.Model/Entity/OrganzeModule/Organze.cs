using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonData;
namespace HRApp.Model
{
    public class Organze:BaseFileAsIntKey
    {
        public int ParentId { get; set; }
        public string Code { get; set; }
        public int Scale { get; set; }//ushort <=63335
    }
}
