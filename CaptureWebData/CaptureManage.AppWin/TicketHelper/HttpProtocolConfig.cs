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
        public void GetVerifyCodeParamStatic(string filePath,string nodeName) 
        {
            new VerifyCode().GetEntityConfig(filePath, nodeName);
        }
    }
}
