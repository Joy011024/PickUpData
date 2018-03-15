using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonData;
namespace HRApp.ApplicationService
{
    [DescriptionSort("数据查询来自其他平台")]
    public class DataFromOtherService
    {
        /*
         通过查询其他数据库下的数据，脚本执行格式模板
         
DECLARE	@return_value int
EXEC	@return_value = TecentDataUinDA.[dbo].[SP_QueryAccount]
		@beginTime = NULL,
		@endTime = NULL

SELECT	'Return Value' = @return_value
         */

    }
}
