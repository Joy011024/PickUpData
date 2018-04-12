using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Management;
using Common.Data;
namespace CommonHelperEntity
{
    public class MachineInfor 
    {
        public  string ProcessorId = "ProcessorId";
        public  string Name = "Name";
        public  string Status = "Status";
        public  string SystemName = "SystemName";
    }
    public static class IpHelp
    {
        public static bool CanPing(string ip) 
        {
            if (string.IsNullOrEmpty(ip)) 
            {
                return false;
            }
            try
            {
                Ping ping = new Ping();
            
                PingReply pr = ping.Send(ip);
                bool success = false;
                if (pr != null) 
                {
                    success = (pr.Status == IPStatus.Success);
                }
                IDisposable dis = ping;
                dis.Dispose();
                return success;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 需要使用System.Management程序集
        /// </summary>
        public static Dictionary<string, object> GetMechineInfo(string manageClassName = "Win32_Processor") 
        {//获取设备IP，设备名称等信息
            Dictionary<string, object> manage = new Dictionary<string, object>();
            ManagementClass mc = new ManagementClass(manageClassName);
            StringBuilder sb = new StringBuilder();
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject item in moc)
            {
                PropertyDataCollection pds = item.Properties;
                foreach (PropertyData data in pds)
                {
                    object value = data.Value;
                    Console.WriteLine(data.Name + "\t" + value);
                    manage.Add(data.Name, value);
                    sb.AppendLine(data.Name + "\t" + value);
                }
            }
            LogHelper help = new LogHelper();
            help.AppendLogUsingProjectPath(sb.ToString(), "Log", "ManagementClass_" + DateTime.Now.ToString(CommonFormat.DateToMinuteIntFormat) + FileSuffix.Log,(new int?()));
            return manage;
        }
        public static bool CanConnection(string ip, int port)
        {
            IPAddress address;
            bool can = IPAddress.TryParse(ip, out address);
            if (!can)
            {
                return false;
            }
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPort = new IPEndPoint(address, port);
            IAsyncResult ar = socket.BeginConnect(endPort, new AsyncCallback(SocketAsycnCallBack), null);
            ar.AsyncWaitHandle.WaitOne(600);//设置线程等待时间能判断链接结果(可是该方法不合理，不能确定所有的socket执行时间)
            //ar.AsyncWaitHandle.WaitOne(-1);//进行线程等待
            return socket.Connected;
        }
        private static void SocketAsycnCallBack(IAsyncResult ar) 
        {
        
        }
        public static bool SocketIsConnectBySend(string ip,int port) 
        {
            IPEndPoint endPort = GetIPEndPoint(ip, port);
            if (endPort == null)
            {
                return false;
            }
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Blocking = false;
            byte[] bt = new byte[1];
            try
            {
                socket.BeginConnect(endPort, new  AsyncCallback(SocketAsycnCallBack), null);
                socket.Send(bt);
                return true;
            }
            catch (SocketException ex) 
            {
                // NativeErrorCode 产生 10035 == WSAEWOULDBLOCK 错误，说明被阻止了，但是还是连接的
                //? 10061
                return false;
            }
        }
        private static IPEndPoint GetIPEndPoint(string ip, int port)
        {
            IPAddress address;
            bool can = IPAddress.TryParse(ip, out address);
            if (!can)
            {
                return null;
            }
            IPEndPoint endPort = new IPEndPoint(address, port);
            return endPort;
        }
    }
}
