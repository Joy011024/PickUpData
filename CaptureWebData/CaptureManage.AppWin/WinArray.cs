using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.GlobalModel;
using Infrastructure.ExtService;
using System.Windows.Forms;
namespace CaptureManage.AppWin
{
    public class WinArray
    {
        public Dictionary<string, List<ClassInfo>> winGroup = new Dictionary<string, List<ClassInfo>>();
        public WinArray() 
        {
            InitWinGroup();
        }
        //将窗体进行分组
        void InitWinGroup() 
        {
            string dir = NowAppDirHelper.GetNowAppDir(AppCategory.WinApp);
            string winListDir = dir + "/CaptureManage.AppWin.dll";
            Form[] frms= AssemblyHelp.GetAllType<Form>(winListDir);
            string assName = (new AssemblyInfo()).GetAssemblyName();
            winGroup = new Dictionary<string, List<ClassInfo>>();
            if (frms != null && frms.Length > 0)
            {
                List<ClassInfo> wins = new List<ClassInfo>();
                foreach (Form item in frms)
                {
                    ClassInfo web = new ClassInfo()
                    {
                        ClassName=item.Name,
                        AssemblyName = assName,
                        Display=item.Text
                    };
                    wins.Add(web);
                  
                }
                winGroup.Add("News", wins);
                return;
            }
            else
            {
               
                List<ClassInfo> wins = new List<ClassInfo>();
                ClassInfo web = new ClassInfo()
                {
                    ClassName = typeof(WebDataCaptureForm).Name,
                    AssemblyName = assName,
                    Display = typeof(WebDataCaptureForm).Name
                };
                wins.Add(web);
                winGroup.Add("News", wins);
            }
        }
         
    }
    public class AssemblyInfo
    {
        public string GetAssemblyName()
        {
            string  info= this.GetType().Assembly.FullName;
            // CaptureManage.AppWin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
            return info.Split(',')[0];
        }
    }
}
