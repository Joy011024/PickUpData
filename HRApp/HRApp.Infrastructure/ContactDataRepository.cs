using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using IHRApp.Infrastructure;
namespace HRApp.Infrastructure
{
    public class ContactDataRepository:IContactDataRepository
    {
        public string SqlConnString
        {
            get;
            set;
        }

        public bool Add(ContactData entity)
        {
            entity.InitData();
            string sql = entity.InsertSql();
            return CommonRepository.ExtInsert<ContactData>(sql, SqlConnString, entity);
        }

        public bool Edit(ContactData entity)
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

        public ContactData Get(object key)
        {
            throw new NotImplementedException();
        }

        public IList<ContactData> Query(string cmd)
        {
            throw new NotImplementedException();
        }
    }
}
