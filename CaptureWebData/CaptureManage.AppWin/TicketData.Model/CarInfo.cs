using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketData.Model
{
    public class CarInfo
    {
        public Guid Id { get; set; }
        public string TranNo { get; set; }
        public string CarNo { get; set; }
        public string BeginStation { get; set; }
        public string ToStation { get; set; }
        public string StartStation { get; set; }
        public string EndStation { get; set; }
        public string CreateTime { get; set; }
    }
    public class TicketSeatData 
    {
        public Guid Id { get; set; }
        public Guid CarFKId { get; set; }//外键 CarInfo
        /// <summary>
        /// 不同座位同一车次类型的标志
        /// </summary>
        public Guid TicketId { get; set; }
        public DateTime CreateTime { get; set; }
        public int TicketNum { get; set; }
        public string TicketNumDesc { get; set; }
        public float TicketPrice { get; set; }
        public string TicketType { get; set; }

    }
}
