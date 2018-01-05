using Common.Data;
using HRApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRApp.IApplicationService
{
    public interface IAppSettingService
    {
        string SqlConnString { get; set; }
        JsonData Add(CategoryItems item);
    }
}
