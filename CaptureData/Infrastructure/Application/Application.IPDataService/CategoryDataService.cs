using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.CommonData;
using Infrastructure.EFMsSQL;
namespace ApplicationService.IPDataService
{
    public class CategoryDataService
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
                    item.Id = Guid.NewGuid().GetHashCode();
                    if (item.Id < 0) 
                    {
                        item.Id = 0 - item.Id;
                    }
                    item.IsDelete = false;
                    item.CreateTime = now;
                    data=data.Where(ch => {
                        if (!string.IsNullOrEmpty(ch.ParentCode) && ch.ParentCode == item.Code) 
                        {
                            ch.ParentId = item.Id;
                        }
                        return true;
                    }).ToList();
                }

                MainRespority<CategoryData> mr = new MainRespority<CategoryData>(ConnString);
                mr.InsertList(data);
            }
            catch (Exception ex)
            {
            
            }
        }
        public IEnumerable<CategoryData> QueryCityCategory() 
        {
            MainRespority<CategoryData> main = new MainRespority<CategoryData>(ConnString);
            IEnumerable<CategoryData> data= main.Query(t => !t.IsDelete && t.ItemType == "City" && !string.IsNullOrEmpty(t.Name));
            return data;
        }
    }
}
