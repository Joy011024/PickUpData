using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infrastructure.ExtService;
using CommonHelperEntity;
namespace ServiceSmallTool
{
    public partial class ExcelCompareToolFrm : Form
    {
        public ExcelCompareToolFrm()
        {
            InitializeComponent();
        }
    }
    public class ExcelCompareHelper
    {
        enum ExcelDataSource 
        {
            [Description("第一份Excel（Left）")]
            OriginExcel=1,
            [Description("第二份Excel（Right）")]
            TargetExcel=2
        }
        [Description("查询Excel的列头")]
        public void QueryExcelHead(string excelPath) 
        {
            /*
             读取第一列
             */
        }
    }
}
