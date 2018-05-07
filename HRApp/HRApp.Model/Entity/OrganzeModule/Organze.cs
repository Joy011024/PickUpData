using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonData;
namespace HRApp.Model
{
    public class Organze : FieldContainerCode
    {
        public int ParentId { get; set; }
        public int Scale { get; set; }//ushort <=63335
    }
    public class OrganzeMember:BaseFieldAsIntKey
    {
        public string SpellName { get; set; }
        public bool IsLeader { get; set; }
        public string ServeTime { get; set; }
        public string OrganzeId { get; set; }
        public short Statue { get; set; }
    }
}
