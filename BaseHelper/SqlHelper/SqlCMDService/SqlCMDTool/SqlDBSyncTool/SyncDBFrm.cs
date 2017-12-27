using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Window.DataHelper;
using SqlCmd.Model;
using CommonHelperEntity;
using System.Configuration;
using MsSqlHelper;
namespace SqlDBSyncTool
{
    public partial class SyncDBFrm : BaseForm
    {
        SqlConnString target = new SqlConnString();
        SqlConnString Sync = new SqlConnString();
        string testConnSqlFormat = "服务器：{0} ,测登录用户:{1} ,数据库:{2} ,测试结果:{3} ,测试连接时间:{4} ";
        public SyncDBFrm()
        {
            InitializeComponent();
        }

        private void SyncDBFrm_Load(object sender, EventArgs e)
        {

        }

        private void Button_Click(object sender, EventArgs e)
        {
            GetConfigData();
            //记录页面上输入的数据库连接字符串信息是否变动，如果没有变动则不需要再遍历页面控件
            SqlHelper help = new SqlHelper(target.GetConnString());
            bool can = help.ConnectionSuccess;
        }
        void Init()
        {
          

        }
        void GetConfigData()
        {
            target.GetClassFromControl(panelTarget.Controls);
            Sync.GetClassFromControl(panelSync.Controls);
        }
        void DoConnTest(SqlConnString str) 
        {//每次测试数据库连接情况，都将部分信息显示到界面上
            SqlHelper help = new SqlHelper(str.GetConnString());
            bool can = help.ConnectionSuccess;
            lstbTip.Items.Add(string.Format(testConnSqlFormat, str.DataSource, str.UserId, str.InitialCatalog, can, DateTime.Now.ToString(DateTimeUnitlf)));
        }
    }
}
