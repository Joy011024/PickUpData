using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServiceSmallTool
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {   Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ParallelFrm());
        }
        static void Pick() 
        {
            var str = "<MsgType id=271 flag=1>ICAO到期提醒</MsgType><MsgType id=290 flag=1>身份证到期更换提醒</MsgType><MsgType id=287 flag=1>外籍工作证到期提醒</MsgType><MsgType id=299 flag=1>飞行90天内3次起降提醒</MsgType><MsgType id=237 flag=1>复训熟练检查到期提醒</MsgType><MsgType id=283 flag=1>岗级转升</MsgType><MsgType id=211 flag=1>港澳通行证到期提醒</MsgType><MsgType id=300 flag=1>7天36小时休息</MsgType><MsgType id=301 flag=1>7天飞行时间超40小时</MsgType><MsgType id=78 flag=1>安排宾馆</MsgType><MsgType id=74 flag=1>出境生成</MsgType><MsgType id=75 flag=1>发报</MsgType><MsgType id=65 flag=1>飞行时间超时</MsgType><MsgType id=62 flag=1>航班备降</MsgType><MsgType id=61 flag=1>航班返航</MsgType><MsgType id=63 flag=1>航班取消</MsgType><MsgType id=60 flag=1>航班延误</MsgType><MsgType id=76 flag=1>航前接车</MsgType><MsgType id=77 flag=1>航医检查</MsgType><MsgType id=69 flag=1>航站接飞不上</MsgType><MsgType id=64 flag=1>换机型机号</MsgType><MsgType id=80 flag=1>换停机位</MsgType><MsgType id=79 flag=1>回收</MsgType><MsgType id=72 flag=1>计划发布</MsgType><MsgType id=71 flag=1>计划审批</MsgType><MsgType id=70 flag=1>计划生效</MsgType><MsgType id=73 flag=1>任务书打印</MsgType><MsgType id=68 flag=1>时间接飞不上</MsgType><MsgType id=67 flag=1>休息时间不足</MsgType><MsgType id=66 flag=1>执勤时间超时</MsgType><MsgType id=220 flag=1>护照到期提醒</MsgType><MsgType id=273 flag=1>机长年度航线检查到期提醒</MsgType><MsgType id=289 flag=1>责任机长年满58周岁提醒</MsgType><MsgType id=402 flag=1>不可飞2500飞机</MsgType><MsgType id=400 flag=1>岗位缺人</MsgType><MsgType id=401 flag=1>换机型机号</MsgType><MsgType id=272 flag=1>签证提醒</MsgType><MsgType id=223 flag=1>人员生日提醒</MsgType><MsgType id=269 flag=1>日本报备到期提醒</MsgType><MsgType id=210 flag=1>入台许可证到期提醒</MsgType><MsgType id=209 flag=1>台湾通行证到期提醒</MsgType><MsgType id=282 flag=1>特殊机场放飞到期提醒</MsgType><MsgType id=225 flag=1>体检合格证到期提醒</MsgType><MsgType id=279 flag=1>临时执照证件到期提醒</MsgType><MsgType id=295 flag=1>聘任后120个日历日内新岗位经历时间监控</MsgType><MsgType id=286 flag=1>外籍合同到期提醒</MsgType><MsgType id=284 flag=1>机务人员已安排未组环提醒</MsgType><MsgType id=305 flag=1>外籍居留证到期提醒</MsgType><MsgType id=274 flag=1>排班技术授权提醒</MsgType><MsgType id=219 flag=1>应急生存训练提醒</MsgType><MsgType id=234 flag=1>危险品航空运输训练到期提醒</MsgType><MsgType id=275 flag=1>有执照无授权提醒</MsgType><MsgType id=281 flag=1>执照到期提醒</MsgType>";
            var regex = ">(.*?)<"; 
            Regex reg = new Regex(regex); 
            MatchCollection mc = reg.Matches(str);
            StringBuilder sb = new StringBuilder();
            foreach (Match item in mc)
            { //这是一条完整的记录
                if (item.Groups.Count <= 1)
                {
                    continue;
                }
                string st = item.Groups[1].Value;
                sb.AppendLine(st);
            } 

        }
    }
}
 