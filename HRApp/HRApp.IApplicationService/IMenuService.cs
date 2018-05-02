﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
namespace HRApp.IApplicationService
{
    public interface IMenuService : IBaseServiceWithSqlConnstring<Menu>
    {
        Common.Data.JsonData QueryAllMenu();
        List<Menu> QueryMenusByAuthor(string userCode);
    }
}
