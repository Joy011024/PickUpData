using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonHelperEntity
{
    public class DataCompareHelper
    {
        /// <summary>
        /// 数据比较结果
        /// </summary>
        public class DataCompareResult
        {
            /// <summary>
            /// 之前独有
            /// </summary>
            public string[] OnlyOldValue{get;set;}
            /// <summary>
            /// 现在独有
            /// </summary>
            public string[] OnlyNewValue { get; set; }
            /// <summary>
            /// 共同拥有
            /// </summary>
            public string[] EqualValue { get; set; }
        }
        /// <summary>
        /// 数据比较
        /// </summary>
        /// <param name="oldeValue">老数据</param>
        /// <param name="newValue">新数据</param>
        /// <param name="ingnoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public DataCompareResult StartCompare(string[] oldeValue, string[] newValue, bool ingnoreCase)
        {
            DataCompareResult result = new DataCompareResult();
            if (oldeValue == null || oldeValue.Length == 0)
            {
                result.OnlyNewValue = newValue;
                return result;
            }
            if (newValue == null || newValue.Length == 0)
            {
                result.OnlyOldValue = oldeValue;
                return result;
            }
            if (ingnoreCase)
            {
                oldeValue = oldeValue.Select(s => s.ToLower()).ToArray();
                newValue = newValue.Select(s => s.ToLower()).ToArray();
            }
            List<string> oldData = new List<string>();
            List<string> newData = new List<string>();
            List<string> equalData = new List<string>();
            foreach (string old in oldeValue)
            {
                if (newValue.Contains(old))
                {
                    equalData.Add(old);
                }
                else
                {
                    oldData.Add(old);
                }
            }
            foreach (string item in newValue)
            {
                if (!oldeValue.Contains(item))
                {
                    newData.Add(item);
                }
            }
            return new DataCompareResult() 
            {
                OnlyNewValue = newData.ToArray(), 
                OnlyOldValue = oldData.ToArray(), 
                EqualValue = equalData.ToArray()
            };
        }
    }
}
