using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using IHRApp.Infrastructure;
using Domain.CommonData;
using CommonHelperEntity;
using System.Data;
using System.Data.SqlClient;
using Infrastructure.MsSqlService.SqlHelper;
namespace HRApp.Infrastructure
{
    public class AppSettingRepository : IAppSettingRepository
    {
        public IList<CategoryItems> QueryList(string cmd, Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public string SqlConnString
        {
            get;
            set;
        }

        public bool Add(CategoryItems entity)
        {
            //拼接SQL语句
            string cmd = @"insert into CategoryItems([Name],[ParentID],[ParentCode],[Code],[Sort],[IsDelete],[ItemUsingSize],[CreateTime],[NodeLevel],[ItemDesc],[ItemValue])
values({Name},{ParentId},{ParentCode},{Code},{Sort},{IsDelete},{ItemUsingSize},{CreateTime},{NodeLevel},{ItemDesc},{ItemValue})";
            //读取属性名
            Dictionary<string, object> properties = entity.GetAllPorpertiesNameAndValues();
            List<SqlParameter> ps = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> item in properties)
            {
                string paramName = "@" + item.Key;
                string field="{" + item.Key + "}";
                if (cmd.Contains(field))
                {
                    cmd = cmd.Replace(field, paramName);
                    //获取参数的数据类型
                    SqlParameter p = new SqlParameter(paramName, item.Value==null?DBNull.Value:item.Value);
                    ps.Add(p);
                }
            }
            SqlCmdHelper helper = new SqlCmdHelper() { SqlConnString = SqlConnString };
            return helper.ExcuteNoQuery(cmd, ps.ToArray()) > 0;
        }

        public bool Edit(CategoryItems entity)
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

        public CategoryItems Get(object key)
        {
            throw new NotImplementedException();
        }

        public IList<CategoryItems> Query(string cmd)
        {
            throw new NotImplementedException();
        }
        [DescriptionSort("根据元素的根节点查询数据")]
        public List<CategoryItems> GetNodeListByParent(string parentNodeCode)
        {
            List<CategoryItems> datas = new List<CategoryItems>();
            CategoryItems model = new CategoryItems();
            string sql = model.BuilderSqlParam();
            SqlParameter[] param = new SqlParameter[]
            { 
                new SqlParameter(){ ParameterName="@code",Value=parentNodeCode} 
            };
            return CommonRepository.QueryModelList<CategoryItems>(sql, param, SqlConnString, 0, int.MaxValue);
        }

    }
}
