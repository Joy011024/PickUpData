using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
namespace IHRApp.Infrastructure
{
    public interface IEmailDataRepository
    {
        string SqlConnString { set; get; }
        /// <summary>
        /// 查询出待发送的邮件信息
        /// </summary>
        /// <param name="id"></param>
        void QueryWaitSendEmailData(Guid id);
        /// <summary>
        /// 查询某天待发送的邮件信息
        /// </summary>
        /// <param name="dayInt"></param>
        void QueryWaitSendEmailListDetail(int dayInt);
        /// <summary>
        /// 保存待发送的邮件列表
        /// </summary>
        /// <param name="email"></param>
        bool SaveWaitSendEmailData(AppEmailData email);
        int SaveWaitSendEmailListData(List<AppEmailData> emails);
    }
}
