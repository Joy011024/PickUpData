﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
namespace HRApp.IApplicationService
{
    public interface IRelyTableService
    {
        string SqlConnString { get; set; }
        List<RelyTable> QueryAllTableColumns();
    }
}
