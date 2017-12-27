using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public class BaseModel
    {
        public Guid Gid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> SpecialData { get; set; }
    }
}
