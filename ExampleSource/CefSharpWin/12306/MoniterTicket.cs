﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PureMVC.Interfaces;
namespace CefSharpWin
{
    public partial class MoniterTicket : FormMediatorService
    {

        #region private member 
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
            Dictionary<string, string> contact = new Dictionary<string, string>();
            contact.Add("passenger_name", "姓名");
            lstContact.Columns.Clear();
            lstContact.View = View.Details;
            foreach (var item in contact)
            {
                lstContact.Columns.Add(new ColumnHeader() { Name = item.Key, Text = item.Value });
            }
        }
        private void LoadContacters(Ticket12306Resonse conts)
        {
            List<Rider> cons = conts.data.normal_passengers;
            for (int i = 0; i < cons.Count; i++)
            {
                ListViewItem row = new ListViewItem() { Name=i.ToString() };
               

                lstContact.Items.Add(row);
            }
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
        #endregion
    }
}
