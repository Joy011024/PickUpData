using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Infrastructure.EFSQLite
{
    public class SQLiteReporistory<T> : System.Data.Entity.DbContext where T : class
    {
        string conn;
        public SQLiteReporistory(string connString)
            : base(connString)
        {
            conn = connString;
        }
        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public System.Data.Entity.IDbSet<T> Entity { get; set; }
        public void BatchAdd(List<T> data)
        {
            try
            {
                foreach (var item in data)
                {
                    Entity.Add(item);
                }
                int succ = SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
