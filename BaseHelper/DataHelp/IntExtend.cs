using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataHelp
{
    public static class IntExtend
    {
        /// <summary>
        /// 找出一组int类型数据中的最大数
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static int? GetMax(this List<int> arr) 
        {
            if (arr == null || arr.Count == 0) { return null; }
            int max = arr[0];
            foreach (int item in arr)
            {
                if (item > max) { max = item; }
            }
            return max;
        }
    }
}
