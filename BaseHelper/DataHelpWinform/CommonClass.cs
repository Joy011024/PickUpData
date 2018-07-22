using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DataHelpWinform
{
    public class TimeIntervalParam
    {
        /// <summary>
        /// 设置定时的类别
        /// </summary>
        public TimeIntervalCategory SetIntervalCategory { get; set; }
        /// <summary>
        /// 触发次数
        /// </summary>
        public int TriggerCount { get; set; }
        /// <summary>
        /// 指定的时间间隔
        /// </summary>
        public int? DateTimeSpan { get; set; }
        /// <summary>
        /// 设置的具体时间
        /// </summary>
        public DateTime? SetDateTime { get; set; }
        /// <summary>
        /// 指定某个时间之后才开始执行
        /// </summary>
        public DateTime? AfterDateTimeRun { get; set; }
    }
    public enum ComboBoxItem
    {
        Value = 1,
        Key = 2
    }
    public enum TimeIntervalCategory
    {
        [Description("自定义时间")]
        CustomTime = 1,
        [Description("指定时间")]
        FixedTime = 2,
        [Description("放弃定时")]
        Quit = -1
    }
    public enum TimeSpanCategory
    {
        [Description("秒")]
        Second = 1,
        [Description("分")]
        Minute = 2
    }
}
