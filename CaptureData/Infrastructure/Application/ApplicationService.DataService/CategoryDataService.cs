using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.CommonData;
using Infrastructure.EFMsSQL;
using System.Configuration;
using ApplicationService.IDataService;
namespace ApplicationService.DataService
{
    public class CategoryDataService: ICategroyService
    {
        public string ConnString { get; set; }
        public CategoryDataService(string connString) 
        {
            ConnString = connString;
        }
        public void SaveCategoryNode(List<CategoryData> data)
        {
            try
            {
                DateTime now = DateTime.Now;
                foreach (CategoryData item in data)
                {
                    item.IsDelete = false;
                    item.CreateTime = now;
                }
                MainRespority<CategoryData> mr = new MainRespority<CategoryData>(ConnString);
                mr.InsertList(data);
            }
            catch (Exception ex)
            {
            
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"> "City"</param>
        /// <returns></returns>
        public IEnumerable<CategoryData> QueryCityCategory(string key) 
        {
            MainRespority<CategoryData> main = new MainRespority<CategoryData>(ConnString);
            IEnumerable<CategoryData> data = main.Query(t => !t.IsDelete && t.ItemType == key && !string.IsNullOrEmpty(t.Name));
            //将数据同步到sqlite  :TecentDASQLite

            return data;
        }
    }

    public class CategoryDataServiceInIOFile : ICategroyService
    {
        public string ConnString { get; set; }
        /// <summary>
        /// 查找文件
        /// </summary>
        /// <param name="file">@"..\..\DB\City.log"</param>
        /// <returns></returns>
        public IEnumerable<CategoryData> QueryCityCategory(string file)
        {
            string json = FileHelper.ReadFile(file);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<CategoryData>>(json);
        }
    }


}
