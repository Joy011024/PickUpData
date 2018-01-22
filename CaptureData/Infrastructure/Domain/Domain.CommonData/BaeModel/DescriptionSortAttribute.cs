using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.CommonData
{
    public class DescriptionSortAttribute : DescriptionAttribute
    {
        public int Sort { get; set; }
        public DescriptionSortAttribute(string description, int sort = 0)
        {
            Sort = sort;
            DescriptionValue = description;
        }
    }
}
