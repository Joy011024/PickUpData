using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.ExtService;
using CaptureManage.AppWin.TicketData.Model.Request;
namespace CaptureManage.AppWin
{
    /// <summary>
    /// http协议请求或者响应相关配置
    /// </summary>
    public class HttpProtocolConfig
    {
        public VerifyCode GetVerifyCodeParamStatic(string filePath, string nodeName) 
        {
           return new VerifyCode().GetEntityConfig(filePath, nodeName,true);
        }
    }
}
