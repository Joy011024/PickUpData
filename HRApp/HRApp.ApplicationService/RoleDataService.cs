using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.IApplicationService;
namespace HRApp.ApplicationService
{
    public class RoleDataService : IRoleDataService
    {
        public bool Add<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public bool Edit(string sql, Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public bool Delete(object key)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(object key) where T : class
        {
            throw new NotImplementedException();
        }

        public List<T> Query<T>(string cmd, Dictionary<string, object> param) where T : class
        {
            throw new NotImplementedException();
        }

        public bool LogicDelete(object key)
        {
            throw new NotImplementedException();
        }

        public string SqlConnString
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
