using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using CommonHelperEntity;
using Domain.GlobalModel;
using System.Reflection;
namespace Infrastructure.MsSqlService.SqlHelper
{
    public class SqlCmdHelper
    {
        /// <summary>
        /// 超时时长
        /// </summary>
        public int? Timeout { get; set; }
        public string SqlConnString { get; set; }
        /// <summary>
        /// 执行非查询命令时收影响的行数
        /// </summary>
        /// <param name="sqlCmd"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public int ExcuteNoQuery(string sqlCmd, params SqlParameter[] pms)
        {
            if (string.IsNullOrEmpty(SqlConnString))
            {
                return -1;
            }
            SqlConnection conn = new SqlConnection(SqlConnString);
            conn.Open();
            SqlCommand comm = new SqlCommand(sqlCmd, conn);
            if (pms != null && pms.Length > 0)
            {
                comm.Parameters.AddRange(pms);
            }
            int result = comm.ExecuteNonQuery();
            conn.Close();
            return result;
        }
        public int RunProcedureNoQuery(string proceudreCmd, params SqlParameter[] pms)
        {
            SqlConnection conn = new SqlConnection(SqlConnString);
            conn.Open();
            SqlCommand comm = new SqlCommand(proceudreCmd, conn);
            if (pms != null && pms.Length > 0)
            {
                comm.Parameters.AddRange(pms);
            }
            comm.CommandType = CommandType.StoredProcedure;
            int result = comm.ExecuteNonQuery();
            conn.Close();
            return result;
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="table">待批量添加的数据</param>
        /// <param name="destinationTableName">数据库中要批量添加数据的表名</param>
        public void BulkSave(DataTable table, string destinationTableName)
        {
            SqlConnection conn = new SqlConnection(SqlConnString);
            Stopwatch sw = new Stopwatch();
            SqlBulkCopy bulk = new SqlBulkCopy(conn);//批量添加
            bulk.DestinationTableName = destinationTableName;
            foreach (DataColumn item in table.Columns)
            {
                bulk.ColumnMappings.Add(item.ColumnName, item.ColumnName);
            }
            bulk.BatchSize = table.Rows.Count;
            conn.Open();
            sw.Start();
            bulk.WriteToServer(table);
            sw.Stop();
            conn.Close();
        }
        public DataSet QueryDataSet(string sql,string sqlconnString,SqlParameter[] param,int? beginRow,int? endRow,string dataSetName)
        {
            SqlConnection conn = new SqlConnection(sqlconnString);
            Stopwatch sw = new Stopwatch();//语句运行时间检测
            if (Timeout.HasValue)
            {
                Exception exMsg = null;
                bool connSuccess = false;
                Thread t = new Thread(delegate()
                {
                    try
                    {
                        sw.Start();
                        conn.Open(); //如果一开始数据库就没法连接则不会进入到操作超时过程
                        connSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        if (exMsg == null)
                        {
                            exMsg = ex;
                        }
                    }
                });
                t.Start();
                //如果执行成功则跳过这个步骤
                while (Timeout.Value > sw.ElapsedMilliseconds / 1000)
                { //秒级别比较
                    if (t.Join(1)) break;
                    if (connSuccess) {
                        break;
                    }
                }
                if (!connSuccess)
                { //如果没有执行成功的
                    throw exMsg;
                }
            }
            else 
            {
                conn.Open();
            }
            SqlCommand comm = new SqlCommand(sql, conn);
            if (Timeout.HasValue)
            {
                comm.CommandTimeout = Timeout.Value;//单位为秒
            }
            
            if (param != null && param.Length > 0)
            {
                comm.Parameters.AddRange(param);
            }
            SqlDataAdapter dap = new SqlDataAdapter(comm);
            DataSet ds = new DataSet();
            if (!beginRow.HasValue)
            {
                dap.Fill(ds);
            }
            else 
            {
                dap.Fill(ds, beginRow.Value, endRow.HasValue ? endRow.Value : int.MaxValue, dataSetName);
            }
            conn.Close();
            return ds;
        }
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlconnString"></param>
        /// <param name="param"></param>
        /// <returns>查询结果</returns>
        public SqlDataReader ExcuteQuery(string sql, string sqlconnString, SqlParameter[] param)
        {
            SqlConnection conn = new SqlConnection(sqlconnString);
            conn.Open();
            SqlCommand comm = new SqlCommand(sql, conn);
            if (param != null && param.Length > 0)
            {
                comm.Parameters.AddRange(param);
            }
            SqlDataReader reader= comm.ExecuteReader();
            return reader;
        }
        /// <summary>
        /// 当SQL语句中没有进行分页时，提供beginRow，endRow进行数据集填充分页限定
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="connString"></param>
        /// <param name="beginRow"></param>
        /// <param name="endRow"></param>
        /// <param name="tableName"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        [Description("执行查询的存储过程")]
        public DataSet ExcuteQuerySP(string sp,string connString,int? beginRow,int? endRow,string tableName,params SqlParameter[] ps) 
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand comm = new SqlCommand();// new SqlCommand(sp, conn);
            comm.CommandType = CommandType.StoredProcedure;//存储过程
            comm.Connection = conn;
            comm.CommandText = sp;
            conn.Open();
            if (ps != null && ps.Length > 0)
            {
                comm.Parameters.AddRange(ps);
            }
            SqlDataAdapter dap = new SqlDataAdapter(comm);
            DataSet ds = new DataSet();
            if (beginRow.HasValue)
            {
                dap.Fill(ds, beginRow.Value, (endRow.HasValue ? endRow.Value : int.MaxValue), tableName);
            }
            else {
                dap.Fill(ds);
            }
            return ds;
        }
        /// <summary>
        /// 提供待执行的SQL语句进行参数化执行SQL语句
        /// </summary>
        /// <typeparam name="T">参数实体类</typeparam>
        /// <param name="sqlCmd">sql 中参数预设值为 {paramName}</param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int GenerateNoQuerySqlAndExcute<T>(string sqlCmd, T entity) where T : class
        {
            Dictionary<string, object> properties = entity.GetAllPorpertiesNameAndValues();
            List<SqlParameter> pms = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> item in properties)
            {
                string paramName = "@" + item.Key;
                string field = "{" + item.Key + "}";
                if (sqlCmd.Contains(field))
                {
                    sqlCmd = sqlCmd.Replace(field, paramName);
                    //获取参数的数据类型
                    SqlParameter p = new SqlParameter(paramName, item.Value == null ? DBNull.Value : item.Value);
                    pms.Add(p);
                }
            }
            if (string.IsNullOrEmpty(SqlConnString))
            {
                return -1;
            }
            SqlConnection conn = new SqlConnection(SqlConnString);
            conn.Open();
            SqlCommand comm = new SqlCommand(sqlCmd, conn);
            if (pms != null && pms.Count > 0)
            {
                comm.Parameters.AddRange(pms.ToArray());
            }
            int result = comm.ExecuteNonQuery();
            conn.Close();
            return result;
        }

        public DataSet GenerateQuerySqlAndExcute<T>(string sqlCmd, T entity) where T : class
        {
            if (string.IsNullOrEmpty(SqlConnString))
            {
                return null;
            }
            Dictionary<string, object> properties = entity.GetAllPorpertiesNameAndValues();
            List<SqlParameter> pms = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> item in properties)
            {
                string paramName = "@" + item.Key;
                string field = "{" + item.Key + "}";
                if (sqlCmd.Contains(field))
                {
                    sqlCmd = sqlCmd.Replace(field, paramName);
                    //获取参数的数据类型
                    SqlParameter p = new SqlParameter(paramName, item.Value == null ? DBNull.Value : item.Value);
                    pms.Add(p);
                }
            }
            
            SqlConnection conn = new SqlConnection(SqlConnString);
            conn.Open();
            SqlCommand comm = new SqlCommand(sqlCmd, conn);
            if (pms != null && pms.Count > 0)
            {
                comm.Parameters.AddRange(pms.ToArray());
            }
            SqlDataAdapter dap = new SqlDataAdapter();
            dap.SelectCommand = comm;
            DataSet ds = new DataSet();
            dap.Fill(ds);
            conn.Close();
            return ds;
        }
        /// <summary>
        /// 根据SQL参数规则（参数名全小写=属性名全小写）进行实体属性匹配，并生成待执行的参数调节项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="containerRuleInSqlCmd">规则化的SQL语句</param>
        /// <param name="entity"></param>
        /// <param name="sqlCmdMapRegexRule">SQL参数匹配规则=默认 {参数名}</param>
        /// <returns>如果结果中属性NoMapRule长度》0则存在没有匹配成功的规则，不能成功执行SQL语句</returns>
        [Description("使用正则从字符串中获取指定的属性列表")]
        public SqlRuleMapResult PreparePropertiesFromString<T>(string containerRuleInSqlCmd, T entity, string sqlCmdMapRegexRule = "{(.*?)}")  where T:class
        {// 结果字典：key=字符串中的关键字，value=实体属性
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(sqlCmdMapRegexRule);
            System.Text.RegularExpressions.MatchCollection matchs= reg.Matches(containerRuleInSqlCmd);
            List<string> param = new List<string>();
            System.Reflection.PropertyInfo[] pis= entity.GetType().GetProperties();
            Dictionary<string, string> ruleAsProperty = new Dictionary<string, string>();
            List<string> loseMapRule = new List<string>();//语句规则中没法匹配的规则项
            List<SqlParameter> sqlParam = new List<SqlParameter>();//在规则全部符合的情形下进行执行命令的参数列表
            string waitExuet = containerRuleInSqlCmd;
            foreach (System.Text.RegularExpressions.Match item in matchs)
            {
                System.Text.RegularExpressions.GroupCollection gc = item.Groups;
                string  ruleParamName= gc[1].Value;
                string ruleParamFormat = gc[0].Value;//数组元素  表达式1 {(.*?)} =[{属性},属性] ，表达式2 ({.*?})=[{属性},{属性}]
                if (ruleAsProperty.ContainsKey(ruleParamFormat))
                {//定义了多个相对规则参数名，会在第一次出现时进行处理
                    continue;
                }
                bool noMap = true;
                System.Reflection.PropertyInfo targetProperty = null;
                foreach (System.Reflection.PropertyInfo pi in pis)
                {
                    string pn = pi.Name;
                    if (pn.ToLower() == ruleParamName.ToLower())
                    {//这是目标属性
                        ruleAsProperty.Add(ruleParamFormat, pn);
                        noMap = false;
                        targetProperty = pi;
                        break;
                    }
                }
                if (loseMapRule.Count>0|| noMap)
                {//存在规则参数没有匹配的属性
                    loseMapRule.Add(ruleParamFormat);
                }
                else
                {
                    #region 将规则映射到参数
                    string pn = "@" + ruleAsProperty[ruleParamFormat];
                    waitExuet = waitExuet.Replace(ruleParamFormat, pn);
                    object obj= targetProperty.GetValue(entity, null);
                    if (obj != null)
                    {
                        sqlParam.Add(new SqlParameter() { ParameterName = pn, Value = obj });
                    }
                    else 
                    {
                        sqlParam.Add(new SqlParameter() { ParameterName = pn, Value = DBNull.Value});
                    }
                    #endregion
                }
            }
            return new SqlRuleMapResult()
            {
                OriginSql = containerRuleInSqlCmd,
                WaitExcuteSql = new List<string>() { waitExuet },
                NoMapRule = loseMapRule,
                SqlParams = sqlParam,
                RuleMapProperty = ruleAsProperty
            };
        }
        public class SqlRuleMapResult
        {
            /// <summary>
            /// 原始sql
            /// </summary>
            public string OriginSql { get; set; }
            /// <summary>
            /// 执行规则处理后的sql
            /// </summary>
            public List<string> WaitExcuteSql = new List<string>();
            /// <summary>
            /// 规则匹配的参数列表
            /// </summary>
            public List<SqlParameter> SqlParams = new List<SqlParameter>();
            /// <summary>
            /// 没有匹配的规则集合
            /// </summary>
            public List<string> NoMapRule = new List<string>();
            /// <summary>
            /// 规则匹配的实体属性
            /// </summary>
            public Dictionary<string, string> RuleMapProperty { get; set; }
        }
        /// <summary>
        /// 根据参数规则批量准备带执行脚本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="containerRuleSqlCmd"></param>
        /// <param name="entity"></param>
        /// <param name="sqlCmdMapRegexRule"></param>
        /// <returns></returns>
        [Description("批量准备实体参数列表")]
        public SqlRuleMapResult PrepareMapObjectListFormString<T>(string containerRuleSqlCmd, List<T> entity, string sqlCmdMapRegexRule = "{(.*?)}") where T : class
        {
            //查找出匹配的属性列表
            System.Reflection.PropertyInfo[] pis= typeof(T).GetProperties();
            Regex reg = new Regex(sqlCmdMapRegexRule);
            MatchCollection mc = reg.Matches(containerRuleSqlCmd);
            Dictionary<string, string> ruleMapPorperty = new Dictionary<string, string>();
            List<string> noMapRules = new List<string>();
            #region 属性与规则匹配
            foreach (Match item in mc)
            {
                string ruleFormat = item.Groups[0].Value;//规则格式
                string ruleName = item.Groups[1].Value;//规则名
                if (ruleMapPorperty.ContainsKey(ruleFormat))
                {
                    continue;  
                }
                bool hasNoMap = true;
                foreach (System.Reflection.PropertyInfo pi in pis)
                {
                    string propertyName = pi.Name;
                    if (propertyName.ToLower() == ruleName.ToLower())
                    {
                        ruleMapPorperty.Add(ruleFormat, propertyName);
                        hasNoMap = false;
                        break;
                    }
                }
                if (hasNoMap)
                {//存在没有匹配的规则 
                    noMapRules.Add(ruleFormat);
                }
            }
            #endregion
            List<SqlParameter> param = new List<SqlParameter>();
            List<string> batchSqlArr = new List<string>();
            if (noMapRules.Count > 0)
            {
                return new SqlRuleMapResult()
                {
                    OriginSql = containerRuleSqlCmd,
                    WaitExcuteSql = batchSqlArr,
                    NoMapRule = noMapRules,
                    RuleMapProperty = ruleMapPorperty,
                    SqlParams = param
                };
            }
            for (int i = 0; i < entity.Count; i++)
            {
                T item = entity[i];
                Type t=item.GetType();
                string arrSql = containerRuleSqlCmd;
                foreach (var kv in ruleMapPorperty)
                {
                    string paramName="@"+kv.Value+i;
                    arrSql = arrSql.Replace(kv.Key, paramName);
                    object obj = t.GetProperty(kv.Value).GetValue(item,null);
                    if (obj == null)
                    {
                        param.Add(new SqlParameter() { ParameterName = paramName, Value = DBNull.Value });
                    }
                    else 
                    {
                        param.Add(new SqlParameter() { ParameterName = paramName, Value = obj });
                    }
                }
                batchSqlArr.Add(arrSql);//批量执行的SQL列表
            }
            return new SqlRuleMapResult()
            {
                OriginSql = containerRuleSqlCmd,
                WaitExcuteSql = batchSqlArr,
                NoMapRule = noMapRules,
                RuleMapProperty = ruleMapPorperty,
                SqlParams = param
            };
        }
        /// <summary>
        /// 填充SQL的参数列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="data"></param>
        /// <param name="result"></param>
        /// <param name="paramMapRule"></param>
        public void InsertSqlParam<T>(string sql, T data, SqlRuleMapResult result, string paramMapRule = "{(.*?)}") where T : class
        { 
            //提取出参数列表
            Regex reg = new Regex(paramMapRule);
            MatchCollection mc= reg.Matches(sql);
            string sqlResult = sql;
            List<SqlParameter> param = new List<SqlParameter>();
            List<string> sqlNoMapParam = new List<string>();//命令中没有匹配的参数集合
            Type obj=data.GetType();
            string objName=obj.Name;
            Dictionary<string, object> propertyValue = data.GetAllPorpertiesNameAndValues();
            Dictionary<string, string> paramMapPropertyRule = new Dictionary<string, string>();
            //当前SQL语句是第几组
            int index = result.WaitExcuteSql.Count;
            foreach (Match item in mc)
            {
                string g = item.Groups[0].Value;//参数串
                string pn = item.Groups[1].Value;//参数名剔除特殊限定
                string lowerName = pn.ToLower();
                bool map = false;
                foreach (var property in propertyValue)
                {
                    if (property.Key.ToLower() == lowerName)
                    {
                        map = true;
                        paramMapPropertyRule.Add(g, property.Key);
                        string name = "@" + objName +"_"+ index + "_" + property.Key;
                        sqlResult = sqlResult.Replace(g, name);
                        result.SqlParams.Add(new SqlParameter(name, property.Value));
                        break;
                    }
                }
                if (!map)
                {//属性没有匹配 
                    sqlNoMapParam.Add(g);
                }
            }
            result.WaitExcuteSql.Add(sqlResult);
            //本次没有匹配的参数列表
            result.NoMapRule.Add(string.Join("|", sqlNoMapParam));
        }
        /// <summary>
        /// 根据实体生成insert语句
        /// </summary>
        /// <typeparam name="T">实体类
        /// 【TableFieldAttribute可以匹配实体和表关系,
        /// PropertyIgnoreFieldAttribute修饰的属性不组装到命令中】</typeparam>
        /// <returns>返回拼装的insert语句</returns>
        public static string GenerateInsertSql<T>() where T:class
        {
            Type ty = typeof(T);
            string table = ty.Name;
            //判断是否存在特性指定表名称
            object[] att= ty.GetCustomAttributes(typeof(TableFieldAttribute), false);
            List<string> dbGenrateField = new List<string>();//数据库生成字段
            List<string> ignoreProperty=new List<string>();//忽略的属性
            if (att != null)
            {
                TableFieldAttribute mapp = att[0] as TableFieldAttribute;
                table =string.IsNullOrEmpty(mapp.TableName)?table: mapp.TableName;
                if (mapp.DbGeneratedFields != null)
                {
                    dbGenrateField.AddRange(mapp.DbGeneratedFields);
                }
                if (mapp.IgnoreProperty != null)
                {
                    ignoreProperty.AddRange(mapp.IgnoreProperty);
                }
            }
            Dictionary<string, string> columnMapProperty = new Dictionary<string, string>();
            foreach (PropertyInfo item in ty.GetProperties())
            {
                string property = item.Name;
                #region 过滤掉不组装到语句中的属性
                if (ignoreProperty.Contains(property))
                {
                    continue;
                }
                if (dbGenrateField.Contains(property))
                {
                    continue;
                }
                //该属性是否存在被忽略特性
                object[] isIgnoreProperty= item.GetCustomAttributes(typeof(PropertyIgnoreFieldAttribute), false);
                if (isIgnoreProperty != null)
                {
                    continue;
                }
                #endregion
                string field = property;
                //判断是否存在转换特性
                object[] obj= item.GetCustomAttributes(typeof(ColumnAttribute), false);
                if (obj != null)
                {
                    ColumnAttribute column = obj[0] as ColumnAttribute;
                    field = column.Name;
                }
                columnMapProperty.Add("{" + property+"}", field);
            }
            if (columnMapProperty.Count == 0)
            {
                return string.Empty;
            }
            string sql = "Insert into dbo.{table} ({columns}) values({columnsValueFormat})";
            return sql.Replace("{table}", table).Replace("{columns}", string.Join(",", columnMapProperty.Values.ToArray()))
                .Replace("{columnsValueFormat}", string.Join(",", columnMapProperty.Keys.ToArray()));
        }
    }
}
