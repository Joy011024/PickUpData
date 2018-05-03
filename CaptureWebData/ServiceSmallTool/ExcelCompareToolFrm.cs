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
            btnCompare.Tag = ButtonEvent.CompareExcelData.ToString();
            btnCompare.Click += new EventHandler(Button_Click);
            btnClear.Tag = ButtonEvent.ClearRecord;
            btnClear.Click += new EventHandler(Button_Click);
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

            leftExcelData.Add("OriginHeadName", "左侧列");
            leftExcelData.Add("OriginHeadIndex", "左侧列序号");
            lstCompare.BindHead(leftExcelData,true);
            rightExcelData.Add("NewHeadName", "右侧列");
            rightExcelData.Add("NewHeadIndex", "右侧列序号");
            lstCompare.BindHead(rightExcelData, true);
            lstCompare.FullRowSelect = true;
            lstCompare.SelectedIndexChanged += new EventHandler(CompareListView_Select);
            ReadExcelHead();
        }
        enum ECompareTarget 
        {
            Left=1,
            Right=2
        }
        enum ButtonEvent
        { 
            CompareExcelData=1,
            ClearRecord=2
        }
        List<bool> leftHead = new List<bool>();
        List<bool> rightHead = new List<bool>();
        Dictionary<string, string> leftExcelData = new Dictionary<string, string>();
        Dictionary<string, string> rightExcelData = new Dictionary<string, string>();
        void ReadExcelHead()
        {
            string exeDir = new AppDirHelper().GetAppDir(AppCategory.WinApp);
            string dir =exeDir+ @"\LogFile\ExcelCompare.xlsx";
            firstFile.SetPath(dir);
            ExcelDataHelper helper = new ExcelDataHelper();
            helper.QueryExcelHead(dir);
            lstLeft.BindDataSource<ExcelHeadAttribute>(helper.heads);
            string rightDir = exeDir+@"\LogFile\ExcelCompareCh.xlsx";
            secondFile.SetPath(rightDir);
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
                        foreach (var column in leftExcelData)
                        {
                           vc.SubItems[column.Key].Text=  compare.GetPropertyValue(column.Key);   
                        }
                        break;
                    case ECompareTarget.Right:
                        foreach (var column in rightExcelData)
                        {
                            vc.SubItems[column.Key].Text = compare.GetPropertyValue(column.Key);
                        }
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
        void Button_Click(object sender,EventArgs e) 
        {
            Button btn=sender as Button;
            if(btn==null)
            {
                return;
            }
            object tag = btn.Tag;
            string bt = tag == null ? string.Empty : tag.ToString();
            ButtonEvent be;
            if (!Enum.TryParse(bt, out be)) 
            {
                return;
            }
            switch (be)
            {
                case ButtonEvent.CompareExcelData:
                    DoComapreExcelEvent();
                    break;
                case ButtonEvent.ClearRecord:
                    DoClearRecordEvent();
                    break;
                default:
                    break;
            }
        }
        void DoComapreExcelEvent() 
        {
            ListView.ListViewItemCollection lvs = lstCompare.Items;
            //提取列表
            List<CompareData> heads = new List<CompareData>();
            if (lvs.Count == 0)
            {
                rtbNote.Text += "\r\n请选择比较的excel列";
            }
            foreach (ListViewItem item in lvs)
            {
                CompareData column = item.Tag as CompareData;
                heads.Add(column);
            }
            ExcelCompareActionHelp helper = new ExcelCompareActionHelp();
            string firstExcel=firstFile.SelectFileFullName;
            string secondExcel=secondFile.SelectFileFullName;
            helper.DoExcelCompare(firstExcel, secondExcel, heads);
        }
        void DoClearRecordEvent() 
        {
            rtbNote.Text = string.Empty;
        }
    }
    public class ExcelDataHelper
    {
       
        public List<ExcelHeadAttribute> heads;
        [Description("查询Excel的列头")]
        public void QueryExcelHead(string excelPath)
        {
            /*
             读取第一列
             */
            ExcelHelper.ReadSheet(excelPath, false, 0, ReadRowInSheet, true);
        }
        public void ReadRowInSheet(IRow row)
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
/*
 1.ListView 增加拖拽事件
 * 2.选择项事件触发时获取当前选择索引
 */