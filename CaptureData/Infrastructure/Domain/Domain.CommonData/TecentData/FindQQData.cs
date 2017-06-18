using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.CommonData
{
    [System.ComponentModel.DataAnnotations.Schema.Table("TecentQQData")]
    public class FindQQDataTable
    {
        public Guid ID { get; set; }
        public DateTime CreateTime { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int Distance { get; set; }
        public int Face { get; set; }
        public string Gender { get; set; }
        public string Nick { get; set; }
        public string Province { get; set; }
        public int Stat { get; set; }
        public string Uin { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.Column("HeadImageUrl")]
        public string Url { get; set; }
    }
}
