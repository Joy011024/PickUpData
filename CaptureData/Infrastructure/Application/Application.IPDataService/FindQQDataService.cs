using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using Domain.CommonData;
using Infrastructure.EFMsSQL;
namespace CaptureWebData
{
    public class FindQQDataService
    {
        public string ConnString { get; set; }
        public FindQQDataService(string connString) 
        {
            ConnString = connString;
        }
        public void SaveFindQQ( List<FindQQDataTable> data) 
        {
            try 
            {
                DateTime now = DateTime.Now;
                foreach (FindQQDataTable item in data)
                {
                    item.ID = Guid.NewGuid();
                    item.CreateTime = now;
                }
                MainRespority<FindQQDataTable> mr = new MainRespority<FindQQDataTable>(ConnString);
                mr.InsertList(data);

            }
            catch (Exception ex) { }
        }
    }
}
