using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaptureManage.AppWin.TicketData.Model.Request
{
    public class LoginParam12306
    {
       
    }
    public class VerifyCode
    {
        public string answer { get; set; }
        public string login_site { get; set; }
        public string rand { get; set; }
    }
    public enum ECfgItem
    {
        appSettings=1,
        dict=2
    }
}
