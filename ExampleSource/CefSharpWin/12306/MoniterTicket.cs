using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PureMVC.Interfaces;
using Domain.CommonData;
namespace CefSharpWin
{
    public partial class MoniterTicket : FormMediatorService
    {

        #region private member 
        private bool mExistsStation = false;
        #endregion
        #region contructor 
        public MoniterTicket()
        {
            InitializeComponent();
            InitEle();
        }
        #endregion
        #region override  
        #endregion

        #region event


        #endregion
        #region self function
        private void InitEle()
        {
            lstContact.Scrollable = true;
            lstSchedule.Scrollable = true;
            Grid contactCols = XmlService.GetGridSetting()["Contacter"];
            BindGridColumn(lstContact, contactCols.Columns);
            Grid carScheCols = XmlService.GetGridSetting()["CarSchedule"];
            BindGridColumn( lstSchedule, carScheCols.Columns);
            //是否存在车站信息
            AppSetting appSetting = XmlService.GetAppSetting();
            string file = FileHelper.ReadFile(SystemConfig.DebugDir + appSetting.SystemSetting.KeepStatic.StationJsonFile);
            if (!string.IsNullOrEmpty(file) && !string.IsNullOrEmpty(file.Trim()))
            {
                //是否能解析为json串
                RegexHelper.GetMatchValue(file, "");
            }
            else {
                DownloadStation();
            }
        }
        private void BindGridColumn(ListView grid, Columns gridColumn)
        {
            grid.Columns.Clear();
            if (gridColumn == null)
            {
                return;
            }
            grid.View = View.Details;
            grid.GridLines = true;
            foreach (var item in gridColumn.Heads)
            {
                grid.Columns.Add(new ColumnHeader() { Name = item.Name, Text = item.Text, Width = item.Width });
            }
        }
        private void LoadContacters(Ticket12306Resonse conts)
        {
            string tip = conts.data.exMsg;
            if (!string.IsNullOrEmpty(tip))
            {
                return;
            }
            List<Rider> cons = conts.data.normal_passengers;
            for (int i = 0; i < cons.Count; i++)
            {
                ListViewItem row = new ListViewItem() { Name=i.ToString() };
               

                lstContact.Items.Add(row);
            }
        }
        private void DownloadStation()
        {
            string url = XmlService.GetAppSetting().SystemSetting.KeepStatic.StationAPI;
            string station= HttpHelper.GetResponse(url);
           // station.WriteLog(ELogType.DebugData, false);
           //进行解析
        }
        #endregion
        #region 消息传递
        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case NotifyList.Notify_Refresh_Contacter://接收联系人列表
                    Ticket12306Resonse data = notification.Body as Ticket12306Resonse;
                    //Notify_Close_Account
                    /*
                     System.InvalidOperationException:“在创建窗口句柄之前，不能在控件上调用 Invoke 或 BeginInvoke。”
                     */
                    
                    //发送消息去关闭登陆窗体
                    SendNotification(NotifyList.Notify_Close_Account);
                    LoadContacters(data);
                    ShowDialog();
                    break;
                case NotifyList.Notify_ToFront_Contacter:

                    break;
            }
        }
        public override IList<string> ListNotificationInterests()
        {
            return new string[] {
               NotifyList.Notify_Refresh_Contacter,
               NotifyList.Notify_ToFront_Contacter
           };
        }
        private void CheckBox_Click(object sender,EventArgs e)
        {
            CheckBox ck = sender as CheckBox;
            if (!ck.Checked)
            {
                ckAll.Checked = false;
                return;
            }
            foreach (var item in ck.Parent.Controls)
            {
                CheckBox sibling = item as CheckBox;
                if (sibling == null)
                {
                    continue;
                }
                if (!sibling.Checked)
                {
                    return;
                }
            }
            ckAll.Checked = true;
        }
        private void CheckAll_Click(object sender,EventArgs e)
        {
            CheckBox ck = sender as CheckBox;
            if (!ck.Checked)
            {
                return;
            }
            foreach (var item in carTypePanel.Controls)
            {
                CheckBox type = item as CheckBox;
                if (type == null)
                {
                    continue;
                }
                type.Checked = true;
            }
        }
        #endregion
    }
}
