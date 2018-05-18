using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
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

        public List<Organze> QueryOrganzes(RequestParam param)
        {
            string sql = SqlCmdHelper.GenerateSampleSelectSql<Organze>()+" where 1=1";
            List<SqlParameter> sqlParam = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(param.QueryKey))
            {
                sql += "and  Code like @key+'%' or Name like @key+'%'";
                sqlParam.Add(new SqlParameter() { ParameterName = "@key", Value = param.QueryKey });
            }
            SqlCmdHelper help = new SqlCmdHelper() { SqlConnString = SqlConnString };
            return CommonRepository.QueryModelList<Organze>(sql, sqlParam.ToArray(), SqlConnString, param.RowBeginIndex, param.RowEndIndex);
        }
    }
}
