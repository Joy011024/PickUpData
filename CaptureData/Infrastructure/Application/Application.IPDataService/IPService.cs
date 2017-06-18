using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.CommonData;
using Infrastructure.EFMsSQL;
namespace ApplicationService.IPDataService
{
    public class IPService
    {
        public string SqlConnString { get;private set; }
        public IPService(string connString)
        {
            SqlConnString = connString;
        }
        public bool SaveList(List<IpDataMapTable> ips) 
        {
            MainRespority<IpDataMapTable> mr = new MainRespority<IpDataMapTable>(SqlConnString);
            return mr.InsertList(ips);
        }
    }
}
