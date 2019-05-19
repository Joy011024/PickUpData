using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace CaptureWebData
{
    public class ProxyDataService
    {
        #region help entity
        public class Proxy
        {
            [System.ComponentModel.DataAnnotations.Key]
            public int IP_Int { get; set; }
            public int Port { get; set; }
            public DateTime Last_Valid_Time { get; set; }
            public string IP { get; set; }
            public int Status { get; set; }
        }
        public class DataContextHelp<T>:DbContext where T:class
        {
            public DataContextHelp(string connString) :base(connString)
            {

            }
            public DbSet<T> CustomEntity { get; set; }
        }

        #endregion
        #region  api function
        string connString;
        public ProxyDataService(string connString)
        {
            this.connString = connString;
        }
        public List<Proxy> GetUsageProxy()
        {
            List<Proxy> ps = new List<Proxy>();
            try
            {
                DataContextHelp<Proxy> context = new DataContextHelp<Proxy>(connString);
                int begin = 0;
                int end = 30;
                ps = context.CustomEntity.AsQueryable().Skip(begin).Take(end - begin).ToList();
            }
            catch (Exception ex)
            {

            }
            return ps;
        }
        #endregion
    }
}
