using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using IHRApp.Infrastructure;
namespace HRApp.Infrastructure
{
    public class EnumDataRepository:IEnumDataRepository
    {
        public IList<EnumData> QueryList(string cmd, Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public IList<EnumData> QueryAll()
        {
            throw new NotImplementedException();
        }

        public string SqlConnString
        {
            get;
            set;
        }

        public bool Add(EnumData entity)
        {
            throw new NotImplementedException();
        }

        public bool Edit(EnumData entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(object key)
        {
            throw new NotImplementedException();
        }

        public bool LogicDel(object key)
        {
            throw new NotImplementedException();
        }

        public EnumData Get(object key)
        {
            throw new NotImplementedException();
        }

        public IList<EnumData> Query(string cmd)
        {
            throw new NotImplementedException();
        }
    }
}
