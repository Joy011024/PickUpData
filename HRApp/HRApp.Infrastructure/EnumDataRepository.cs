using System;
using System.Data;
using System.Data.SqlClient;
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
            return false;
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


        public int AddReturnId(EnumData data)
        {
            string sql = SqlCmdHelper.GenerateInsertSql<EnumData>();
            object obj= SqlCmdHelper.ExtInsertReturnGenerateId(sql, SqlConnString, data);
            if (obj == null)
            {
                return -1;
            }
            return int.Parse(obj.ToString());//此处  (int)obj  进行强制类型转换会出现异常，因为数据库中查询出的数据没有定义数据类型
        }


        public List<EnumData> QueryEnumMember(string enumName, bool isContainerDelete)
        {
            Dictionary<string, string> columns = SqlCmdHelper.GenerateColumnMapPropertyDict<EnumData>();
            string[] col = columns.Keys.ToArray();
            EnumData ed = new EnumData() { Code = enumName };
            string sql = ed.QueryEnumMembersSqlFormat(col, isContainerDelete);
            return CommonRepository.QueryModels<EnumData,EnumData>(sql, ed, SqlConnString);
        }
    }
}
