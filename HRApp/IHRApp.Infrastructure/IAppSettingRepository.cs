﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
namespace IHRApp.Infrastructure
{
    public interface IAppSettingRepository:IBaseListRepository<CategoryItems>
    {
        List<CategoryItems> GetNodeListByParent(string parentNodeCode);
    }
}
