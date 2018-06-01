using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using HRApp.IApplicationService;
using System.ComponentModel;
using Infrastructure.ExtService;
using AppLanguage;
using IHRApp.Infrastructure;
using Domain.CommonData;
namespace HRApp.ApplicationService
{
    public class OrganizationService:IOrganizationService
    {
        public List<Organze> QueryAll()
        {
            throw new NotImplementedException();
        }
        public IOrganizationRepository organzeDal;
        public OrganizationService(IOrganizationRepository organzeRepository) 
        {
            organzeDal = organzeRepository;
        }
        public string SqlConnString
        {
            get;
            set;
        }
        [Description("组织数据录入")]
        public Common.Data.JsonData Add(Organze model)
        {
            Common.Data.JsonData json = new Common.Data.JsonData() { Result=true};
            if (string.IsNullOrEmpty(model.Name)) 
            {
                json.Message = Lang.Tip_NameIsRequired;
                return json;
            }
            if (string.IsNullOrEmpty(model.Code)) 
            {
                model.Code = model.Name.TextConvertChar();
            }
            model.CreateTime = DateTime.Now;
            try
            {
                json.Success = organzeDal.Add(model);
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
            }
            return json;
        }

        public List<Organze> QueryWhere(Organze model)
        {
            throw new NotImplementedException();
        }

        public Organze Get(object id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Organze entity)
        {
            throw new NotImplementedException();
        }

        public List<FieldContainerCode> QueryOrganzes(string queryKey)
        {
            List<Organze> list= organzeDal.QueryOrganzes(new RequestParam() { QueryKey = queryKey, RowBeginIndex = 0, RowEndIndex = int.MaxValue });
            List<FieldContainerCode> output = new List<FieldContainerCode>();
            foreach (Organze item in list)
            {
                output.Add(item);
            }
            return output;
        }


        public int Count(object entity)
        {
            RequestParam  key = entity as RequestParam;
            return organzeDal.Count(key);
        }
    }
}
