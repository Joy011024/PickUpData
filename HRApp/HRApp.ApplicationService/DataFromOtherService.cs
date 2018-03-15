using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonData;
using HRApp.IApplicationService;
using IHRApp.Infrastructure;
namespace HRApp.ApplicationService
{
    [DescriptionSort("数据查询来自其他平台")]
    public class DataFromOtherService : IDataFromOtherService
    {
        /*
         通过查询其他数据库下的数据，脚本执行格式模板
         
DECLARE	@return_value int
EXEC	@return_value = TecentDataUinDA.[dbo].[SP_QueryAccount]
		@beginTime = NULL,
		@endTime = NULL

SELECT	'Return Value' = @return_value
         */
        IDataFromOtherRepository dataFromRepository;
        public DataFromOtherService(IDataFromOtherRepository repository)
        {
            dataFromRepository = repository;
        }
        public Common.Data.JsonData QueryUinList(DateTime beginTime, DateTime endTime, int beginRow, int endRow)
        {
            //根据数据库配置
            int count = 0;
            Common.Data.JsonData json = new Common.Data.JsonData();
            try
            {
                json.Data = dataFromRepository.QueryUinList(beginTime, endTime, beginRow, endRow, out count);
                json.Total = count;
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
            }
            return json;
        }

        public string SqlConnString
        {
            get;
            set;
        }

        public Common.Data.JsonData Add(FindQQDataTable model)
        {
            throw new NotImplementedException();
        }

        public List<FindQQDataTable> QueryWhere(FindQQDataTable model)
        {
            throw new NotImplementedException();
        }
    }
}
