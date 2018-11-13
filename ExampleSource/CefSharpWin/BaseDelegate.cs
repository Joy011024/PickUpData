using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data.Entity;
using Domain.CommonData;
using Newtonsoft.Json;
namespace CefSharpWin
{
    [Description("回调事件")]
    public delegate void CallTodo(object obj);
    [Description("获取到cookie之后执行的事件")]
    public delegate void  GetCookieTodo(object obj);

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
        public void BatchAddList<R>(List<R> coll,int batchNum2DB) where R:class
        {
            int batch = coll.Count / batchNum2DB + (coll.Count % batchNum2DB > 0 ? 1 : 0);
            int cur = 0;
            List<R> once = new List<R>();
            DbSet<R> ds = dbcontext.Set<R>();
            for (int i = 0; i < coll.Count; i++)
            {
                if (cur == batchNum2DB)
                {
                    dbcontext.SaveChanges();
                    cur = 0;
                }
                ds.Add(coll[i]);
                cur++;

            }
            dbcontext.SaveChanges();
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

    public class CategoryData// : NodeData
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)] //防止序列化时需要引用ef
        //[Key] //新增为了兼容sqlite
        public int Id { get; set; }

        public int? ParentId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string ItemType { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? CreateTime { get; set; }
        public int Sort { get; set; }
        public string ParentCode { get; set; }
        public int NodeLevel { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class InitSQLiteManage
    {
        public static void SyncCityData2SQLite()
        {
            try
            {
                DBReporistory<CategoryData> qt = new DBReporistory<CategoryData>("TecentDASQLite");
                List<CategoryData> list = qt.DoQuery<CategoryData>().ToList();
                string json = FileHelper.ReadFile("City.txt");
                List<CategoryData> datas = new List<CategoryData>();
                if (string.IsNullOrEmpty(json))
                {
                    ApplicationService.IPDataService.CategoryDataService css = new ApplicationService.IPDataService.CategoryDataService("TecentDA");
                    List<Domain.CommonData.CategoryData> data = css.QueryCityCategory().ToList();
                    json = JsonConvert.SerializeObject(data);
                }
                datas = JsonConvert.DeserializeObject<List<CategoryData>>(json);
                //数据入库
               
                qt.BatchAddList(datas, 100);
            }
            catch (Exception ex)
            {
                /*增加SQL server 连接串出现异常：
                 No Entity Framework provider found for the ADO.NET provider with invariant name 'System.Data.SqlClient'. Make sure the provider is registered in the 'entityFramework' section of the application config file. See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.
                 */
                /*
                删除节点的属性出现错误：
                The connection string 'TecentDA' in the application's configuration file does not contain the required providerName attribute."
                */
                /*
                 配置文件中中增加节点出现异常：
                 The Entity Framework provider type 'System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer' registered in the application config file for the ADO.NET provider with invariant name 'System.Data.SqlClient' could not be loaded. Make sure that the assembly-qualified name is used and that the assembly is available to the running application. See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.
                 */
                /*
                A null store-generated value was returned for a non-nullable member 'Id' of type 'CefSharpWin.CategoryData'. 
                */
            }
        }
        
    }
}
