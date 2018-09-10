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
