using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Domain.CommonData;
namespace GatherIdCard
{
    public class HtmlElementAnalysis
    {
        public static void IdgxskyHtmlAnalysic(string htmlDir) 
        {
            DirectoryInfo di = new DirectoryInfo(htmlDir);
            FileInfo[] fis= di.GetFiles("*.txt");
            if (fis.Length == 0)
            { 
            
            }
            FileInfo htmlFile = fis[0];
            string fullName= htmlFile.FullName;
            string html = FileHelper.ReadFile(fullName);
        }
    }
    public interface IA 
    {
        void FunA();
    }
    public interface IB
    {
        void FunB();
    }
    public interface IFun : IA, IB
    { }
}
