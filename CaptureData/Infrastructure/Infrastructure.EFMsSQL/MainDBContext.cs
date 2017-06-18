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
    }
}
