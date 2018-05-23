using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using Domain.CommonData;
namespace IHRApp.Infrastructure
{
    public interface ILogDataRepository
    {
        string SqlConnString { get; set; }
        bool WriteLog(ELogType type, string log, string title, bool isError);
    }
}
