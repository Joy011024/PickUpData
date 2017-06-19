using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using InfrastructureRepository.IMainContext;
namespace Infrastructure.EFMsSQL
{
    class MainDbContext<T> : DbContext where T : class
    {
        public DbSet<T> Entity { get; set; }
        static MainDbContext() 
        {
            Database.SetInitializer<MainDbContext<T>>(null);
        }
        public MainDbContext(string appSectionOrConnString) : base(appSectionOrConnString) 
        {
        
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //???
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
    public class MainRespority<T>:IMainRepository<T>   where  T: class
    {
        public bool HasException { get; private set; }
        public string Message { get; private set; }
        private MainDbContext<T> mainDB { get; set; }
        public MainRespority(string appSectionOrConnString)
        {
            mainDB = new MainDbContext<T>(appSectionOrConnString);
          //  Entity = main.Entry(T);
        }
        void InitData() 
        {
            HasException = false;
            Message = string.Empty;
        }
        void SetExceprion(string msg) 
        {
            Message = msg;
            HasException = true;
        }
        public bool Insert(T entity) 
        {
            try
            {
                InitData();
                mainDB.Entity.Add(entity);
                int result= mainDB.SaveChanges();
                if (result > 0) return true;
                return false;
            }
            catch (Exception ex) 
            {
                SetExceprion(ex.Message);
                return false;
            }
        }
        public bool InsertList(List<T> data) 
        {
            try
            {
                InitData();
                foreach (T item in data)
                {
                    mainDB.Entity.Add(item);
                }
                int result = mainDB.SaveChanges();
                if (result > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                SetExceprion(ex.Message);
                return false;
            }
        }
        public IEnumerable<T> Query(Func<T,bool> lambda) 
        {
            return mainDB.Entity.Where(lambda);
        }
        /// <summary>
        /// 执行非查询操作的存储过程
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="param"></param>
        /// <returns>返回受影响的行数</returns>
        public int ExecuteSPNoQuery(string cmd, params object[] param)
        {
            if (param == null)
            {
                param = new object[0];
            }
            int number = mainDB.Database.ExecuteSqlCommand(cmd, param);
            return number;
        }
        /// <summary>
        /// 执行查询操作的存储过程
        /// </summary>
        /// <typeparam name="R">返回的对象类型</typeparam>
        /// <param name="cmd">执行的存储过程语句</param>
        /// <param name="param">存储过程的参数</param>
        /// <returns></returns>
        public IEnumerable<R> ExecuteSPSelect<R>(string cmd, params object[] param)
        {
            if (param == null)
            {
                param = new object[0];
            }
            IEnumerable<R> obj = mainDB.Database.SqlQuery<R>(cmd, param);
            return obj;
        }
    }
}
