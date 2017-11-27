using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketData.Model;
using TicketData.IService;
using TicketData.Service;
using TicketData.IManage;
using Infrastructure.ExtService;
namespace TicketData.Manage
{
    public class SyncTicketDataManage:ISyncTicketDataManage
    {
        AppCategory appType;
        string dbConnString;
        public SyncTicketDataManage(AppCategory category,string connString) 
        {
            appType = category;
            dbConnString = connString;
        }
        public void SaveCarWithSeatData(leftTicketDTOResponse record)
        {
            //throw new NotImplementedException();
        }
    }
}
