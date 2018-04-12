using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaptureManage.AppWin.Model
{
    public class TicketCfg
    {
        public int TimeSpan { get; set; }
        public bool OpenDb { get; set; }
        public int FirstSeatCategory { get; set; }
        public string FirstCarCategory { get; set; }
        /// <summary>
        /// 乘车日期
        /// </summary>
        public string CarDate { get; set; }
        /// <summary>
        /// 开始抢票时间
        /// </summary>
        public DateTime StartTriggerTime { get; set; }
        /// <summary>
        /// 抢票成功时间
        /// </summary>
        public DateTime SuccessTime { get; set; }
        /// <summary>
        /// 抢票进行次数
        /// </summary>
        public int TryTime { get; set; }
        /// <summary>
        /// 抢票失败是官方给出的错误消息
        /// </summary>
        public string ErrorMsg { get; set; }
        public string ErrorCode { get; set; }
    }
}
