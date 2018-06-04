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
    public class EnumDataRepository:IEnumDataRepository
    {
        public IList<EnumData> QueryList(string cmd, Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public IList<EnumData> QueryAll()
        {
            throw new NotImplementedException();
        }

        public string SqlConnString
        {
            get;
            set;
        }

        public bool Add(EnumData entity)
        {
            entity.Init();
            string sql= SqlCmdHelper.GenerateInsertSql<EnumData>();
            return CommonRepository.ExtInsert(sql, SqlConnString, entity);
        }

        public bool Edit(EnumData entity)
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

        public EnumData Get(object key)
        {
            throw new NotImplementedException();
        }

        public IList<EnumData> Query(string cmd)
        {
            throw new NotImplementedException();
        }

        public bool UpdateRemark(int id, string remark)
        {
            string sql = new EnumData().GetUpdateRemarkSql();
            return new SqlCmdHelper().GenerateNoQuerySqlAndExcute(sql, new EnumData() { Id=id,Remark=remark })>0;
        }
    }
}
