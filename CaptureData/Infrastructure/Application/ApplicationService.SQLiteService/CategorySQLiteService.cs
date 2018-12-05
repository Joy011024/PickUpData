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
}
