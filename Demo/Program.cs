using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TimeIntervalListen;
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
           
            Application.Run(new JobDetailFrm());
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
}
