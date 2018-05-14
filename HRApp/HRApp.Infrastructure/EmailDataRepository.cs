using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using IHRApp.Infrastructure;
using Infrastructure.MsSqlService.SqlHelper;
using System.ComponentModel;
namespace HRApp.Infrastructure
{
    public class EmailDataRepository:IEmailDataRepository
    {

        public string SqlConnString
        {
            get;
            set;
        }

        public void QueryWaitSendEmailData(Guid id)
        {
            throw new NotImplementedException();
        }

        public void QueryWaitSendEmailListDetail(int dayInt)
        {
            throw new NotImplementedException();
        }

        public bool SaveWaitSendEmailData(AppEmailData email)
        {
            DateTime now = DateTime.Now;
            SqlCmdHelper.SqlRuleMapResult mapSql = new SqlCmdHelper.SqlRuleMapResult();
            //数据存储到数据表
            AppEmail ae = new AppEmail()
            {
                Body = email.Body,
                Subject = email.Subject,
                CreateTime = now,
                IsDelete = false,
                ParentId = new Guid(),
                SendBy = email.From,
                BodyType = (short)email.BodyType.GetHashCode()
            };
            if (email.EmailId.Equals(new Guid()))
            {
                ae.Id = Guid.NewGuid();
            }
            else 
            {
                ae.Id = email.EmailId;
            }
            SqlCmdHelper help = new SqlCmdHelper() { SqlConnString = SqlConnString };
            help.InsertSqlParam(ae.GetInsertSql(), ae, mapSql);
            if (!email.SendTime.HasValue)
            {//这条邮件数据是此刻进行发送
                email.SendTime = DateTime.Now;
            }
            //进行定时计划存储
            AppEmailPlan plan = new AppEmailPlan()
            {
                CreateTime = now,
                Id = Guid.NewGuid(),
                PrimaryMsgId = ae.Id,
                SendNumber = 0,
                SendTime = email.SendTime.Value
            };
            help.InsertSqlParam(plan.GetInsertSql(), plan, mapSql);
            AppEmailReceiverPlan emailTo = new AppEmailReceiverPlan()
            {
                CreateTime =now,
                Id = Guid.NewGuid(),
                IsMailer = false,
                PrimaryMsgId = ae.Id,
                SendTo = email.To
            };
            help.InsertSqlParam(emailTo.GetInsertSql(), emailTo, mapSql);
            List<AppEmailReceiverPlan> emailToColl = new List<AppEmailReceiverPlan>();
            emailToColl.Add(emailTo);
            if (email.Mailer != null)
            {
                foreach (var item in email.Mailer)
                {//抄送人
                    AppEmailReceiverPlan emailers = new AppEmailReceiverPlan()
                    {
                        IsMailer = true,
                        SendTo = item,
                        PrimaryMsgId = ae.Id,
                        Id = Guid.NewGuid(),
                        CreateTime = now
                    };
                    help.InsertSqlParam(emailers.GetInsertSql(), emailers, mapSql);
                    //emailToColl.Add(emailers);
                }
            }
            if (!string.IsNullOrEmpty(string.Join("", mapSql.NoMapRule)))
            {
                //日志输出没有匹配的规则
                return false;
            }
            string sql = string.Join(";", mapSql.WaitExcuteSql);
            return help.ExcuteNoQuery(sql, mapSql.SqlParams.ToArray()) == mapSql.WaitExcuteSql.Count;
        }
        void WriteSendHistory(AppEmailData email, Guid emailId, SqlCmdHelper.SqlRuleMapResult mapSql) 
        {
            AppEmailReceiver send = new AppEmailReceiver()
            {
                PrimaryMsgId = emailId,
                CreateTime = DateTime.Now,
                IsDelete = false,
                SendResult = false,
                SendTo = email.To,
                IsMailer = false
            };
            send.SendTime = email.SendTime.HasValue ? email.SendTime.Value : DateTime.Now;
            send.DayInt = int.Parse(send.CreateTime.ToString("yyyyMMdd"));
            send.Id = Guid.NewGuid();
            SqlCmdHelper help = new SqlCmdHelper() { SqlConnString = SqlConnString };
            help.InsertSqlParam(send.GetInsertSql(), send, mapSql);
            if (email.Mailer != null)
            { //抄送人
                send.IsMailer = true;
                for (int i = 0; i < email.Mailer.Count; i++)
                {
                    send.Id = Guid.NewGuid();
                    send.SendTo = email.Mailer[i];
                    help.InsertSqlParam(send.GetInsertSql(), send, mapSql);
                }
            }
        }
        public int SaveWaitSendEmailListData(List<AppEmailData> emails)
        {
            throw new NotImplementedException();
        }

        [Description("查询email列表")]
        public List<ReserveEmailAccount> QueryEmailAccounts()
        {
            string sql = new ReserveEmailAccount().GetSelectAllSql();
            SqlCmdHelper help = new SqlCmdHelper(){SqlConnString=SqlConnString};
            return CommonRepository.QueryModelList<ReserveEmailAccount>(sql, null, SqlConnString, 0, int.MaxValue);
        }
    }
}
