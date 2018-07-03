using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonData;
namespace HRApp.Model
{
    public class AppEmail : GuidTimeField
    {
        public string Body { get; set; }
        public short BodyType { get; set; }//正文数据库存储形式 1 内容 2 文本路径（内容超过长度时不直接进行数据库存储，写入到文本中）
        public bool IsDelete { get; set; }
        [DescriptionSort("邮件标题")]
        public string Subject { get; set; }
        [DescriptionSort("补充消息")]//比如进行消息回复时候关联到上级消息
        public Guid ParentId { get; set; }
        [DescriptionSort("邮件发送人")]
        public string SendBy { get; set; }
        public string GetInsertSql() 
        {
            return @"INSERT INTO  [dbo].[AppEmail]   ([ID],[Body],[BodyType],[Subject],[IsDelete],[ParentId],[SendBy],[CreateTime])
VALUES  ({ID},{Body},{BodyType},{Subject},{IsDelete},{ParentId},{SendBy},{CreateTime})";
        }
    }
    [DescriptionSort("日志定时发送计划")]
    public class AppEmailPlan : GuidTimeFieldwithDelete
    {
        public Guid PrimaryMsgId { get; set; }
        /// <summary>
        /// [发送失败]重新主要用于记录重新发送次数
        /// </summary>
        public short SendNumber { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }
        public string GetInsertSql()
        {
            return @"INSERT INTO  [dbo].[AppEmailPlan] ([Id],[PrimaryMsgId],[SendTime],[CreateTime],[IsDelete],[SendNumber])
VALUES   ({Id},{PrimaryMsgId},{SendTime},{CreateTime},{IsDelete},{SendNumber}) ";
        }
    }
    [DescriptionSort("邮件接收人计划列表")]
    public class AppEmailReceiverPlan : GuidTimeFieldwithDelete
    {
        public Guid PrimaryMsgId { get; set; }
        [DescriptionSort("是否为抄送人")]
        public bool IsMailer { get; set; }
        [DescriptionSort("接收人")]
        public string SendTo { get; set; }
        public string GetInsertSql()
        {
            return @"INSERT INTO  [dbo].[AppEmailReceiverPlan] ([Id],[PrimaryMsgId],[IsMailer],[SendTo],[CreateTime],[IsDelete])
     VALUES ({Id},{PrimaryMsgId},{IsMailer},{SendTo},{CreateTime},{IsDelete}) ";
        }
    }
    [DescriptionSort("邮件接收人接收情况")]
    public class AppEmailReceiver : GuidTimeFieldwithDelete
    {
        public Guid PrimaryMsgId { get; set; }
        public bool IsMailer { get; set; }
        public string SendTo { get; set; }
        public DateTime SendTime { get; set; }
        [DescriptionSort("邮件发送结果")]
        public bool SendResult { get; set; }
        [DescriptionSort("发送次数")]
        public short SendNumber { get; set; }
        public int DayInt { get; set; }
        public string GetInsertSql()
        {
            return @"INSERT INTO  [dbo].[AppEmailReceiver] ([Id],[PrimaryMsgId],[IsMailer],[SendTo],[SendTime],[CreateTime],[IsDelete],[DayInt])
     VALUES ({Id},{PrimaryMsgId},{IsMailer},{SendTo},{SendTime},{CreateTime},{IsDelete},{DayInt}) ";
        }
    }
    public class AppEmailData : SampleEmailData
    {
        [DescriptionSort("数据库中存储的邮件id")]
        public Guid EmailId { get; set; }
       
        public string From { get; set; }
       
        [DescriptionSort("邮件创建时间")]
        public DateTime EmailCreateTime { get; set; }
        [DescriptionSort("邮件发送时间")]
        public DateTime? SendTime { get; set; }
        [DescriptionSort("邮件发送结果")]
        public short SendResult { get; set; }
        [DescriptionSort("重发次数")]
        public short TryAgain { get; set; }
        public EnumEmailBodyType BodyType { get; set; }
        public string QueryWaitSendEmmails(int topNum) 
        {//查询待发送的账号列表：结果为多个数据集，含有邮件内容信息，接收人列表
            return @"select top {topNum} e.* from AppEmail e,AppEmailPlan p  
where e.IsDelete=0 and e.Id=p.PrimaryMsgId and p.SendTime between {BeginSendTime} and {EndSendTime}
and e.Id not exists(select count(1) from AppEmailData where e.id=EmailId)".Replace("{topNum}", topNum.ToString());
        }
    }
    public class SampleEmailData 
    {
        public string Body { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        [DescriptionSort("抄送人")]
        public List<string> Mailer { get; set; }
    }
    public class SendEmailData : SampleEmailData
    {
        public string SendTime { get; set; }
    }
    [DescriptionSort("邮件系统配置")]
    public class EmailSystemSetting
    {
        public string EmailAccount { get; set; }
        public string EmailAuthortyCode { get; set; }
        public string EmailHost { get; set; }
        public int? EmailHostPort { get; set; }
        public EnumSMTP Smtp { get; set; }
        [DescriptionSort("根据smtp类型获取端口")]
        public static int GetHostPortSmtp(short smtpType)
        {
            EnumSMTP smtp = (EnumSMTP)smtpType;
            return smtp == EnumSMTP.QQ ? 587 : 25;
        }
    }
    public class ReserveEmailAccount : EmailAccount
    {
        public DateTime CreateTime { get; set; }
        public string Remark { get; set; }
        [DescriptionSort("账户使用的优先级")]
        public short UsePriority { get; set; }
        [DescriptionSort("是否系统默认账户")]
        public bool IsPrimaryAccount { get; set; }
        [DescriptionSort("该账户是否可用")]
        public bool IsEnable { get; set; }
        [DescriptionSort("查询全部系统默认邮件发送账户")]
        public string GetSelectAllSql() 
        {
            return @"SELECT  [Id],[Account],[AuthortyCode],[Smtp],[SmtpHost],[CreateTime],[IsDelete],[Remark],[UsePriority],[IsPrimaryAccount],[IsEnable]
  FROM [HrApp].[dbo].[ReserveEmailAccount]";
        }
       
    }
    public class EmailAccount : BaseLogicWithIntPrimary
    {
        public string Account { get; set; }
        [DescriptionSort("账户授权码")]
        public string AuthortyCode { get; set; }
        public short Smtp { get; set; }
        public string SmtpHost { get; set; }
    }
}
