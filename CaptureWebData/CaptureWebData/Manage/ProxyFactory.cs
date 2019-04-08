using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PureMVC.Interfaces;
namespace CaptureWebData
{
    public class ProxyFactory : IProxy
    {
        private string proxyName;
        private object notifyData;
        public ProxyFactory(string name)
        {
            proxyName = name;

        }
        public string ProxyName => proxyName;

        public object Data { get => notifyData; set => notifyData=value; }

        public void InitializeNotifier(string key)
        {
            
        }

        public void OnRegister()
        {
          
        }

        public void OnRemove()
        {
            
        }

        public void SendNotification(string notificationName, object body = null, string type = null)
        {
          
        }
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
}
