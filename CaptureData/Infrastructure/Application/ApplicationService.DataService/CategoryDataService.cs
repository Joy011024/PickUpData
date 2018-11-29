﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.CommonData;
using Infrastructure.EFMsSQL;
using System.Configuration;
namespace ApplicationService.DataService
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
        public IEnumerable<CategoryData> QueryCityCategory() 
        {
            MainRespority<CategoryData> main = new MainRespority<CategoryData>(ConnString);
            IEnumerable<CategoryData> data = main.Query(t => !t.IsDelete && t.ItemType == "City" && !string.IsNullOrEmpty(t.Name));
            //将数据同步到sqlite  :TecentDASQLite

            return data;
        }
    }
    
}
