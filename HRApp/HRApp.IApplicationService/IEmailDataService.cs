using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRApp.IApplicationService
{
    public interface IEmailDataService
    {
        /// <summary>
        /// 每日激活SMTP
        /// </summary>
        /// <returns></returns>
        bool EveryDayActiveEmailSMTP(string emailSmtpAccount,string smtpAuthorCode,string emailHost,int? emailPort, string emailTo,string EmailFrom);
    }
}
