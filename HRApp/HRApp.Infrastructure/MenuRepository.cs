using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IHRApp.Infrastructure;
using HRApp.Model;
using Infrastructure.MsSqlService.SqlHelper;
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
            Menu m = new Menu() 
            {
                IsEnable=true
            };
            string insert = SqlCmdHelper.GenerateInsertSql<Menu>();// m.InserSql();
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

        public List<Menu> QueryMenus()
        {
            Menu menu = new Menu();
            string cmd = SqlCmdHelper.GenerateSampleSelectSql<Menu>();// menu.QueryMenus();
            return CommonRepository.QueryModelList<Menu>(cmd, null, SqlConnString, 0, int.MaxValue);
        }
    }
}
