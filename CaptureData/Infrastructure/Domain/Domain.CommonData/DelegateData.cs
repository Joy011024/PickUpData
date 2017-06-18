using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.CommonData
{
    public class DelegateData
    {
        public delegate void BaseDelegate(object data);
        public BaseDelegate BaseDel { get; set; }
    }
}
