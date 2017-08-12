using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using TimeIntervalListen;
namespace Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public partial class Win32Native
        {
            [StructLayout(LayoutKind.Sequential)]
            public class MIB_IF_ROW2
            {
                public long InterfaceLuid;
                public int InterfaceIndex;
                public Guid GUID;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 514)]
                private byte[] bAlias;
                public string Alias
                {
                    get
                    {
                        return Encoding.Unicode.GetString(this.bAlias);
                    }
                }
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 514)]
                private byte[] bDescription;
                public string Description
                {
                    get
                    {
                        return Encoding.Unicode.GetString(this.bDescription);
                    }
                }
                public int PhysicalAddressLength;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
                public byte[] PhysicalAddress;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
                public byte[] PermanentPhysicalAddress;
                public int Mtu;
                public int Type;
                public int TunnelType;
                public int MediaType;
                public int PhysicalMediumType;
                public int AccessType;
                public int DirectionType;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
                public byte[] InterfaceAndOperStatusFlags;
                public int OperStatus;
                public int AdminStatus;
                public int MediaConnectState;
                public Guid NetworkGuid;
                public int ConnectionType;
                public long TransmitLinkSpeed;
                public long ReceiveLinkSpeed;
                public long InOctets;
                public long InUcastPkts;
                public long InNUcastPkts;
                public long InDiscards;
                public long InErrors;
                public long InUnknownProtos;
                public long InUcastOctets;
                public long InMulticastOctets;
                public long InBroadcastOctets;
                public long OutOctets;
                public long OutUcastPkts;
                public long OutNUcastPkts;
                public long OutDiscards;
                public long OutErrors;
                public long OutUcastOctets;
                public long OutMulticastOctets;
                public long OutBroadcastOctets;
                public long OutQLen;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct MIB_IF_TABLE2
            {
                public int NumEntries;
                public MIB_IF_ROW2[] Table;
            }

            [DllImport("Iphlpapi.dll", SetLastError = true)]
            public static unsafe extern int GetIfTable2(ref int Table);

            [DllImport("Kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static unsafe extern bool RtlMoveMemory(ref int Destination, int Source, int Length);

            public const int NULL = 0;
            public const int NO_ERROR = 0;
        }
        public static unsafe Win32Native.MIB_IF_TABLE2 GetMIB2Interface()
        {
            int ptr_Mit2 = Win32Native.NULL;
            Win32Native.MIB_IF_TABLE2 s_Mit2 = new Win32Native.MIB_IF_TABLE2(); // create empty mit2.
            if (Win32Native.GetIfTable2(ref ptr_Mit2) != Win32Native.NO_ERROR)
                return s_Mit2;
            if (Win32Native.RtlMoveMemory(ref s_Mit2.NumEntries, ptr_Mit2, 4) && s_Mit2.NumEntries > 0)
            {
                ptr_Mit2 = ptr_Mit2 + 8; // ptr_Mit2 += 8; offset pointer to MIB_IF_TABLE2::Table
                int dwBufferSize = 1352; // sizeof(Win32Native.MIB_IF_ROW2);
                s_Mit2.Table = new Win32Native.MIB_IF_ROW2[s_Mit2.NumEntries];
                for (int i = 0; i < s_Mit2.NumEntries; i++)
                {
                    IntPtr ptr = (IntPtr)(ptr_Mit2 + (i * dwBufferSize));
                    s_Mit2.Table[i] = (Win32Native.MIB_IF_ROW2)
                        Marshal.PtrToStructure(ptr, typeof(Win32Native.MIB_IF_ROW2));
                }
            }
            return s_Mit2;
        }

        private unsafe void Form1_Load(object sender, EventArgs e)
        {
            IntervalListen();
        }
        private void IntervalListen() 
        {
            string group = "ListenNIC";
            QuartzJob job = new QuartzJob(group);
            BaseDelegate bd=new BaseDelegate(DoInterval);
            //job.CreateJobWithParam(,)
            int interval=30;
            int repeact=24*60*60/interval;
            job.CreateJobWithParam<JobDelegateFunction>(new object[] { bd }, DateTime.Now, interval, repeact);
        }
        private void DoInterval(object obj) 
        {
            if (lstNic.InvokeRequired)
            {
                BaseDelegate bd = new BaseDelegate(DoInterval);
                this.Invoke(bd, obj);
                return;
            }
            else 
            {
                lstNic.Items.Clear();
                Win32Native.MIB_IF_TABLE2 s_Mit2 = GetMIB2Interface();
                for (int i = 0; i < s_Mit2.NumEntries; i++)
                {
                    lstNic.Items.Add(
                        new ListViewItem(
                            new string[]
                            {
                                s_Mit2.Table[i].Description,
                                s_Mit2.Table[i].Alias,
                                s_Mit2.Table[i].InOctets.ToString(),
                                s_Mit2.Table[i].OutOctets.ToString()
                            }
                            )
                        );
                }
            }
        }
    }
}
