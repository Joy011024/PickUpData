using Infrastructure.MsSqlService.SqlHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonHelperEntity;
namespace HRApp.Infrastructure
{
    public class CommonRepository
    {
        /// <summary>
        /// 扩展拼接SQL语句函数
        /// </summary>
        /// <typeparam name="T">提供数据的实体类【实体属性和SQL参数存在关联关系 {属性名} 】</typeparam>
        /// <param name="cmd"></param>
        /// <param name="connString"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool ExtInsert<T>(string cmd, string connString, T entity) where T : class
        {
            Dictionary<string, object> properties = entity.GetAllPorpertiesNameAndValues();
            List<SqlParameter> ps = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> item in properties)
            {
                string paramName = "@" + item.Key;
                string field = "{" + item.Key + "}";
                if (cmd.Contains(field))
                {
                    cmd = cmd.Replace(field, paramName);
                    //获取参数的数据类型
                    SqlParameter p = new SqlParameter(paramName, item.Value == null ? DBNull.Value : item.Value);
                    ps.Add(p);
                }
            }
            SqlCmdHelper helper = new SqlCmdHelper() { SqlConnString = connString };
            return helper.ExcuteNoQuery(cmd, ps.ToArray()) > 0;
        }
    }
}
