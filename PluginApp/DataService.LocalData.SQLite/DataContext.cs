using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace DataService.LocalData.SQLite
{
    public class DataContext:DbContext
    {
        public DataContext(string dbConnString) : base(dbConnString)
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
    /// <summary>
    /// 封装单个数据表的简单操作/外键约束表的增加[db-first]
    /// </summary>
    public class DBReporistory
    {
        static DataContext dbcontext = null;
        public DBReporistory()//string dbConnString)
        {
            if (dbcontext == null)
            {
                dbcontext = new DataContext("SQLite");//dbConnString);
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

    }
}
