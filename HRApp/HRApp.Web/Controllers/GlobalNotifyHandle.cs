using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PureMVC.Interfaces;
using PureMvcExt.Factory;
using HRApp.IApplicationService;
using HRApp.Model;
using System.Threading.Tasks;
using System.Text;
using Domain.CommonData;
namespace HRApp.Web.Controllers
{
    public class AppNotify
    {
        public const string GlobalExecuteEvent = "DoSomeInitEventFacadeFactory";
        public const string GlobalLicense = "DoSomethinFacadeFactory";
        public const string Get_AllEmailSMTP = "Get_AllEmailSMTP";
        public const string Back_AllEmailSMTP = "Back_AllEmailSMTP";
    }
    public class MVCMediatorControl : BaseController, IMediator, INotifier
    {
        private string mediator;
        private object commpent;
        public MVCMediatorControl() : this("", null)
        {

        }
        public MVCMediatorControl(string mediator, object commpent)
        {
            this.mediator = mediator;
            this.commpent = commpent;
        }
        private IFacade facade = FacadeFactory.FacadeInstance;
        public string MediatorName { get { return mediator; } set { mediator = value; } }

        public object ViewComponent
        {
            get { return commpent; }
            set { commpent = value; }
        }

        public void HandleNotification(INotification notification)
        {

        }

        public void InitializeNotifier(string key)
        {

        }

        public string[] ListNotificationInterests()
        {
            return new string[] { };
        }

        public void OnRegister()
        {

        }

        public void OnRemove()
        {

        }

        public void SendNotification(string notificationName, object body = null, string type = null)
        {
            facade.SendNotification(notificationName, body, type);
        }
    }
    public class GlobalNotifyHandle:PureMvcExt.Factory.FacadeFactory
    {
        public GlobalNotifyHandle() : base("GlobalNotifyHandle")
        {

        }
        public override void RegisterMediator(IMediator mediator)
        {
            base.RegisterMediator(mediator);
        }
        public override void RegisterProxy(IProxy proxy)
        {
            base.RegisterProxy(proxy);
        }
        public override void RegisterCommand(string notificationName, Func<ICommand> commandFunc)
        {
            base.RegisterCommand(notificationName, commandFunc);
        }
        protected override void InitializeController()
        {
            base.InitializeController();
            //监听消息的服务类
            FacadeInstance.RegisterCommand(AppNotify.GlobalLicense, () => new DoSomethinFacadeFactory());
            FacadeInstance.RegisterCommand(AppNotify.GlobalExecuteEvent, () => new DoSomeInitEventFacadeFactory());
        }

       
    }
    #region do  something
    public class DoSomeInitEventFacadeFactory : PureMvcExt.Factory.CommandFactory
    {
        public override void Execute(INotification notification)
        {
            switch (notification.Type)
            {
                case AppNotify.Back_AllEmailSMTP:
                    List<EmailAccount> account = notification.Body as List<EmailAccount>;
                    SendActiveEmail(account);
                    break;
            }
        }
        public override void SendNotification(string notificationName, object body = null, string type = null)
        {
            base.SendNotification(notificationName, body, type);
        }
        public void ActiveEmailSmtp()
        {
            FacadeFactory.FacadeInstance.SendNotification(AppNotify.GlobalLicense, null, AppNotify.Get_AllEmailSMTP);
            
        }
        private void SendActiveEmail(List<EmailAccount> accs)
        {
            if (accs == null || accs.Count == 0)
            {
                return;
            }
            BaseBackstageThing baseBackstageThing = new BaseBackstageThing();
            string subject = string.Format(InitAppSetting.Global.ActiveSmtpSubjectFormat, DateTime.Now.ToString(Common.Data.CommonFormat.DateIntFormat));
            baseBackstageThing.LoopSendEmali(accs, subject);
        }
    }
    public class DoSomethinFacadeFactory : PureMvcExt.Factory.CommandFactory
    {
        public override void Execute(INotification notification)
        {
            switch (notification.Type)
            {
                case AppNotify.Get_AllEmailSMTP:
                    Task.Factory.StartNew(() =>
                    {
                        IEmailDataService email = IocMvcFactoryHelper.GetInterface<IEmailDataService>();
                        List<EmailAccount> accs = email.QueryEmailAccountInDB();
                        FacadeFactory.FacadeInstance.SendNotification(AppNotify.GlobalExecuteEvent, accs, AppNotify.Back_AllEmailSMTP);
                    });
                    break;
            }
        }
        public override void SendNotification(string notificationName, object body = null, string type = null)
        {
            base.SendNotification(notificationName, body, type);
        }
    }
    #endregion

    public class BaseBackstageThing
    {
        public void LoopSendEmali(List<EmailAccount> accs,string subject)
        {//每日激活邮件账户
         //查询邮件账户列表 
            string dir = LogPrepare.GetLogPath();
            ELogType el = ELogType.EmailLog;
            string file = LogPrepare.GetLogName(el);
            IEmailDataService emailService = IocMvcFactoryHelper.GetInterface<IEmailDataService>();
            foreach (var item in accs)
            {
                string time = DateTime.Now.ToString(Common.Data.CommonFormat.DateTimeFormat);
                string title = "[HrApp Everyday active]";
                try
                {
                    //使用邮件账户进行邮件发送
                    short smtp = item.Smtp;
                    //拼接发送的邮件内容
                    EmailSystemSetting ess = new EmailSystemSetting()
                    {
                        EmailAccount = item.Account,
                        EmailAuthortyCode = item.AuthortyCode,
                        EmailHost = item.SmtpHost,
                        EmailHostPort = EmailSystemSetting.GetHostPortSmtp(smtp)
                    };
                    ess.Smtp = (EnumSMTP)smtp;
                    StringBuilder body = new StringBuilder();
                    body.AppendLine(" Guid:<br/> ");
                    for (int i = 0; i < 10; i++)
                    {
                        body.AppendLine(string.Format(" {0}<br/> ", Guid.NewGuid().ToString().ToUpper()));
                    }
                    string text = string.Format("{0} <br/>  smtp= {1}<br/> time= {2} <br/>{3}  ", title,  item.Account, time, body.ToString());
                    string receive = InitAppSetting.Global.ReceiverInEmailActive;
                    AppEmailData emailData = new AppEmailData()
                    {
                        EmailCreateTime = DateTime.Now,
                        To = string.IsNullOrEmpty(receive) ? "158055983@qq.com" : receive,
                        Subject =subject,
                        From = item.Account,
                        Body = text
                    };
                    emailService.SendEmail(ess, emailData, ess.Smtp);
                    LoggerWriter.CreateLogFile(title + "[Success]" + time, dir, el, file, true);
                }
                catch (Exception ex)
                {
                    title += ex.Message;
                    LoggerWriter.CreateLogFile(title + "[Error]" + time, dir, el, file, true);
                }
            }
        }

    }
}