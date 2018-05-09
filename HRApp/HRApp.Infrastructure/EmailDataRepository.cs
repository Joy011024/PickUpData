using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using IHRApp.Infrastructure;
namespace HRApp.Infrastructure
{
    public class EmailDataRepository:IEmailDataRepository
    {

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
            throw new NotImplementedException();
        }

        public int SaveWaitSendEmailListData(List<AppEmailData> emails)
        {
            throw new NotImplementedException();
        }

        public IList<AppEmail> QueryList(string cmd, Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public IList<AppEmail> QueryAll()
        {
            throw new NotImplementedException();
        }

        public string SqlConnString
        {
            get;
            set;
        }

        public bool Add(AppEmail entity)
        {
            throw new NotImplementedException();
        }

        public bool Edit(AppEmail entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(object key)
        {
            throw new NotImplementedException();
        }

        public bool LogicDel(object key)
        {
            throw new NotImplementedException();
        }

        public AppEmail Get(object key)
        {
            throw new NotImplementedException();
        }

        public IList<AppEmail> Query(string cmd)
        {
            throw new NotImplementedException();
        }
    }
}
