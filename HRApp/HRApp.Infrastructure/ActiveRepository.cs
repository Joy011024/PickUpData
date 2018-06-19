using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using IHRApp.Infrastructure;
using Infrastructure.MsSqlService.SqlHelper;
namespace HRApp.Infrastructure
{
    public class ActiveRepository:IActiveRepository
    {
        public List<bool> BatchAdd(List<ActiveData> entities)
        {
            throw new NotImplementedException();
        }

        public List<bool> BatchDelete(List<object> keys)
        {
            throw new NotImplementedException();
        }

        public List<bool> BatchLogicDelete(List<object> keys)
        {
            throw new NotImplementedException();
        }

        public IList<ActiveData> QueryList(string cmd, Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public IList<ActiveData> QueryAll()
        {
            throw new NotImplementedException();
        }

        public bool Add(ActiveData entity)
        {
            SqlCmdHelper cmd = new SqlCmdHelper() { SqlConnString=SqlConnString};
            return cmd.ExcuteInsert(entity) > 0;
        }

        public bool Edit(ActiveData entity)
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

        public ActiveData Get(object key)
        {
            throw new NotImplementedException();
        }

        public IList<ActiveData> Query(string cmd)
        {
            throw new NotImplementedException();
        }

        public string SqlConnString
        {
            get;
            set;
        }
    }
}
