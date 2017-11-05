using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketData.Manage
{
    /// <summary>
    /// 12306 数据预处理
    /// </summary>
    public class TicketDataPrepareManager
    {
        /// <summary>
        /// 生成12306登录验证码加载图片的随机数
        /// </summary>
        /// <returns>随机数</returns>
        public static string GenerateVerifyCodeGuid() 
        {
            float rand =(float) (new Random()).NextDouble();
            return rand.ToString();
        }
    }
}
