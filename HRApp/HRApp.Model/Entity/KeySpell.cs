using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonData;
namespace HRApp.Model
{
    public class KeySpell : GuidTimeField
    {
        public string SpellWord { get; set; }
        public int TableId { get; set; }
    }
    public class KeySpellGuid
    {
        public Guid TableRowId { get; set; }
    }
    public class KeySpellInt
    {
        public int TableRowId { get; set; }
    }
}
