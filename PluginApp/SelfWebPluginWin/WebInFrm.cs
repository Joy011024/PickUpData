using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataService.LocalData.SQLite;
using System.IO;
using System.Data.SQLite;
namespace SelfWebPluginWin
{
    public partial class WebInFrm : Form
    {
        public WebInFrm()
        {
            InitializeComponent();
            bg.DoWork += new DoWorkEventHandler(Work);
            StartCycle();
            Test();
        }
        #region private member 
        private BackgroundWorker bg = new BackgroundWorker();
        private bool ContinuteFlag { get; set; }
        #endregion
        #region contructor 
        #endregion
        #region override  
        #endregion

        #region event
        private void Work(object sender,DoWorkEventArgs e)
        {
            while (true)
            {
                if (!ContinuteFlag)
                {
                    return;
                }
                CycleWork();
                System.Threading.Thread.Sleep(1000 * 1);//轮询间隔
            }
        }

        #endregion
        #region self function
        private void CycleWork()
        {

        }
        private void StartCycle()
        {
            if (bg.IsBusy)
            {
                return;
            }
            ContinuteFlag = true;
            bg.RunWorkerAsync();
        }
        private void PauseCycle()
        {
            ContinuteFlag = false;
        }
        private void Test()
        {
            AppSetting db = new AppSetting()
            {
                Name = "AppVersion",
                CreateTime = DateTime.Now,
                Id = 1,
                Statues = 0,
                Value = "1.0",
                Desc = "程序版本"
            };
            string appPath = AppDomain.CurrentDomain.BaseDirectory;//当前程序目录
            // @"PluginApp\SelfWebPluginWin\bin\Debug";
            string dbPath= CycleParent(4, appPath) + @"\LocalData.SQLite\AppSetting.db" ;
            //检测是否存在
            string connString =string.Format( "data source={0}",dbPath);
            try
            {
                DBReporistory dbReporistory = new DBReporistory();
                dbReporistory.AddList(new AppSetting[] { db });
                /*
                 No Entity Framework provider found for the ADO.NET provider with invariant name 'System.Data.SqlClient'. Make sure the provider is registered in the 'entityFramework' section of the application config file. See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.
                 */
            }
            catch (Exception ex)
            {

            }
        }
        private string CycleParent(int cycle,string path)
        {
            cycle--;
            DirectoryInfo di = new DirectoryInfo(path);
            if (cycle < 1)
            {
                return di.Parent.FullName;
            }
            if (di.Parent == di.Root)
            {
                return di.Root.FullName;
            }
            return CycleParent(cycle, di.Parent.FullName);
        }
        #endregion
    }
    [System.Data.Linq.Mapping.Table(Name = "AppSetting")]
    public class AppSetting
    {
        [System.ComponentModel.DataAnnotations.Key]
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Value { get; set; }
        public virtual DateTime CreateTime { get; set; }
        public virtual int Statues { get; set; }
        public virtual string Desc { get; set; }
    }
}
