using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.GlobalModel;
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
            winGroup = new Dictionary<string, List<ClassInfo>>();
            List<ClassInfo> wins = new List<ClassInfo>();
            string assName = (new AssemblyInfo()).GetAssemblyName();
            ClassInfo web = new ClassInfo() 
            {
                ClassName = typeof(WebDataCaptureForm).Name,
                AssemblyName = assName
            };
            wins.Add(web);
            winGroup.Add("News", wins);
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
