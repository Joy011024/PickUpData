using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class SqlDBMappedAttribute : Attribute
    {
        public string Name { get; set; }
        public SqlDBMappedAttribute(string name) 
        {
            Name = name;
        }
    }
}
