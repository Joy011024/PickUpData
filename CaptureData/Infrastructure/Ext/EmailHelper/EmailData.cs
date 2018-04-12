using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmailHelper
{
    public class EmailData
    {
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }

        /// <summary>
        /// 邮件主题
        /// </summary>
        public string EmailSubject { get; set; }
        /// <summary>
        /// 邮件内容
        /// </summary>
        public string EmailBody { get; set; }
        private DateTime? _CreateTime;
        /// <summary>
        /// 邮件创建时间
        /// </summary>
        public DateTime? CreateTime
        {
            get { return _CreateTime; }
            set
            {
                _CreateTime = value;
                if (!SendTime.HasValue) { SendTime = value; }
            }
        }
        /// <summary>
        /// 发送时间【定时触发】
        /// </summary>
        public DateTime? SendTime { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<byte[]> File { get; set; }
       
    }
}
