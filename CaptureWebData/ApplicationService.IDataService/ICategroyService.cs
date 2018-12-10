﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonData;
namespace ApplicationService.IDataService
{
    public interface  ICategroyService
    {
        string ConnString { get; set; }
        IEnumerable<CategoryData> QueryCityCategory(string key);
        
    }
    public interface IQQDataService
    {
        string ConnString { get; set; }
        void SaveQQ(List<FindQQDataTable> data);
    }
}
