using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TicketData.Model
{
    [Description("天猫商品数据信息采集product-iWrap 层下productImg-wrap")]
    public class TianmaoGood
    {
        public string productImg { get; set; }
        public string productPrice { get; set; }
        public string productTitle { get; set; }
        public string productShop { get; set; }
        public string productStatus { get; set; }

    }
}
