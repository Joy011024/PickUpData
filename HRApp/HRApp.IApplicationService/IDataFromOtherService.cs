using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonData;
using Common.Data;
namespace HRApp.IApplicationService
{
    public interface IDataFromOtherService : IBaseServiceWithSqlConnstring<FindQQDataTable>
    {
        JsonData QueryUinList(DateTime beginTime,DateTime endTime,int beginRow,int endRow);
    }
}
