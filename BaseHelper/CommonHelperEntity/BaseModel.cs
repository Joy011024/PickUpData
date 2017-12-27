using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonHelperEntity
{
    public abstract class BaseModel
    {
        /// <summary>
        /// 数据
        /// </summary>
        public virtual object Data { get; set; }
        /// <summary>
        /// 是否出现错误
        /// </summary>
        public virtual bool IsError { get; set; }
        /// <summary>
        /// 提示
        /// </summary>
        public virtual string Message { get; set; }
        public virtual string JsonData { get; set; }
    }
   
}
