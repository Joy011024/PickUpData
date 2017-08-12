using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Demo
{
    [StructLayout(LayoutKind.Explicit)]
    public struct IPHeader
    {
        [FieldOffset(0)]
        public byte ip_verlen;//I4位首部长度+4位IP版本号
        [FieldOffset(1)]
        public byte ip_tos;//8位服务类型TOS
        [FieldOffset(2)]
        public ushort ip_totallength;//16位数据包总长度（字节）
        [FieldOffset(4)]
        public ushort ip_id;//16位标识
        [FieldOffset(6)]
        public ushort ip_offset;//3位标志位
        [FieldOffset(8)]
        public byte ip_ttl;//8位生存时间TTL
        [FieldOffset(9)]
        public byte ip_protocol;//8位协议(TCP, UDP, ICMP, Etc.)
        [FieldOffset(10)]
        public ushort ip_checksum;//16位IP首部校验和
        [FieldOffset(12)]
        public uint ip_srcaddr;//32位源IP地址
        [FieldOffset(16)]
        public uint ip_destaddr;//32位目的IP地址
    }
    public class RawSocket
    {
        private bool error_occurred;//是否产生错误   
        public bool KeepRunning;//是否继续进行   
        private static int len_receive_buf;//得到的数据流的长度   
        byte[] receive_buf_bytes;//收到的字节   
        private Socket socket = null;   //声明套接字   
        const int SIO_R = unchecked((int)0x98000001);//接收所有的数据包
        const int SIO_1 = unchecked((int)0x98000002);
        const int SIO_2 = unchecked((int)0x98000003);
        private static string localip;
        public RawSocket()//构造函数   
        {
            error_occurred = false;
            len_receive_buf = 4096;
            receive_buf_bytes = new byte[len_receive_buf];
        }

        public void CreateAndBindSocket(string IP)//建立并绑定套接字   
        {
            localip = IP;
            if (localip != "")
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
                socket.Blocking = false;//置socket非阻塞状态   
                socket.Bind(new IPEndPoint(IPAddress.Parse(IP), 0));
                if (SetSocketOption() == false) error_occurred = true;
            }
        }

        public void Shutdown()
        {
            if (socket != null)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }

        private bool SetSocketOption()  //设置raw socket 
        {
            bool ret_value = true;
            try
            {
                socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, 1);
                byte[] IN = new byte[4] { 1, 0, 0, 0 };
                byte[] OUT = new byte[4];
                int ret_code = socket.IOControl(SIO_R, IN, OUT);//低级别操作模式   
                ret_code = OUT[0] + OUT[1] + OUT[2] + OUT[3];//把4个8位字节合成一个32位整数   
                System.Windows.Forms.MessageBox.Show(ret_code.ToString());
                if (ret_code != 0) ret_value = false;
            }
            catch (SocketException)
            {
                ret_value = false;
            }
            return ret_value;
        }
        public bool ErrorOccurred
        {
            get
            {
                return error_occurred;
            }
        }
        //解析接收的数据包，形成PacketArrivedEventArgs时间数据类对象，并引发PacketArrival事件   
        unsafe private void Receive(byte[] buf, int len)
        {
            byte temp_protocol = 0;
            uint temp_version = 0;
            uint temp_ip_srcaddr = 0;
            uint temp_ip_destaddr = 0;
            short temp_srcport = 0;
            short temp_dstport = 0;
            ushort temp_id = 0;
            IPAddress temp_ip;

            //return;  

            PacketArrivedEventArgs e = new PacketArrivedEventArgs();//新网络数据包信息事件

            fixed (byte* fixed_buf = buf)
            {
                IPHeader* head = (IPHeader*)fixed_buf;
                e.HeaderLength = (uint)(head->ip_verlen & 0x0F) << 2;
                temp_id = head->ip_id;
                e.IpId = temp_id.ToString();
                temp_protocol = head->ip_protocol;
                switch (temp_protocol)
                {
                    case 1: e.Protocol = "ICMP:"; break;
                    case 2: e.Protocol = "IGMP:"; break;
                    case 6: e.Protocol = "TCP:"; break;
                    case 17: e.Protocol = "UDP:"; break;
                    default: e.Protocol = "UNKNOWN"; break;
                }
                temp_version = (uint)(head->ip_verlen & 0xF0) >> 4;
                e.IPVersion = temp_version.ToString();
                temp_ip_srcaddr = head->ip_srcaddr;
                temp_ip_destaddr = head->ip_destaddr;
                temp_ip = new IPAddress(temp_ip_srcaddr);
                e.OriginationAddress = temp_ip.ToString();
                temp_ip = new IPAddress(temp_ip_destaddr);
                e.DestinationAddress = temp_ip.ToString();
                temp_srcport = *(short*)&fixed_buf[e.HeaderLength];
                temp_dstport = *(short*)&fixed_buf[e.HeaderLength + 2];
                e.OriginationPort = IPAddress.NetworkToHostOrder(temp_srcport).ToString();
                e.DestinationPort = IPAddress.NetworkToHostOrder(temp_dstport).ToString();
                //   if(e.DestinationAddress!="211.87.212.116"||int.Parse(e.DestinationPort)>1000)   
                //   {   
                //     return;   
                //   }   
                e.PacketLength = (uint)len;
                e.MessageLength = (uint)len - e.HeaderLength;
                e.MessageBuffer = new byte[e.MessageLength];
                e.MessageData = new byte[e.MessageLength - 8];
                e.ReceiveBuffer = buf;
                //把buf中的IP头赋给PacketArrivedEventArgs中的IPHeaderBuffer   
                Array.Copy(buf, 0, e.IPHeaderBuffer, 0, (int)e.HeaderLength);
                //把buf中的包中内容赋给PacketArrivedEventArgs中的MessageBuffer   
                Array.Copy(buf, (int)e.HeaderLength, e.MessageBuffer, 0, (int)e.MessageLength);
                //Array.Copy(e.MessageBuffer, 8, e.MessageData, 0, e.MessageData.Length);
            }
            //引发PacketArrival事件   
            OnPacketArrival(e);
        }

        //开始监听
        public void Run()
        {
            IAsyncResult ar = socket.BeginReceive(receive_buf_bytes, 0, len_receive_buf, SocketFlags.None, new AsyncCallback(CallReceive), this);
        }

        //异步回调
        private void CallReceive(IAsyncResult ar)
        {
            int received_bytes;
            received_bytes = socket.EndReceive(ar);
            Receive(receive_buf_bytes, received_bytes);
            if (KeepRunning) Run();
        }
        public class PacketArrivedEventArgs : EventArgs
        {
            /*******************************************
            *   ReceiveBuffer 所有的包
            *   MessageBuffer 去除IPHeader后的包
            *   MessageData   去除UserHeader后的包
            *   UserHeader    s_port d_port length checksum
            *******************************************/
            public PacketArrivedEventArgs()
            {
                this.protocol = "";
                this.destination_port = "";
                this.origination_port = "";
                this.destination_address = "";
                this.origination_address = "";
                this.ip_version = "";
                this.ip_id = "";
                this.total_packet_length = 0;
                this.message_length = 0;
                this.header_length = 0;
                this.receive_buf_bytes = new byte[len_receive_buf];
                this.ip_header_bytes = new byte[len_receive_buf];
                this.message_bytes = new byte[len_receive_buf];
            }

            public bool IsSend
            {
                get { return OriginationAddress.Equals(localip); }
            }

            public string Protocol
            {
                get { return protocol; }
                set { protocol = value; }
            }
            public string DestinationPort
            {
                get { return destination_port; }
                set { destination_port = value; }
            }
            public string OriginationPort
            {
                get { return origination_port; }
                set { origination_port = value; }
            }
            public string DestinationAddress
            {
                get { return destination_address; }
                set { destination_address = value; }
            }
            public string OriginationAddress
            {
                get { return origination_address; }
                set { origination_address = value; }
            }
            public string IPVersion
            {
                get { return ip_version; }
                set { ip_version = value; }
            }
            public string IpId
            {
                get { return ip_id; }
                set { ip_id = value; }
            }
            public uint PacketLength
            {
                get { return total_packet_length; }
                set { total_packet_length = value; }
            }
            public uint MessageLength
            {
                get { return message_length; }
                set { message_length = value; }
            }
            public uint HeaderLength
            {
                get { return header_length; }
                set { header_length = value; }
            }
            public byte[] ReceiveBuffer
            {
                get { return receive_buf_bytes; }
                set { receive_buf_bytes = value; }
            }
            public byte[] IPHeaderBuffer
            {
                get { return ip_header_bytes; }
                set { ip_header_bytes = value; }
            }
            public byte[] MessageBuffer
            {
                get { return message_bytes; }
                set { message_bytes = value; }
            }
            /// <summary>
            /// 数据包正文
            /// </summary>
            public byte[] MessageData
            {
                get { return message_datas; }
                set { message_datas = value; }
            }
            private string protocol;
            private string destination_port;
            private string origination_port;
            private string destination_address;
            private string origination_address;
            private string ip_version;
            private string ip_id;
            private uint total_packet_length;
            private uint message_length;
            private uint header_length;
            private byte[] receive_buf_bytes = null;
            private byte[] ip_header_bytes = null;
            private byte[] message_bytes = null;
            private byte[] message_datas = null;
        }

        public delegate void PacketArrivedEventHandler(Object sender, PacketArrivedEventArgs args);

        public event PacketArrivedEventHandler PacketArrival;

        protected virtual void OnPacketArrival(PacketArrivedEventArgs e)
        {
            if (PacketArrival != null)
            {
                PacketArrival(this, e);
            }
        }
    }
}
