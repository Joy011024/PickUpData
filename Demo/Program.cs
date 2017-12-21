using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TimeIntervalListen;
using Domain.CommonData;
using Infrastructure.ExtService;
using System.IO;
using Common.Data;
using System.IO.Packaging;
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
            Test();
            Application.Run(new JobDetailFrm());
        }
        static void Test() 
        {
            string dir=new AppDirHelper().GetAppDir(AppCategory.WinApp);
            string zipDir= new DirectoryInfo(dir).Parent.Parent.FullName+"/ZipTemplate";
            ZipFileHelper zip = new ZipFileHelper();
            zip.GenerateZip(zipDir, dir +"/"+ELogType.ZipLog.ToString(), 
                //DateTime.Now.ToString(CommonFormat.DateTimeIntFormat)+
                "000886197005184001_100.zip");
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

    }
}
