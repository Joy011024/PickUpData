using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationService.MainContextData
{
    /// <summary>
    /// 进度响应【这是一个帮助类，不匹配数据库】
    /// </summary>
    public class ProcessResponse
    {
        public int? ErrorCode { get; set; }
        public string ErrorMsg { get; set; }
    }
}
