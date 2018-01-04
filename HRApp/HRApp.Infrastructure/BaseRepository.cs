using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IHRApp.Infrastructure;
using System.Data.SqlClient;
//SQL server 操作类库
namespace HRApp.Infrastructure
{
    public class BaseRepository<T>:IBaseListRepository<T> where T:class
    {

        public IList<T> QueryList(string cmd, Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public string SqlConnString
        {
            get;
            set;
        }

        public bool Add(T entity)
        {
            throw new NotImplementedException();
        }

        public bool Edit(T entity)
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

        public T Get(object key)
        {
            throw new NotImplementedException();
        }

        public IList<T> Query(string cmd)
        {
            throw new NotImplementedException();
        }
    }
}
