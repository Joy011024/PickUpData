using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PureMVC.Interfaces;
namespace CaptureWebData
{
    public class APPNotify
    {
        public const string Cmd_Get_SpilderUIN = "Cmd_Get_SpilderUIN";//查询统计
        public const string Cmd_Response_SpilderUIN = "Cmd_Response_SpilderUIN";//反馈
    }
    public class CommonFacade : PureMVC.Patterns.Facade.Facade
    {
        private static CommonFacade mInstacne = null;
        public static CommonFacade FacadeInstance
        {
            get
            {
                if (mInstacne == null)
                {
                    mInstacne = new CommonFacade("Facade");
                }
                return mInstacne;
            }
        }
        public CommonFacade(string key) : base(key)
        { }
        protected override void InitializeController()
        {
            base.InitializeController();
        }
        protected override void InitializeModel()
        {
            base.InitializeModel();
            try
            {

            }
            catch (Exception ex)
            {
            }
        }
    }
    public class CommonCommand : PureMVC.Patterns.Command.SimpleCommand
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
