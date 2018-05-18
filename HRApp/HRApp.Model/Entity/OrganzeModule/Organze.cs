using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonData;
using System.Data.Linq.Mapping;
using Domain.GlobalModel;
namespace HRApp.Model
{
    [TableField(DbGeneratedFields = new string[] { "Id" }, TableName = "Organze")]
    
    public class Organze : FieldContainerCode
    {
        public int ParentId { get; set; }
        [DescriptionSort("组织规模")]
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
