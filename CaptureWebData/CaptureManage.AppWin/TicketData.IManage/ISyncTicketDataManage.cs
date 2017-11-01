using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketData.Model;
namespace TicketData.IManage
{
    public interface ISyncTicketDataManage
    {
        void SaveCarWithSeatData(leftTicketDTOResponse record);
    }
}
