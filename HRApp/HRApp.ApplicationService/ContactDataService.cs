using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using HRApp.IApplicationService;
using HRApp.Infrastructure;
using IHRApp.Infrastructure;
namespace HRApp.ApplicationService
{
    public class ContactDataService:IContactDataService
    {
        public string SqlConnString
        {
            get;
            set;
        }
        IContactDataRepository appRepostory;
        public ContactDataService(IContactDataRepository _appReposiory) 
        {
            appRepostory = _appReposiory;
        }


        public Common.Data.JsonData Add(ContactData model)
        {
            Common.Data.JsonData json = new Common.Data.JsonData() { Result=true};
            try
            {
                json.Success = appRepostory.Add(model);
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
            }
            return json;
        }

        public List<ContactData> QueryWhere(ContactData model)
        {
            throw new NotImplementedException();
        }


        public ContactData Get(object id)
        {
            throw new NotImplementedException();
        }
    }
}
