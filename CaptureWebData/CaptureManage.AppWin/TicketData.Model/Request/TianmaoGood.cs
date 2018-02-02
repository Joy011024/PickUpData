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
        [Description("商品链接【productImg 的href属性补充http:】")]
        public string GoodHref { get; set; }
        [Description("商品图片 IMG属性src")]
        public string productImg { get; set; }
        public string productPrice { get; set; }
        [Description("单价")]
        public float GoodPrice { get; set; }
        char priceUnit;
        [Description("币种")]
        public char PriceUnit
        {
            get
            {
                if (!string.IsNullOrEmpty(priceUnit.ToString()))
                {
                    priceUnit = '$';
                }
                return priceUnit;
            }
            set { priceUnit = value; }
        }
        public string productTitle { get; set; }
        public string productShop { get; set; }
        public string productStatus { get; set; }

    }
}
