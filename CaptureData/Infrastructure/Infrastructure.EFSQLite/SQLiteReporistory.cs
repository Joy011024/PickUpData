using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using System.Linq.Expressions;
namespace Infrastructure.EFSQLite
{
    internal class SQLiteDataContext<T> : System.Data.Entity.DbContext where T : class
    {
        string conn;
        public SQLiteDataContext(string connString)
            : base(connString)
        {
            conn = connString;
        }
        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();//去除表名的复数形式
        }
        public System.Data.Entity.IDbSet<T> Entity { get; set; }
    }

    /// <summary>
    /// 封装单个数据表的简单操作/外键约束表的增加[db-first]
    /// </summary>
    public class DBReporistory<T> where T : class
    {
        static SQLiteDataContext<T> dbcontext = null;
        public DBReporistory(string connString)
        {
            if (dbcontext == null)
            {
                dbcontext = new SQLiteDataContext<T>(connString);
            }
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public int AddList<R>(R[] datas) where R : class
        {
            DbSet<R> ds = dbcontext.Set<R>();
            foreach (var data in datas)
            {
                ds.Add(data);
            }
            return Submit();
        }
        /// <summary>
        /// 主键删除
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Delete<R>(object obj) where R : class
        {
            DbSet<R> ds = dbcontext.Set<R>();
            R one = ds.Find(obj);
            dbcontext.Entry(one).State = EntityState.Deleted;
            return Submit();
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="ids"></param>
        public void BatchDelete<R>(object[] ids) where R : class
        {
            DbSet<R> ds = dbcontext.Set<R>();
            foreach (var item in ids)
            {
                R one = ds.Find(item);
                dbcontext.Entry(one).State = EntityState.Deleted;
            }
        }
        /// <summary>
        /// 修改[需要执行submit]
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="entity"></param>
        /// <param name="origin"></param>
        public void Update<R>(R entity, R origin) where R : class
        {
            //内存中数据没有被更新 在command中使用了不同的上下文
            /*
            dbcontext.Set<R>().Attach(entity);
            dbcontext.Entry(entity).State = EntityState.Modified;
            */
            dbcontext.Entry(origin).CurrentValues.SetValues(entity);//进行数据更新替换
        }
        /// <summary>
        /// 开放提交接口，用于进行批量修改时使用
        /// </summary>
        /// <returns></returns>
        public int Submit()
        {
            return dbcontext.SaveChanges();
        }
        /// <summary>
        /// 主键查找
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public R Get<R>(object obj) where R : class
        {
            DbSet<R> ds = dbcontext.Set<R>();//这里存在问题：当定义的上下文为单例模式，数据库和内存如何实现同步更新
            return ds.Find(obj);
        }
        /// <summary>
        /// 提供使用lambda表达式操作的查询
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <returns></returns>
        public IEnumerable<R> DoQuery<R>() where R : class
        {
            DbSet<R> ds = dbcontext.Set<R>();
            return ds.AsNoTracking().AsEnumerable();
        }
        /// <summary>
        /// 执行查询的SQL语句
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IEnumerable<R> ExecuteSQL<R>(string sql,string keyName) where R : class
        {
            object[] obj = new object[] { };
            return dbcontext.Database.SqlQuery<R>(sql, obj).Distinct(new FastPropertyComparer<R>(keyName)).ToList();
        }
    }
    public class FastPropertyComparer<T> : IEqualityComparer<T>
    {
        private Func<T, Object> getPropertyValueFunc = null;

        /// <summary>
        /// 通过propertyName 获取PropertyInfo对象
        /// </summary>
        /// <param name="propertyName"></param>
        public FastPropertyComparer(string propertyName)
        {
            PropertyInfo _PropertyInfo = typeof(T).GetProperty(propertyName,
            BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
            if (_PropertyInfo == null)
            {
                throw new ArgumentException(string.Format("{0} is not a property of type {1}.",
                    propertyName, typeof(T)));
            }

            ParameterExpression expPara = Expression.Parameter(typeof(T), "obj");
            MemberExpression me = Expression.Property(expPara, _PropertyInfo); 
            //Type pt = _PropertyInfo.GetType();
            getPropertyValueFunc = Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.Property(expPara, _PropertyInfo), typeof(object)),expPara).Compile();//这里传递的属性会导致异常错误
        }

        #region IEqualityComparer<T> Members

        public bool Equals(T x, T y)
        {
            object xValue = getPropertyValueFunc(x);
            object yValue = getPropertyValueFunc(y);

            if (xValue == null)
                return yValue == null;

            return xValue.Equals(yValue);
        }

        public int GetHashCode(T obj)
        {
            object propertyValue = getPropertyValueFunc(obj);

            if (propertyValue == null)
                return 0;
            else
                return propertyValue.GetHashCode();
        }

        #endregion
    }
}
