using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IHRApp.Infrastructure;
using HRApp.Model;
namespace HRApp.Infrastructure
{
    public class MenuRepository:IMenuRepository
    {
        public string SqlConnString
        {
            get;
            set;
        }

        public bool Add(Model.Menu entity)
        {
            Menu m = new Menu();
            string insert = m.InserSql();
            return CommonRepository.ExtInsert<Menu>(insert, SqlConnString, entity);
        }

        public bool Edit(Model.Menu entity)
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

        public Model.Menu Get(object key)
        {
            throw new NotImplementedException();
        }

        public IList<Model.Menu> Query(string cmd)
        {
            throw new NotImplementedException();
        }
    }
}
