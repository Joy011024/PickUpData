using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PureMVC.Patterns.Proxy;
using PureMVC.Interfaces;
namespace PureMvcExt.Factory
{
    public class ProxyFactory: IProxy
    {
        private string proxyName;
        private object notifyData;
        public ProxyFactory() : this("ProxyFactory")
        {

        }
        public ProxyFactory(string name)
        {
            proxyName = name;

        }
        public string ProxyName => proxyName;

        public object Data { get => notifyData; set => notifyData = value; }

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
}
