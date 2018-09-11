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
namespace CefSharpWin
{
    public partial class MoniterTicket : FormMediatorService
    {
        public MoniterTicket()
        {
            InitializeComponent(); 
        }
        #region 消息传递
        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case NotifyList.Notify_Refresh_Contacter://接收联系人列表
                    object data = notification.Body;
                    //Notify_Close_Account
                    /*
                     System.InvalidOperationException:“在创建窗口句柄之前，不能在控件上调用 Invoke 或 BeginInvoke。”
                     */
                    
                    //发送消息去关闭登陆窗体
                    SendNotification(NotifyList.Notify_Close_Account);
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
