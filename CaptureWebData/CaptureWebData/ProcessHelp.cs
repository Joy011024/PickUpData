using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using PureMVC.Interfaces;
using System.Threading.Tasks;
using Domain.CommonData;
namespace CaptureWebData
{
    /// <summary>
    /// 由于调用Quartz会在后台开启控制台主机进程，如果quartz使用不当将出现多个进程运行的情况
    /// </summary>
    public class ProcessHelp
    {
        public static void ForeachProcess()
        {
            Process now = Process.GetCurrentProcess();//当前进程
            //获取当前运行的设备名
            string host = Dns.GetHostName();
            Process[] machineP = Process.GetProcesses(host);//获取当前运行设备上的进程

        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct IpHeader
    {
        [FieldOffset(0)]
        public byte ip_verlen; // IP version and IP Header length
        [FieldOffset(1)]
        public byte ip_tos; // Type of service
        [FieldOffset(2)]
        public ushort ip_totallength; // total length of the packet
        [FieldOffset(4)]
        public ushort ip_id; // unique identifier
        [FieldOffset(6)]
        public ushort ip_offset; // flags and offset
        [FieldOffset(8)]
        public byte ip_ttl; // Time To Live
        [FieldOffset(9)]
        public byte ip_protocol; // protocol (TCP, UDP etc)
        [FieldOffset(10)]
        public ushort ip_checksum; //IP Header checksum
        [FieldOffset(12)]
        public uint ip_srcaddr; //Source address
        [FieldOffset(16)]
        public uint ip_destaddr;//Destination Address
    }
    class RawSocket
    {
        Socket socket;
        String IP = "218.20.242.189";
        private String StandardIP(uint ip)
        {
            byte[] b_ip = new byte[4];
            b_ip[0] = (byte)(ip & 0x000000ff);
            b_ip[1] = (byte)((ip & 0x0000ff00) >> 8);
            b_ip[2] = (byte)((ip & 0x00ff0000) >> 16);
            b_ip[3] = (byte)((ip & 0xff000000) >> 24);
            return b_ip[0].ToString() + "." + b_ip[1].ToString() + "." + b_ip[2].ToString() + "." + b_ip[3].ToString();
        }
        private void ParseReceive(byte[] buffer, int size)
        {
            if (buffer == null) return;
            //fixed (byte* pbuffer = buffer)
            //{
            //    IpHeader* ip_header = (IpHeader*)pbuffer;
            //    int protocol = ip_header->ip_protocol;
            //    uint ip_srcaddr = ip_header->ip_srcaddr, ip_destaddr = ip_header->ip_destaddr, header_len = 0;
            //    string out_string = "";
            //    short src_port = 0, dst_port = 0;
            //    IPAddress tmp_ip;
            //    string from_ip = "", to_ip = "";
            //    from_ip = ip_header->ip_srcaddr.ToString();
            //    switch (protocol)
            //    {
            //        case 1: out_string = "ICMP:"; break;
            //        case 2: out_string = "IGMP:"; break;
            //        case 6:
            //            out_string = "TCP:";
            //            break;
            //        case 17: out_string = "UDP:";
            //            break;
            //        default: out_string = "UNKNOWN"; break;
            //    }
            //    //                System.Console.WriteLine(out_string + "from ip:" + IPAddress.Parse(from_ip).ToString() + " to ip:" + IPAddress.Parse(to_ip).ToString());
            //    System.Console.WriteLine(out_string + "src:" + StandardIP(ip_srcaddr) + "dest:" + StandardIP(ip_destaddr));
            //}

        }
        public void ShutDown()
        {
            if (socket != null)
            {
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (Exception)
                {
                    System.Console.WriteLine("关闭socket错误!");
                }

            }
        }
        /// <summary>
        /// 如何让该功能监听该计算机上全部的socket请求
        /// </summary>
        public void Run()
        {
            System.Console.WriteLine("Raw Socket running...");
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
            byte[] buffer = new byte[4096];
            int rcv_size = 0;
            // socket.Blocking = false;
            socket.Bind(new IPEndPoint(IPAddress.Parse(IP), 0));
            while (true)
            {
                System.Console.WriteLine("开始新一次循环");
                try
                {
                    //socket.BeginReceive(buffer, 0, 10, SocketFlags.None, Callback, null);
                    rcv_size = socket.Receive(buffer);
                    ParseReceive(buffer, rcv_size);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("异常：" + e.Message);
                    return;
                }
                //System.Threading.Thread.Sleep(500);
                //System.Console.WriteLine("接收到:" + rcv_size.ToString());
            }
        }
        ~RawSocket()
        {
            ShutDown();
        }
    }



    /// <summary>
    /// 后台程序
    /// </summary>
    public class BackstageCommandFactory:PureMvcExt.Factory.CommandFactory
    {
        public BackstageCommandFactory():base()
        {

        }
        public override void Execute(INotification notification)
        {
            switch (notification.Type)
            {
                case AppNotify.Get_UinTotal:
                    Task.Factory.StartNew(() =>
                    {
                        Domain.CommonData.PickUpStatic pc = (new QQDataDA()).TodayStatic();
                        SendNotification(AppNotify.Back_UinTotal, pc);
                    });
                    break;
                case AppNotify.Get_CityData:
                    Task.Factory.StartNew(() =>
                    {
                        List<CategoryData> list = new DataFromManage().QueryCities();
                        SendNotification(AppNotify.Back_CityData, list);
                    });
                    break;
            }
            //base.Execute(notification);
        }
        public override void SendNotification(string notificationName, object body = null, string type = null)
        {
            base.SendNotification(notificationName, body, type);
        }
 
    }
    public class BackstageFacade : PureMvcExt.Factory.FacadeFactory
    {
        public BackstageFacade():base("BackstageFacade")
        {
            
        }

        #region supply register
        public override void RegisterCommand(string notificationName, Func<ICommand> commandFunc)
        {
            base.RegisterCommand(notificationName, commandFunc);
        }
        public override void RegisterMediator(IMediator mediator)
        {
            base.RegisterMediator(mediator);
        }
        public override void RegisterProxy(IProxy proxy)
        {
            base.RegisterProxy(proxy);
        }
        #endregion
        #region self register
        protected override void InitializeController()
        {//Command
            base.InitializeController();
            FacadeInstance.RegisterCommand(AppNotify.Name_Backstage, ()=>new BackstageCommandFactory());
        }
        protected override void InitializeModel()
        {//Proxy
            base.InitializeModel();
        }
        #endregion
    }

    public class AppNotify
    {
        public const string Name_Backstage = "Name_Backstage";
        public const string Get_UinTotal = "Get_UinTotal";
        public const string Back_UinTotal = "Back_UinTotal";
        public const string Get_CityData = "Get_CityData";
        public const string Back_CityData = "Back_CityData";
    }
}
