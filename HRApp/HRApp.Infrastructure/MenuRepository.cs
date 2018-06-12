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

        /// <summary>
        /// 设置更改菜单类型
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool ChangeMenuType(int id, int type)
        {
            Menu menu = new Menu()
            {
                Id = id,
                MenuType = (short)type
            };
            string sql = menu.ChangeMenuTypeSql();
            SqlCmdHelper cmd = new SqlCmdHelper() { SqlConnString=SqlConnString};
            return cmd.GenerateNoQuerySqlAndExcute(sql, menu) > 0;
        }
        /// <summary>
        /// 设置是否启用菜单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="operate"></param>
        /// <returns></returns>
        public bool ChangeMenuStatue(int id, bool operate)
        {
            Menu menu = new Menu()
            {
                Id = id,
                IsEnable=operate
            };
            string sql = menu.ChangeStatueSql();
            SqlCmdHelper cmd = new SqlCmdHelper() { SqlConnString = SqlConnString };
            return cmd.GenerateNoQuerySqlAndExcute(sql, menu) > 0;
        }
    }
}
