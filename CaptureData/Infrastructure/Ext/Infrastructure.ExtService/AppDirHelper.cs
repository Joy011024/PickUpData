using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Infrastructure.ExtService
{
    public enum AppCategory
    {
        [Description("Web应用")]
        WebApp=1,
        [Description("桌面应用")]
        WinApp=2
    }
    public class AppDirHelper
    {
        public string GetAppDir(AppCategory category) 
        {
            string dir = string.Empty;
            switch (category)
            {
                case AppCategory.WebApp:
                    dir = GetWebAppDir();
                    break;
                case AppCategory.WinApp:
                    dir = GetWinAppDir();
                    break;
                default:
                    break;
            }
            return dir;
        }
        string GetWinAppDir() 
        {
            string path = this.GetType().Assembly.Location;
            DirectoryInfo di = new DirectoryInfo(path);
            return di.Parent.FullName;
        }
        string GetWebAppDir() 
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            return dir;
        }
    }
    public class NowAppDirHelper
    {
        /// <summary>
        /// 根据提供的程序类型【web或者桌面应用程序】获取当前目录
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static string GetNowAppDir(AppCategory category)
        {
            return (new AppDirHelper()).GetAppDir(category);
        }
    }
}
