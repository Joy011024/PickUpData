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
    public class OrganizationRepository : IOrganizationRepository
    {

        public IList<Organze> QueryList(string cmd, Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public IList<Organze> QueryAll()
        {
            throw new NotImplementedException();
        }

        public string SqlConnString
        {
            get;
            set;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Add(Organze entity)
        {
            string sqlFormat= SqlCmdHelper.GenerateInsertSql<Organze>();
            SqlCmdHelper help = new SqlCmdHelper() { SqlConnString=SqlConnString};
            SqlCmdHelper.SqlRuleMapResult rule=new SqlCmdHelper.SqlRuleMapResult();
            help.InsertSqlParam(sqlFormat, entity, rule);
            return help.ExcuteNoQuery(string.Join(";", rule.WaitExcuteSql), rule.SqlParams.ToArray()) > 0;
        }

        public bool Edit(Organze entity)
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

        public Organze Get(object key)
        {
            throw new NotImplementedException();
        }

        public IList<Organze> Query(string cmd)
        {
            throw new NotImplementedException();
        }
    }
}
