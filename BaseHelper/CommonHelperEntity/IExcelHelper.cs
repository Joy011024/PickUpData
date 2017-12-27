using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;
using System.Data.OleDb;
namespace CommonHelperEntity
{
    public class ExcelDataHelper : IExcelHelper { }
    public interface IExcelHelper
    {
    }
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
    }
}
