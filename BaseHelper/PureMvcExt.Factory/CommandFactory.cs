using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PureMVC.Interfaces;
namespace PureMvcExt.Factory
{
    public class CommandFactory : PureMVC.Patterns.Command.SimpleCommand
    {

        public override void Execute(INotification notification)
        {
            base.Execute(notification);
        }
        public override void SendNotification(string notificationName, object body = null, string type = null)
        {
            base.SendNotification(notificationName, body, type);
        }
    }
}
