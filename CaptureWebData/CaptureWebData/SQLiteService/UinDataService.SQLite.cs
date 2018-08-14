using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
namespace CaptureWebData
{
    public class UinDataService
    {
        public void Excute(string sql)
        {
            try
            {
                ConfigurationItems cfg = new ConfigurationItems();
                SQLiteConnection conn = new SQLiteConnection(cfg.SqliteDbConnString);
                conn.Open();
                SQLiteCommand com = new SQLiteCommand(sql, conn);
                int succ = com.ExecuteNonQuery();
                string sqlq = "select * from TecentQQData";
                SQLiteCommand comq = new SQLiteCommand(sqlq, conn);
                SQLiteDataAdapter dap = new SQLiteDataAdapter();
                dap.SelectCommand = comq;

                DataSet ds=new DataSet();
                dap.Fill(ds);
                conn.Clone();
            }
            catch (Exception ex)
            { 
            
            }
        }
    }

    public class SQLiteReporistory<T> :   System.Data.Entity.DbContext where T : class
    {
        public SQLiteReporistory(string connString)
            : base(connString)
        {

        }
        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
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
