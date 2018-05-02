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
            string cmd = entity.GetInsertSql();
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
            string sql = entity.GetUpdateSql();
            SqlCmdHelper helper = new SqlCmdHelper() { SqlConnString = SqlConnString };
            return helper.GenerateNoQuerySqlAndExcute(sql, entity)>0;
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
            int id = Convert.ToInt32(key);
            CategoryItems item = new CategoryItems() { Id = id };
            string sql = item.GetFirstOneSql();
            DataSet ds = new SqlCmdHelper() { SqlConnString=SqlConnString}.GenerateQuerySqlAndExcute(sql, item);
            List<CategoryItems> data =DataHelp.DataReflection.DataSetConvert<CategoryItems>(ds);
            if (data == null||data.Count==0)
            {
                return null;
            }
            return data[0];
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
        [DescriptionSort("无条件查询全部")]
        public IList<CategoryItems> QueryAll() 
        {
            List<CategoryItems> datas = new List<CategoryItems>();
            CategoryItems model = new CategoryItems();
            string sql = model.QueryAllDataOfSql();
            return CommonRepository.QueryModelList<CategoryItems>(sql, null, SqlConnString, 0, int.MaxValue);
        }
        public int ValideExists(string key)
        {
            string sql = CategoryItems.BuilderValideSql();
            SqlParameter[] param = new SqlParameter[] 
            {
                new SqlParameter(){ParameterName="@code",Value=key}
            };
            return CommonRepository.ExecuteCount(sql, param, SqlConnString);
        }
        public bool ChangeSpell(int id, string spell)
        {
            CategoryItems item = new CategoryItems() { Id = id, IndexSpell = spell };
            string sql = item.GetChangeSpellWord();
            SqlCmdHelper helper = new SqlCmdHelper() { SqlConnString = SqlConnString };
            return helper.GenerateNoQuerySqlAndExcute(sql, item) > 0;
        }
        public int BatchChangeSpell(Dictionary<int, string> idWithSpells)
        {
            if (idWithSpells.Count == 0)
            {
                return idWithSpells.Count;
            }
            List<CategoryItems> entities = new List<CategoryItems>();
            foreach (var item in idWithSpells)
            {
                CategoryItems spell = new CategoryItems() { Id = item.Key, IndexSpell = item.Value };
                entities.Add(spell);
            }
            string sql = entities[0].GetChangeSpellWord();
            return CommonRepository.ExtBatchInsert(sql, SqlConnString, entities);
        }
        public List<CategoryItems> QueryNodesByIndex(string keySpell)
        {
            CategoryItems item = new CategoryItems() { IndexSpell="%"+keySpell+"%"};
            DataSet  ds= new SqlCmdHelper() { SqlConnString = SqlConnString }.GenerateQuerySqlAndExcute(item.GetQueryByIndexSpell(), item);
            return DataHelp.DataReflection.DataSetConvert<CategoryItems>(ds);
        }
    }
}
