using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.CommonData
{
    public class DelegateData
    {
        /// <summary>
        /// 最简单的委托【不传递任何参数】
        /// </summary>
        public delegate void SampleDelegate();
        public delegate void BaseDelegate(object data);
        public BaseDelegate BaseDel { get; set; }
    }
}
