using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonData;
namespace IHRApp.Infrastructure
{
    public interface IDataFromOtherRepository : IBaseListRepository<FindQQDataTable>
    {
        List<FindQQDataTable> QueryUinList(DateTime beginTime, DateTime endTime, int beginRow, int endRow,out int count);
    }
}
