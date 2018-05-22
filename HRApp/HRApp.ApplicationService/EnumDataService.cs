using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using IHRApp.Infrastructure;
using HRApp.IApplicationService;
namespace HRApp.ApplicationService
{
    public class EnumDataService:IEnumDataService
    {
        public IEnumDataRepository enumRepository;
        public EnumDataService(IEnumDataRepository enumDal) 
        {
            enumRepository = enumDal;
        }
        public bool Add(EnumData data)
        {
            throw new NotImplementedException();
        }

        public List<EnumData> QueryList(RequestParam param)
        {
            throw new NotImplementedException();
        }

        public string SqlConnString
        {
            get;
            set;
        }

        Common.Data.JsonData IBaseServiceWithSqlConnstring<EnumData>.Add(EnumData model)
        {
            throw new NotImplementedException();
        }

        public List<EnumData> QueryWhere(EnumData model)
        {
            throw new NotImplementedException();
        }

        public EnumData Get(object id)
        {
            throw new NotImplementedException();
        }

        public bool Update(EnumData entity)
        {
            throw new NotImplementedException();
        }
    }
}
