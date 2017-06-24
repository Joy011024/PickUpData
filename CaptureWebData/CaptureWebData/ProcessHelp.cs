using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
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
}
