using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace CommonHelperEntity
{
    public class AttachInforAttribute:Attribute
    {
        public  readonly Dictionary<string, object> AttachData = new Dictionary<string, object>();
        /// <summary>
        /// 附加内容
        /// </summary>
        /// <param name="Key">内容的关键字</param>
        /// <param name="Value">内容</param>
        public AttachInforAttribute(string Key, object Value) 
        {
            AttachData.Add(Key, Value);
        }
        /// <summary>
        /// 获取附加内容的全部关键字
        /// </summary>
        public void GetAttachKeys() 
        {
            List<string> keys = new List<string>();
            foreach (KeyValuePair<string,object> item in AttachData)
            {
                keys.Add(item.Key);
            }
        }
    }
}
