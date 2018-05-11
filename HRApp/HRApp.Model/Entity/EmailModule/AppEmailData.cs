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
        public short SendResult { get; set; }
        public int DayInt { get; set; }
        public string GetInsertSql()
        {
            return @"INSERT INTO  [dbo].[AppEmailReceiver] ([Id],[PrimaryMsgId],[IsMailer],[SendTo],[SendTime],[CreateTime],[IsDelete],[SendResult],[DayInt])
     VALUES ({Id},{PrimaryMsgId},{IsMailer},{SendTo},{SendTime},{CreateTime},{IsDelete},{SendResult},{DayInt}) ";
        }
    }
    public class AppEmailData
    {
        [DescriptionSort("数据库中存储的邮件id")]
        public Guid EmailId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        [DescriptionSort("抄送人")]
        public List<string> Mailer = new List<string>();
        [DescriptionSort("邮件创建时间")]
        public DateTime EmailCreateTime { get; set; }
        [DescriptionSort("邮件发送时间")]
        public DateTime? SendTime { get; set; }
        [DescriptionSort("邮件发送结果")]
        public short SendResult { get; set; }
        [DescriptionSort("重发次数")]
        public short TryAgain { get; set; }
        public EnumEmailBodyType BodyType { get; set; }
    }
    [DescriptionSort("邮件系统配置")]
    public class EmailSystemSetting
    {
        public string EmailAccount { get; set; }
        public string EmailAuthortyCode { get; set; }
        public string EmailHost { get; set; }
        public int? EmailHostPort { get; set; }
    }
}
