using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public enum DataType
    {
        Int=1,
        String=2,
        Bool=3,
        Guid= 4,
        Object=5
    }
    public class EnumMember
    {
        public string MemberName { get; set; }
        public int MemberValue { get; set; }
        public string MemberDesc { get; set; }
    }
    public enum ECompareDataCategory
    {
        Equal = 1,
        OnlyFirst = 2,
        OnlySecond = 3,
        EqualInIgnoreLowerAndNoEqual = 4,
        OnlyFirstInIgnoreLower = 5,
        OnlySecondInIgnoreLower = 6
    }

}
