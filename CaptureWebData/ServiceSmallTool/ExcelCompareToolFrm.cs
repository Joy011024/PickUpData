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
            InitListViewHead();
        }
        void InitListViewHead()
        {
            Dictionary<string, string> heads = new Dictionary<string, string>();
            //显示点击行图标
            heads.Add("ColumnName", "列名");
            heads.Add("ColumnIndex", "序号");//这个需要转换为相应的字符如A，B
            lstLeft.BindHead(heads);
            lstLeft.Tag = ECompareTarget.Left;
            lstLeft.FullRowSelect = true;//设置点击整行触发
            lstLeft.SelectedIndexChanged += new EventHandler(ListViewRow_SelctClick);
            lstRight.BindHead(heads);
            lstRight.FullRowSelect = true;
            lstRight.Tag = ECompareTarget.Right;
            lstRight.SelectedIndexChanged += new EventHandler(ListViewRow_SelctClick);
            //如何增加拖拽事件
            //比较
            Dictionary<string, string> compareHead = new Dictionary<string, string>();
            compareHead.Add("OriginHeadName", "左侧列");
            compareHead.Add("OriginHeadIndex", "左侧列序号");
            compareHead.Add("NewHeadName", "右侧列");
            compareHead.Add("NewHeadIndex", "右侧列序号");
            lstCompare.BindHead(compareHead);
            lstCompare.FullRowSelect = true;
            lstCompare.SelectedIndexChanged += new EventHandler(CompareListView_Select);
            ReadExcelHead();
        }
        enum ECompareTarget 
        {
            Left=1,
            Right=2
        }
        List<bool> leftHead = new List<bool>();
        List<bool> rightHead = new List<bool>();
        void ReadExcelHead()
        {
            string dir = @"D:\LogFile\ExcelCompare.xlsx";
            ExcelCompareHelper helper = new ExcelCompareHelper();
            helper.QueryExcelHead(dir);
            lstLeft.BindDataSource<ExcelHeadAttribute>(helper.heads);
            string rightDir = @"D:\LogFile\ExcelCompareCh.xlsx";
            helper.QueryExcelHead(rightDir);
            lstRight.BindDataSource<ExcelHeadAttribute>(helper.heads);
            
        }
        void ListViewRow_SelctClick(object sender,EventArgs e) 
        {
            ListView lst = sender as ListView;
            //当前选择行的序号
            if (lst.SelectedItems.Count == 0)
            {//点击之后会两次触发，一次是选中，还有一次是释放选中
                return;
            }
            ListViewItem item = lst.FocusedItem;
            //当前选择项的行索引
            int index= item.Index;
            ECompareTarget area = (ECompareTarget)lst.Tag;
            rtbNote.Text +=string.Format( "Dirction:【{0}】 Row Index :【{1}】\r\n",area.ToString(),index);
            ExcelHeadAttribute head= item.Tag as ExcelHeadAttribute;
            CompareData compare = null;
            bool isInsert = false;
            int dataRowIndex = 0;
            if (leftHead.Count == rightHead.Count||
                (leftHead.Count>rightHead.Count&&area==ECompareTarget.Left)
                || (leftHead.Count < rightHead.Count&&area==ECompareTarget.Right))
            { //什么情况是进行添加操作呢？
                /*
                 1.左右两侧都已近补充完整
                 2.等待添加的数据来自现补充列较多的
                 */
                compare = new CompareData();
                isInsert = true;
            }
            else if (area == ECompareTarget.Left)
            {//此时进行的是单方补充
                //当前是数据补充到缺少项中
                dataRowIndex = leftHead.Count;
                compare = lstCompare.Items[dataRowIndex].Tag as CompareData;
            }
            else 
            {
                dataRowIndex = rightHead.Count;
                compare = lstCompare.Items[dataRowIndex].Tag as CompareData;
            }
            switch (area)
            {
                case ECompareTarget.Left:
                    compare.OriginHeadName = head.ColumnName;
                    compare.OriginHeadIndex = head.ColumnIndex;
                    leftHead.Add(true);
                    break;
                case ECompareTarget.Right:
                     compare.NewHeadName = head.ColumnName;
                    compare.NewHeadIndex = head.ColumnIndex;
                    rightHead.Add(true);
                    break;
            }
            //添加到中间区域时进行判断【组合成一项完整的匹配列】
            //【左右都存在内容才是一组完整数据】
            if (isInsert)
            {
                lstCompare.InsertRow(compare);
            }
            else 
            {//还需要对于UI进行改动
                ListViewItem vc= lstCompare.Items[dataRowIndex];
                //界面重绘
                switch (area)
                {
                    case ECompareTarget.Left:
                        vc.SubItems["OriginHeadName"].Text = compare.OriginHeadName;
                        vc.SubItems["OriginHeadIndex"].Text = compare.OriginHeadIndex.ToString();
                        break;
                    case ECompareTarget.Right:
                        vc.SubItems["NewHeadName"].Text = compare.NewHeadName;
                        vc.SubItems["NewHeadIndex"].Text = compare.NewHeadIndex.ToString();
                        break;
                }
            }
            lstCompare.Refresh();
            lst.Items.RemoveAt(index);
        }
        void CompareListView_Select(object obj ,EventArgs e) 
        {//移除选择项，添加到左右两侧
            ListView lst = obj as ListView;
            if (lst.SelectedItems.Count == 0)
            {
                return;
            }
            ListViewItem focus= lst.FocusedItem;
            int index = focus.Index;
            CompareData merge = focus.Tag as CompareData;
            if (leftHead.Count > index)
            {  //左侧
                ExcelHeadAttribute left = new ExcelHeadAttribute() { ColumnIndex = merge.OriginHeadIndex, ColumnName = merge.OriginHeadName };
                lstLeft.InsertRow(left);
                leftHead.RemoveAt(index);
            }
            if (rightHead.Count > index)
            {//右侧
                ExcelHeadAttribute right = new ExcelHeadAttribute() { ColumnIndex = merge.NewHeadIndex, ColumnName = merge.NewHeadName };
                lstRight.InsertRow(right);
                rightHead.RemoveAt(index);
            }
            //合并项
            lst.Items.RemoveAt(index);
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
    public class CompareData 
    {
        [Description("原始列")]
        public string OriginHeadName { get; set; }
        [Description("原始列序号")]
        public int OriginHeadIndex { get; set; }
        [Description("新列")]
        public string NewHeadName { get; set; }
        [Description("新列序号")]
        public int NewHeadIndex { get; set; }
    }
}
/*
 1.ListView 增加拖拽事件
 * 2.选择项事件触发时获取当前选择索引
 */