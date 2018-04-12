using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;
using System.IO;
using System.Security.AccessControl;

namespace DataHelp
{
    public static class RunCoreInWindow
    {
        [DllImport("kernel32.dll")]
        static extern uint WTSGetActiveConsoleSessionId();
        [DllImport("wtsapi32.dll", CharSet = CharSet.Unicode, SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
        static extern bool WTSQuerySessionInformation(System.IntPtr hServer, int sessionId, WTSInfoClass wtsInfoClass, out System.IntPtr ppBuffer, out uint pBytesReturned);

        enum WTSInfoClass
        {
            InitialProgram,
            ApplicationName,
            WorkingDirectory,
            OEMId,
            SessionId,
            UserName,
            WinStationName,
            DomainName,
            ConnectState,
            ClientBuildNumber,
            ClientName,
            ClientDirectory,
            ClientProductId,
            ClientHardwareId,
            ClientAddress,
            ClientDisplay,
            ClientProtocolType
        }
        /// <summary>
        /// 返回值形式  DomainName\\UserName ，如果数据获取异常则返回为空
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetCurrentActiveUser(this object t)
        {
            IntPtr hServer = IntPtr.Zero, state = IntPtr.Zero;
            uint bCount = 0;

            // obtain the currently active session id; every logged on user in the system has a unique session id
            uint dwSessionId = WTSGetActiveConsoleSessionId();
            string domain = string.Empty, userName = string.Empty;

            if (WTSQuerySessionInformation(hServer, (int)dwSessionId, WTSInfoClass.DomainName, out state, out bCount))
            {
                domain = Marshal.PtrToStringAuto(state);
            }

            if (WTSQuerySessionInformation(hServer, (int)dwSessionId, WTSInfoClass.UserName, out state, out bCount))
            {
                userName = Marshal.PtrToStringAuto(state);
            }
            if (string.IsNullOrEmpty(domain) || string.IsNullOrEmpty(userName)) 
            {
                return string.Empty;
            }
            return string.Format("{0}\\{1}", domain, userName);
        }
        public static void HavaWriteAuth(this DirectoryInfo di, string windowsUserName) 
        {//判断用户是否对提供的路径存在新增权限
            DirectorySecurity ds=di.GetAccessControl();
            InheritanceFlags iflags=new InheritanceFlags();
            iflags=InheritanceFlags.ContainerInherit|InheritanceFlags.ObjectInherit;
            FileSystemAccessRule ac = new FileSystemAccessRule(windowsUserName, FileSystemRights.Write, iflags, PropagationFlags.None, AccessControlType.Allow);
            DirectorySecurity sec = Directory.GetAccessControl(di.FullName, AccessControlSections.Access);
            AuthorizationRuleCollection auths= sec.GetAccessRules(true, true, typeof(System.Security.Principal.IdentityReference));

        }
    }
}
