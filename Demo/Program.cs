using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TimeIntervalListen;
using Domain.CommonData;
using Infrastructure.ExtService;
using System.IO;
using Common.Data;
using System.IO.Packaging;
using System.Text;
using System.Text.RegularExpressions;
using CommonHelperEntity;
using NPOI.XSSF.UserModel;
namespace Demo
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            TestMultipleHead();
            Application.Run(new JobDetailFrm());
        }
        static void Test() 
        {

            //使用xpath <B class=ui-page-s-len>2/100</B>
            Regex reg = new Regex("<B[^>]*>(.+)</B>");//提取符合要求的一段文本
            GroupCollection gc= reg.Match("<B class=ui-page-s-len>2/100</B>").Groups;
            if (gc.Count > 1)
            {//第一项为匹配的完整串，第二项为标签内的文本  [2/100]
                Group g = gc[1];
                string value = g.Value;
            }
            //提取标签内的文本 2/100
            string pu = "<B[^>]*>(.*?)</B>";
            Regex inner = new Regex(pu);
            GroupCollection pugc = inner.Match("<B class=ui-page-s-len>2/100</B>").Groups;
            if (pugc.Count > 0)
            {
                Group g = pugc[0];
                string value = g.Value;
            }
            return;
            string dir=new AppDirHelper().GetAppDir(AppCategory.WinApp);
            string zipDir= new DirectoryInfo(dir).Parent.Parent.FullName+"/ZipTemplate";
            ZipFileHelper zip = new ZipFileHelper();
            zip.Test();
            //zip.GenerateZip(zipDir, dir +"/"+ELogType.ZipLog.ToString(), 
                //DateTime.Now.ToString(CommonFormat.DateTimeIntFormat)+
                //"000886197005184001_100.zip");
        }
        static void TestMultipleHead()
        {
            List<ExcelHeadAttribute> heads = new List<ExcelHeadAttribute>();
            heads.Add(new ExcelHeadAttribute()
            {
                ColumnName = "姓名",
                ColumnIndex = 0,
                ColumnWidth = 200,
                BeingCellIndex = 0,
                OccuopationCell = 1,
                OccupationRow = 2,
                RowPosition = 0
            });
            heads.Add(new ExcelHeadAttribute()
            {
                ColumnName = "复训带教",
                ColumnIndex = 1,
                ColumnWidth = 200,
                BeingCellIndex = 1,
                OccuopationCell = 2,
                OccupationRow = 1,
                RowPosition = 0
            });
            heads.Add(new ExcelHeadAttribute()
            {
                ColumnName = "复训检查",
                ColumnIndex = 2,
                ColumnWidth = 200,
                BeingCellIndex = 3,
                OccuopationCell = 2,
                OccupationRow = 1,
                RowPosition = 0
            });
            heads.Add(new ExcelHeadAttribute()
            {
                ColumnName = "复训翻译",
                ColumnIndex = 3,
                ColumnWidth = 200,
                BeingCellIndex = 5,
                OccuopationCell = 2,
                OccupationRow = 1,
                RowPosition = 0
            });
            //子行
            heads.Add(new ExcelHeadAttribute()
            {
                ColumnName = "小时",
                ColumnWidth = 200,
                BeingCellIndex = 1,
                OccuopationCell = 1,
                OccupationRow = 1,
                RowPosition = 1
            });
            heads.Add(new ExcelHeadAttribute()
            {
                ColumnName = "日期",
                ColumnWidth = 200,
                BeingCellIndex = 2,
                OccuopationCell = 1,
                OccupationRow = 1,
                RowPosition = 1
            });
            DateTime now = DateTime.Now;
            string time = now.ToString(Common.Data.CommonFormat.DateTimeIntFormat) + "." + CommonHelperEntity.EExcelType.Xlsx;
            string dir = new AppDirHelper().GetAppDir(AppCategory.WinApp);
            string fullName = dir + "/" + time;
            ExcelHelper.DataFillSheet(fullName, EExcelType.Xlsx, "1", heads, DoFillRowToExcelSheet, new List<ExcelHeadAttribute>());
        }
        static void TestExcel() 
        {
            DateTime now=DateTime.Now;
            string time = now.ToString(Common.Data.CommonFormat.DateTimeIntFormat)+"."+CommonHelperEntity.EExcelType.Xlsx;
            string dir = new AppDirHelper().GetAppDir(AppCategory.WinApp);
            string fullName = dir + "/" + time;
            //合并单元格
            ExcelHelper.DataFillSheet<ExcelHeadAttribute, ExcelHeadAttribute>(fullName, EExcelType.Xlsx, now.ToString(Common.Data.CommonFormat.DateIntFormat), DoMergeExcelSheet, DoFillRowToExcelSheet, new List<ExcelHeadAttribute>());
        }
        static void DoMergeExcelSheet(NPOI.SS.UserModel.ISheet sheet)
        {
            string[] heads = new string[] { "姓名", "复训带教", "复训检查", "复训翻译" };
            //增加列头
            //设置列头直接进行单元格合并
            NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);
            row.Height = 1024;
            for (int i = 0; i < heads.Length; i++)
            {
                int cellIndex = i > 0 ? 2 * i - 1 : i;
                NPOI.SS.UserModel.ICell cell = row.CreateCell(cellIndex);
                cell.SetCellValue(heads[i]);
            }
            for (int i = 0; i < heads.Length; i++)
            {
                if (i == 0)
                {
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 0, 0));
                }
                else
                {
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 2 * i - 1, 2 * i));
                }
            }
        }
        static void DoFillRowToExcelSheet(NPOI.SS.UserModel.ISheet sheet, List<ExcelHeadAttribute> rows)
        { 
        //当前存在多少行数据
            int ri= sheet.LastRowNum;
            //文件未写入完成时如何统计当前已写入了多少行数据
            int rowNumber = sheet.PhysicalNumberOfRows;//当前写入了多少行数据
           
        }
    }
   
    public class QueryJob
    {
        public void GetAllActiveJob() 
        {
            QuartzJob job = new QuartzJob("Query");
            job.GetAllActiveJob();
        }
    }
    public class ZipFileHelper
    {
        public void GenerateZip(string zipSourceDir,string zipDir,string zipFileName) 
        {
            //将目录下的文件生成压缩包
            PackageFolder(zipSourceDir, zipDir,zipFileName, true);
        }
        static bool PackageFolder(string folderName, string zipDir,string zipFileName, bool overrideExisting)
        {
            string zipExt = ".zip";
            string logDir = new AppDirHelper().GetAppDir(AppCategory.WinApp)+typeof(ELogType).Name;
            if (folderName.EndsWith(@"\")||folderName.EndsWith("/"))
                folderName = folderName.Remove(folderName.Length - 1);
            bool result = false;
            if (!Directory.Exists(folderName))
            {
                return result;
            }
            string compressedFileName = zipDir + "/" + zipFileName;
            if (!overrideExisting && File.Exists(compressedFileName))
            {
                return result;
            }
            try
            { 
                string log = DateTime.Now.ToString(CommonFormat.DateIntFormat) + ".log";
                if (!Directory.Exists(zipDir))
                {
                    Directory.CreateDirectory(zipDir);
                }
                
                using (Package package = Package.Open(compressedFileName, FileMode.Create))
                {
                    LoggerWriter.CreateLogFile("Zip Data Source\t"+folderName + "\r\n", logDir, ELogType.DebugData, log, true);
                    var fileList = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);
                    foreach (string fileName in fileList)
                    {
                        LoggerWriter.CreateLogFile(fileName + "\r\n", logDir, ELogType.DebugData, log, true);
                        string fileFullName = fileName.Replace("\\", "/");
                        folderName = folderName.Replace("\\", "/");
                        //The path in the package is all of the subfolders after folderName
                        string pathInPackage = fileFullName.Replace(folderName, string.Empty);
                        //判断开始位置是否含有压缩文件名，存在则从路径中剔除
                        //压缩包中需要剔除所在的目录【保留相对目录】
                        string zipReleativeDir = zipFileName.Replace(zipExt, string.Empty);
                        string sign = "/" + zipReleativeDir;
                        if (pathInPackage.IndexOf(sign) == 0) 
                        {
                            pathInPackage = pathInPackage.Substring(pathInPackage.IndexOf(sign)+ sign.Length);
                        }
                        else if (pathInPackage.IndexOf(zipReleativeDir) == 0)
                        {
                            pathInPackage = pathInPackage.Substring(pathInPackage.IndexOf(zipReleativeDir)+ zipReleativeDir.Length);
                        }
                            //Path.GetDirectoryName(fileName).Replace(folderName, string.Empty) + "/" + Path.GetFileName(fileName);
                       //提取到的相对路径 
                        LoggerWriter.CreateLogFile(pathInPackage + "\r\n", logDir, ELogType.DebugData, log, true);
                        Uri partUriDocument = PackUriHelper.CreatePartUri(new Uri(pathInPackage, UriKind.Relative));
                        PackagePart packagePartDocument = package.CreatePart(partUriDocument, "", CompressionOption.Maximum);
                        using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                        {
                            fileStream.CopyTo(packagePartDocument.GetStream());
                        }
                    }
                }
                result = true;
            }
            catch (Exception e)
            {
                LoggerWriter.CreateLogFile(e.ToString(), logDir, ELogType.ErrorLog);
                //throw new Exception("Error zipping folder " + folderName, e);
            }

            return result;
        }
        public  void Test()
        {
            string[] maxims = new string[]{
                "事常与人违，事总在人为",
                "骏马是跑出来的，强兵是打出来的",
                "驾驭命运的舵是奋斗。不抱有一丝幻想，不放弃一点机会，不停止一日努力。 ",
                "如果惧怕前面跌宕的山岩，生命就永远只能是死水一潭", 
                "懦弱的人只会裹足不前，莽撞的人只能引为烧身，只有真正勇敢的人才能所向披靡"
            };
            StringBuilder sb = new StringBuilder();
            foreach (string item in maxims)
            {
               string pinying= item.TextConvertChar(true);
               sb.AppendLine(item + "\t" + pinying);
            }
            LoggerWriter.CreateLogFile(sb.ToString(), new AppDirHelper().GetAppDir(AppCategory.WinApp), ELogType.DebugData);
        }
    }
}
