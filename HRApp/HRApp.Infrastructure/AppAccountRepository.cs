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
    public class AppAccountRepository:IAppAccountRepository
    {
        public bool SaveSignInInfo(UserAccount user)
        {//保存数据
            if (string.IsNullOrEmpty(user.Encry))
            {
                user.Encry = "-1";
            }
            user.Id = Guid.NewGuid();
            user.CreateTime = DateTime.Now;
            SqlCmdHelper help = new SqlCmdHelper() { SqlConnString=SqlConnString};
            string sql= SqlCmdHelper.GenerateInsertSql<UserAccount>();
            return help.GenerateNoQuerySqlAndExcute(sql, user) > 0;
        }

        public string SqlConnString
        {
            get;
            set;
        }

        /// <summary>
        /// 不含有密码
        /// </summary>
        /// <param name="param"></param>
        /// <param name="canNoWhereQuery">当使用无条件查找时忽略查找条件中非行位置条件</param>
        /// <returns></returns>
        public List<UserAccount> QueryUses(RequestParam param, bool canNoWhereQuery)
        {
            string select= SqlCmdHelper.GenerateSampleSelectSql<UserAccount>();
            bool valid = false;
            List<string> where = new List<string>();
            #region 用户名只允许右匹配
            if (!string.IsNullOrEmpty(param.QueryKey))
            {//不允许前置匹配
                string[] arr = param.QueryKey.Split('%');
                if (!string.IsNullOrEmpty(arr[0]))
                {
                    where.Add("UserName like   {QueryKey} ");
                }
                else 
                {
                    valid = true;
                }
            }
            if (valid)
            {
                return null;
            }
            #endregion
            #region 创建时间检索
            if (!string.IsNullOrEmpty(param.BeginTime)&&!string.IsNullOrEmpty(param.EndTime))
            {
                where.Add("between CreateTime {BeginTime} and {EndTime}");
            }
            else if (!string.IsNullOrEmpty(param.BeginTime))
            {
                where.Add("CreateTime>=  {BeginTime} ");
            }
            else if (!string.IsNullOrEmpty(param.EndTime))
            {
                where.Add("CreateTime<=  {EndTime} ");
            }
            #endregion
            #region 昵称
            if (!string.IsNullOrEmpty(param.Name))
            {
                where.Add("Nick like {Name}");
            }
            #endregion
            if (!canNoWhereQuery && where.Count == 0)
            {//没有提供检索条件
                return null;
            }
            else 
            {
                where = new List<string>();
                //限制查询数目
                if (param.RowEndIndex - param.RowBeginIndex > 30)
                {
                    param.RowEndIndex = param.RowBeginIndex + 30;
                }

            }

            string sql=select+" where "+string.Join(" and ",where);
            SqlCmdHelper cmd = new SqlCmdHelper();
            return DataHelp.DataReflection.DataSetConvert<UserAccount>(cmd.GenerateQuerySqlAndExcute(sql, param));
        }
    }
}
