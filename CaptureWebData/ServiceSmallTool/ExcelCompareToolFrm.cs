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
using NPOI.SS;
using NPOI.SS.UserModel;
using FeatureFrmList;
namespace ServiceSmallTool
{
    public partial class ExcelCompareToolFrm : Form
    {
        public ExcelCompareToolFrm()
        {
            InitializeComponent();
            ReadExcelHead();
        }
        void ReadExcelHead()
        {
            string dir = @"D:\LogFile\ExcelCompare.xlsx";
            ExcelCompareHelper helper = new ExcelCompareHelper();
            helper.QueryExcelHead(dir);
            Dictionary<string, string> heads = new Dictionary<string, string>();
            heads.Add("ColumnName", "列名");
            heads.Add("ColumnIndex", "序号");
            lstLeft.BindHead(heads);
            lstLeft.BindDataSource<ExcelHeadAttribute>(helper.heads);
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
        public List<ExcelHeadAttribute> heads;
        [Description("查询Excel的列头")]
        public void QueryExcelHead(string excelPath) 
        {
            /*
             读取第一列
             */
            ExcelHelper.ReadSheet(excelPath, false, 0, ReadRowInSheet, true);
        }
        public   void ReadRowInSheet(IRow row)
        {
            heads = new List<ExcelHeadAttribute>();
            short cellIndex = row.LastCellNum;//总共多少列
            for (short i = 0; i < cellIndex; i++)
            {
                ICell cell = row.GetCell(i);
                if (cell == null)
                { //对于空列的处理
                    continue;
                }
                IRichTextString rtext = cell.RichStringCellValue;
                string text = rtext.String;
                //文本内容，列索引 
                if (!string.IsNullOrEmpty(text))
                {
                    ExcelHeadAttribute head = new ExcelHeadAttribute()
                    {
                        ColumnIndex = i,
                        ColumnName = text.Trim()
                    };
                    heads.Add(head);
                }
            }
        }
    }
}
