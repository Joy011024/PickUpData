using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationService.IDataService;
using Domain.CommonData;
using Infrastructure.EFSQLite;
namespace ApplicationService.SQLiteService
{
    public class CategorySQLiteService : ICategroyService
    {
        public string ConnString { get; set; }
        public CategorySQLiteService(string connString)
        {
            ConnString = connString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"> "City"</param>
        /// <returns></returns>
        public IEnumerable<CategoryData> QueryCityCategory(string key)
        {
            DBReporistory<CategoryData> main = new DBReporistory<CategoryData>(ConnString);
            IEnumerable<CategoryData> data = main.DoQuery<CategoryData>().Where(t => !t.IsDelete && t.ItemType == key && !string.IsNullOrEmpty(t.Name));
            return data;
        }
    }
    public class SQLiteQQDataService : IQQDataService
    {
        public string ConnString { get; set; }
        public SQLiteQQDataService(string connString)
        {
            ConnString = connString;
        }
        public void SaveQQ<T>(List<T> data) where T: FindQQDataTable
        {
            try
            {
                DBReporistory<TecentQQData> main = new DBReporistory<TecentQQData>(ConnString);
                DateTime now = DateTime.Now;
                foreach (FindQQDataTable item in data)
                {
                    item.ID = Guid.NewGuid();
                    item.CreateTime = now;
                    if (string.IsNullOrEmpty(item.Url))//没有采集到该账户的头像数据
                        item.ImgType = -1;
                    item.DayInt = int.Parse(now.ToString("yyyyMMdd"));
                }
                main.AddList(data.ToArray());//id在数据库中显示为乱码
            }
            catch (Exception ex)
            {

            }
        }
        public class TecentQQData : FindQQDataTable
        {

        }
    }
   
}
