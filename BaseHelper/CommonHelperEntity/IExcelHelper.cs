﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;
using System.Data.OleDb;
using System.ComponentModel;
using DataHelp;
namespace CommonHelperEntity
{
    public class ExcelDataHelper : IExcelHelper { }
    public interface IExcelHelper
    {
    }
    public enum EExcelType
    { 
        Xls=1,
        Xlsx=2
    }
    [Description("Excel列头信息设置")]
    public class ExcelHeadAttribute:Attribute
    {
        [Description("列在表格中的位置号，未使用")]
        public int ColumnIndex { get; set; }
        [Description("列名")]
        public string ColumnName { get; set; }
        [Description("该列开始的索引")]
        public int BeginCellIndex { get; set; }
        [Description("占用列数")]
        public int OccuopationCell { get; set; }
        [Description("占用行数")]
        public int OccupationRow { get; set; }
        [Description("列所在的行索引")]
        public int RowPosition { get; set; }
        [Description("列宽（设置100以上才能查看到显示的效果）")]
        public int ColumnWidth { get; set; }
        [Description("只作为列名应用【特殊列合并】")]
        public bool JustColumnNoRowText { get; set; }
        [Description("纵向进行单元格自动合并")]
        public bool MergeCellInY { get; set; }
    }
     public delegate void SheetHeadDataToDo(NPOI.SS.UserModel.ISheet sheet);
     public delegate void SheetRowDataToDo<T>(NPOI.SS.UserModel.ISheet sheet, List<T> rows) where T : class;
    public static class ExcelHelper 
    {
        public static DataTable ReadExcelSingleSheet<T>(this T helper, FileStream stream) where T : IExcelHelper 
        {//读取单页excel数据
            FileStream fs = stream; 
            try
            {
                HSSFWorkbook book = new HSSFWorkbook(fs);
                if (book.NumberOfSheets == 0)
                {//是否写入了excel
                    return new DataTable();
                }
                ISheet sheet = book.GetSheetAt(0);
                return ReadSheet(sheet);
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                return new DataTable();
            }
            finally
            {
                fs.Close();
            }
        }
        /// <summary>
        /// 只读取指定的工作页
        /// </summary>
        /// <param name="excelDir"></param>
        /// <param name="version"></param>
        /// <param name="sheetIndex"></param>
        /// <returns></returns>
        public static ISheet ReadExcelSheet(string excelDir,EExcelType version, int sheetIndex)
        {
            IWorkbook booke = GetExcelWorkBook(excelDir, version);
            ISheet sheet= booke.GetSheetAt(sheetIndex);
            booke.Close();
            return sheet;
        }
        public static List<DataTable> ReadExcelSheets<T>(this T helper,FileStream stream) where T:IExcelHelper
        {//读取excel的所有工作页
            List<DataTable> tables = new List<DataTable>();
            try
            {
                HSSFWorkbook book = new HSSFWorkbook(stream);
                for (int i = 0; i < book.NumberOfSheets; i++)
                {//存在多少工作页
                    tables.Add(ReadSheet(book.GetSheetAt(i)));
                }
                return tables;
            }
            catch (Exception ex)
            {
                return tables;
            }
            finally 
            {
                stream.Close();
            }
        }
        public static void ReadExcel(string excelPath) 
        {
            //检查文件是否存在【b/s结构进行操作需要将文件上传到服务器】
            if (!File.Exists(excelPath)) 
            {//文件不存在
                return;
            }
            /*Microsoft Jet 数据库引擎打不开文件''。  
             
             它已经被别的用户以独占方式打开，或没有查看数据的权限。*/
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;";
            strConn = string.Format(strConn, excelPath);
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable sheetNames = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
            conn.Close();
            foreach (DataRow row in sheetNames.Rows)
            {//读取数据行
                
            }
            OleDbDataAdapter adapter = new OleDbDataAdapter();

        }
        private static DataTable ReadSheet(ISheet sheet)
        {//读取工作页数据
            int rowIndex = sheet.LastRowNum;//总共存在多少行数据
            int rowFirst = 0;
            int columnFirst = 0;
            int current = sheet.FirstRowNum;
            int columnIndex = 0;
            DataTable table = new DataTable();
            table.TableName = sheet.SheetName;
            //如果行数为0则不需要进行数据读取
            if (rowIndex > 0)
            {
                rowFirst = sheet.FirstRowNum;//首行开始位置
                IRow first = sheet.GetRow(rowFirst);
                columnIndex = first.LastCellNum;
                columnFirst = first.FirstCellNum;
                DataRow row = table.NewRow();
                for (int c = columnFirst; c < columnIndex; c++)
                {
                    ICell cell = first.GetCell(c);
                    //实际上首行有空列数据是异常的
                    string value = cell == null ? string.Empty : cell.ToString();
                    table.Columns.Add(value);
                    row[c] = value;
                }
            }

            for (int r = rowFirst + 1; r <= rowIndex; r++)
            {
                DataRow row = table.NewRow();
                IRow rData = sheet.GetRow(r);

                for (int c = columnFirst; c < columnIndex; c++)
                {
                    ICell cell = rData.GetCell(c);
                    string value = cell == null ? string.Empty : cell.ToString();
                    DataColumn column = new DataColumn(value);
                    row[c] = column;
                }
                table.Rows.Add(row);
            }
            return table;
        }
        #region 从Excel的sheet页中进行数据读取
        [Description("Excel的工作页进行逐行读取，justReadHead是否值读取第一行")]
        static void ReadSheetRow(ISheet sheet,ReadRowCallBack rowRead,bool justReadHead) 
        {
            int rowIndex = sheet.LastRowNum;//行号从0开始
            if (justReadHead)
            {
               IRow row= sheet.GetRow(0);
               rowRead(row);
               return;
            }
            for (int i = 0; i <= rowIndex; i++)
            {
                IRow row = sheet.GetRow(i);
                rowRead(row);
            }
        }
        [Description("根据excel文件名的后缀信息选择文件版本")]
        public static EExcelType SelectExcelVersion(string excelFullPath) 
        {
            FileInfo fi = new FileInfo(excelFullPath);
            string ext = fi.Extension;//文件扩展名/文件后缀
            EExcelType type = ext.ToLower() == EExcelType.Xls.ToString().ToLower()
                ? EExcelType.Xls : EExcelType.Xlsx;
            return type;
        }
        [Description("从Excel中进行数据读取,readAllSheet是否读取全部的工作页，设置true时sheetPageIndex无效")]
        public static void ReadSheet(string excelFullPath,bool readAllSheet,int sheetPageIndex, ReadRowCallBack readRowFun, bool justReadHead)
        {
            EExcelType type = SelectExcelVersion(excelFullPath);
            IWorkbook book = GetExcelWorkBook(excelFullPath, type);
            int sheetPage = book.NumberOfSheets;//总共存储多少sheet页
            //是否增加页索引判断->
            if (!readAllSheet)
            {
                if (sheetPageIndex > sheetPage)
                {//超出行索引没必要进行后续操作
                    book.Close();
                    return;
                }
                ReadSheet(book, sheetPageIndex, readRowFun, justReadHead);
                book.Close();
                return;
            }
            for (int i = 0; i < sheetPage; i++)
            {
                ReadSheet(book, i, readRowFun, justReadHead);
            }
            book.Close();//是释放流文件
        }
        [Description("从Excel中读取指定工作页的数据")]
        static void ReadSheet(IWorkbook book, int sheetIndex, ReadRowCallBack readRowFun, bool justReadHead)
        {
            int sheetPage = book.NumberOfSheets;//总共存储多少sheet页
            if (sheetPage < sheetIndex)
            {
                return;
            }
            ISheet sheet = book.GetSheetAt(sheetIndex);
            ReadSheetRow(sheet, readRowFun, justReadHead);
        }
        public static void ReadRowInSheet(IRow row)
        {
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
                }
            }
        }
        #endregion
        [Description("对行数据进行处理的回调函数")]
        public delegate void ReadRowCallBack(IRow row);
        /// <summary>
        /// 数据保存
        /// </summary>
        /// <param name="book"></param>
        /// <param name="fileStream"></param>
        static void SaveSheet(IWorkbook book,FileStream fileStream) 
        {
            book.Write(fileStream);
            fileStream.Close();
            book.Close();
        }
        static void FillSheetHead(ISheet sheet)
        {
            Dictionary<int, IRow> newRow = new Dictionary<int, IRow>();
            foreach (int item in sheetHead.Select(s=>s.RowPosition).Distinct())
            {
                newRow.Add(item, sheet.CreateRow(item));
            }
            //sheetHead = sheetHead.OrderBy(s => s.ColumnIndex).ToList();//进行一次排序
            foreach (ExcelHeadAttribute column in sheetHead)
            {
                ICell cell = newRow[column.RowPosition].CreateCell(column.BeginCellIndex);
                cell.SetCellValue(column.ColumnName);
            }
            //合并单元格 
            foreach (ExcelHeadAttribute item in sheetHead)
            {
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(item.RowPosition, item.RowPosition + item.OccupationRow - 1, item.BeginCellIndex, item.BeginCellIndex + item.OccuopationCell - 1));                
            }
            
        }
        static List<ExcelHeadAttribute> sheetHead;
        public static void DataFillSheet<R>(string fileFullName, EExcelType excel, string targetSheetName, List<ExcelHeadAttribute> head, SheetRowDataToDo<R> fillRowsDataEvent, List<R> rows) where R : class
        {
            sheetHead = head;
            DataFillRowsInSheet<ExcelHeadAttribute, R>(fileFullName, excel, targetSheetName, FillSheetHead, fillRowsDataEvent, rows);
        }
        /// <summary>
        /// 对Excel进行数据写入【操作结束之后会自动进行存储】
        /// </summary>
        /// <param name="fileFullName">excel全路径，如果文件不存在则进行创建</param>
        /// <param name="excel">Excel版本</param>
        /// <param name="targetSheetName">操作的目标Excel工作页</param>
        /// <param name="fillRowEvent">自定义数据填充</param>
        /// <param name="fillRowsDataEvent">可选择的数据填充</param>
        [Description("对Excel进行数据写入")]
        public static void DataFillRowsInSheet<T, R>(string fileFullName, EExcelType excel, string targetSheetName, SheetHeadDataToDo fillRowEvent, SheetRowDataToDo<R> fillRowsDataEvent, List<R> rows)
            where T : class
            where R : class
        {
            //单元格数据填充处理
            #region 选定目标sheet页
            IWorkbook excelBook = GetExcelWorkBook(fileFullName,excel);
            if (excelBook == null)
            { //文件限定不正常

            }
            //判断工作页是否存在
            int index = excelBook.GetSheetIndex(targetSheetName);
            ISheet sheet = null;
            if (index < 0)
            {//不存在则创建 
                sheet = excelBook.CreateSheet(targetSheetName);
            }
            else
            {
                sheet = excelBook.GetSheetAt(index);
            }
            #endregion
            //列头
            fillRowEvent(sheet);
            //行数据
            if (fillRowsDataEvent != null)
                fillRowsDataEvent(sheet,rows);
            //数据存储
            FileStream fs = new FileStream(fileFullName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
            fs.Flush();
            SaveSheet(excelBook, fs);
        }
        /// <summary>
        /// 读取excel的工作单元【如果不存在则新建】
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <param name="excel"></param>
        /// <returns></returns>
        public static IWorkbook GetExcelWorkBook(string fileFullName, EExcelType excel)
        {
            if (File.Exists(fileFullName))
            {
                return ReadFielStreamFormExcel(fileFullName, excel);//文件已存在进行追加时调用
            }
            //文件不存在直接新建
            FileStream fs = new FileStream(fileFullName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            IWorkbook excelBook = null;
            //当文件是新建的时候没法读取文件流
            switch (excel)
            {
                case EExcelType.Xls:
                    excelBook = new HSSFWorkbook();
                    break;
                case EExcelType.Xlsx:
                    excelBook = new NPOI.XSSF.UserModel.XSSFWorkbook();
                    break;
            }

            return excelBook;
        }
        static IWorkbook ReadFielStreamFormExcel(string fileFullName, EExcelType excel)
        {
            FileStream fs = new FileStream(fileFullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            IWorkbook excelBook = null;
            //当文件是新建的时候没法读取文件流
            switch (excel)
            {
                case EExcelType.Xls:
                    excelBook = new HSSFWorkbook(fs);
                    break;
                case EExcelType.Xlsx:
                    excelBook = new NPOI.XSSF.UserModel.XSSFWorkbook(fs);
                    break;
            }
            return excelBook;
        }
    }
    public class ExcelCompareActionHelp
    {
        public Dictionary<ErrorMsgCode, string> errorCode = new Dictionary<ErrorMsgCode, string>();
        Dictionary<ExcelDataSource, Dictionary<int, List<string>>> dataSource = new Dictionary<ExcelDataSource, Dictionary<int, List<string>>>();
        List<CompareData> mapRule = new List<CompareData>();//列匹配规则
        Dictionary<ExcelDataSource, List<int>> compareColumn = new Dictionary<ExcelDataSource, List<int>>();
        bool nowReadFirstExcel = true;
        [Description("列数据格式化处理")]
        public delegate string CellFormatEvent(string cellValue);
        [Description("进行行数据过滤")]
        public delegate bool RowDataFilter(object rowData);
        /// <summary>
        /// 在进行excel列进行数据处理时每个结果单元列处理事件
        /// </summary>
        public Dictionary<int, CellFormatEvent> CellValueFormat = new Dictionary<int, CellFormatEvent>();
        /// <summary>
        /// 自定义excel行数据限定条件【如果使用该数据则返回true】
        /// </summary>
        public RowDataFilter RowDataFilterEvent;
        public enum ErrorMsgCode
        { 
            None=-1,
            FirstExcelIndexDistinct=1,
            SecondExcelIndexDistinct=2
        }
        enum ExcelDataSource
        {
            [Description("第一份Excel（Left）")]
            OriginExcel = 1,
            [Description("第二份Excel（Right）")]
            TargetExcel = 2
        }
        public ExcelCompareActionHelp() 
        {
            errorCode.Add(ErrorMsgCode.None, "Success");
            errorCode.Add(ErrorMsgCode.FirstExcelIndexDistinct, "第一份excel中列序号存在重复项");
            errorCode.Add(ErrorMsgCode.SecondExcelIndexDistinct, "第一份excel中列序号存在重复项");
        }
        public ErrorMsgCode DoExcelCompare(string firstExcelDir, string secondExcelDir, List<CompareData> mapColumn)
        {
            //参数校验->是否列序号存在重复
            int[] left = mapColumn.Select(s => s.OriginHeadIndex).ToArray();
            int[] distinctLeft = mapColumn.Select(s => s.OriginHeadIndex).Distinct().ToArray();
            if (left.Length > distinctLeft.Length)
            {
                return ErrorMsgCode.FirstExcelIndexDistinct;
            }
            int[] right = mapColumn.Select(s => s.NewHeadIndex).ToArray();
            int[] distinctRight = mapColumn.Select(s => s.NewHeadIndex).Distinct().ToArray();
            if (right.Length > distinctRight.Length)
            {
                return ErrorMsgCode.SecondExcelIndexDistinct;
            }
            //读取excel
            dataSource.Clear();
            compareColumn.Clear();
            foreach (var item in ExcelDataSource.TargetExcel.GetEnumMembers())
            {
                ExcelDataSource tar;
                Enum.TryParse(item, out tar);
                compareColumn.Add(tar, new List<int>());//列比较规则
                dataSource.Add(tar, new Dictionary<int, List<string>>());
            }
            foreach (CompareData item in mapColumn)
            {
                compareColumn[ExcelDataSource.OriginExcel].Add(item.OriginHeadIndex);
                compareColumn[ExcelDataSource.TargetExcel].Add(item.NewHeadIndex);
            }
            //进行excel数据入组
            ExcelHelper.ReadSheet(firstExcelDir, false, 0, OrderReaderExcelRow, false);
            nowReadFirstExcel = false;//开始进行第二份excel的读取
            ExcelHelper.ReadSheet(secondExcelDir, false, 0, OrderReaderExcelRow, false);
            int[] firstDiff = DiffInData(dataSource[ExcelDataSource.OriginExcel], dataSource[ExcelDataSource.TargetExcel]);
            //组内excel数据比较
            int[] secondDiff = DiffInData(dataSource[ExcelDataSource.TargetExcel], dataSource[ExcelDataSource.OriginExcel]);
            //比较结果进行差异化汇总入档
            //分别从两份原始excel中读出差异行，写入到另一份excel
            FileInfo fi = new FileInfo(firstExcelDir);
            string newDir = fi.DirectoryName;//目录
            string on= fi.Name.Replace(fi.Extension,string.Empty);
            string excelName =newDir+"\\"+on+ DateTime.Now.ToString(Common.Data.CommonFormat.DateToHourIntFormat) + fi.Extension;
            WriteDiffRowInBook(excelName, 0, firstExcelDir, 0, firstDiff);
            WriteDiffRowInBook(excelName, 1,secondExcelDir, 0, secondDiff);
            return ErrorMsgCode.None;
        }
        /// <summary>
        /// 查找字符串差异索引
        /// </summary>
        /// <param name="firstColl"></param>
        /// <param name="secondColl"></param>
        /// <returns></returns>
        int[] DiffInData(Dictionary<int,List<string>> firstColl,Dictionary<int,List<string>> secondColl) 
        {
            List<int> diff = new List<int>();
            foreach (KeyValuePair<int, List<string>> first  in firstColl)
            {//第二份结果中的差异
                bool found = false;
                foreach (KeyValuePair<int, List<string>> second in secondColl)
                {//比较这一行是否存在相应数据
                    if (string.Join("", second.Value) == string.Join("", first.Value))
                    {
                        found = true;
                        break;
                    }
                }
                //记录没有比较出的数据结果集
                if (!found)
                {
                    diff.Add(first.Key);
                }
            }
            return diff.ToArray();
        }
        void OrderReaderExcelRow(IRow row) 
        {
            int index = row.RowNum;//这是第几行
            if (index == 0)
            {//不读取列信息
                return;
            }
            List<int> cellIndex;
            ExcelDataSource target;
            if (nowReadFirstExcel)
            {
                target=ExcelDataSource.OriginExcel;
                cellIndex = compareColumn[target];
            }
            else 
            {
                target=ExcelDataSource.TargetExcel;
                cellIndex = compareColumn[target];
            }
            dataSource[target].Add(index,new List<string>());
            List<string> leftRowData = new List<string>();//行数据
            if (RowDataFilterEvent == null) 
            {
                RowDataFilterEvent = ValidRowDataExtend;//行数据校验
            }
            List<string> rowString = new List<string>();
            for (int i = 0; i < cellIndex.Count; i++)
            {
                int ci = cellIndex[i];
                ICell cell = row.GetCell(ci);
                ICellStyle cs = cell.CellStyle;
                //如果是日期列则需要进行特殊处理
                float temp = 0;
                //是否需要对列进行特殊化处理：比如int数据在数据表中增加了 ".00"后缀,以及对日期类型进行规范化处理
                string valueStr = cell == null ? string.Empty : cell.ToString().Trim();
                if (cell.CellType == CellType.Numeric && ! float.TryParse(valueStr, out temp))
                {
                    DateTime time = cell.DateCellValue;
                    valueStr = time.ToString("yyyy-MM-dd");
                }
                //excel 使用npoi操作读取单元格 ，发现使用NumericCellValue取出的值为203.0，cell.ToString()取出的值为203
                if (CellValueFormat.ContainsKey(i))
                {
                   valueStr= CellValueFormat[i](valueStr);//使用比较集合中的索引是为了兼容两个excel中列头排序不一致的情形
                }
                rowString.Add(valueStr);
            }
            string[] rowDataArr = rowString.ToArray();
            if (RowDataFilterEvent(rowDataArr))
            {
                dataSource[target][index].AddRange(rowDataArr);
            }
        }
        void WriteDiffRowInBook(string excelFullPath,int writeSheetIndex,string dataSourceFullPath,int sheetIndex,int[] rowIndex) 
        {
            EExcelType version = ExcelHelper.SelectExcelVersion(excelFullPath);
            IWorkbook book = ExcelHelper.GetExcelWorkBook(excelFullPath, version);
            ISheet sheet= ExcelHelper.ReadExcelSheet(dataSourceFullPath, version, sheetIndex);
            IRow head = sheet.GetRow(0);
            ISheet excuteSheet = null;
            if (book.NumberOfSheets>0&&book.NumberOfSheets-1 >= writeSheetIndex)
            {
                excuteSheet = book.GetSheetAt(writeSheetIndex);
            }
            else 
            {
                excuteSheet = book.CreateSheet();
            }
            IRow wr= excuteSheet.CreateRow(0);//这是要写入的头
            int cellSize = head.LastCellNum;
            for (int i = 0; i < cellSize; i++)
            {
                ICell cell=head.GetCell(i);
                ICell wc= wr.CreateCell(i,cell==null?CellType.String: cell.CellType);
                wc.SetCellValue(cell==null?string.Empty: cell.ToString());
            }
            int rowPoint = 0;
            for (int i = 0; i < rowIndex.Length; i++)
            {
                IRow tr =sheet.GetRow( rowIndex[i]);
                rowPoint++;//行位置重新编排
                IRow wrRow = excuteSheet.CreateRow(rowPoint);
                for (int c = 0; c < cellSize; c++)
                {
                    ICell cell = tr.GetCell(c);
                    ICell wc = wrRow.CreateCell(c, cell == null ? CellType.String : cell.CellType);
                    wc.SetCellValue(cell == null ? string.Empty : cell.ToString());
                }
            }
            //保存
            FileStream fs = new FileStream(excelFullPath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite); //数据存储
            fs.Flush();
            book.Write(fs);
            fs.Close();
            book.Close();
        }
        bool ValidRowDataExtend(object rowData)
        {//兼容验证行数据
            return true;   
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
