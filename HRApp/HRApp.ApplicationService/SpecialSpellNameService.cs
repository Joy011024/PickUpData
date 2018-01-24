using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using HRApp.IApplicationService;
using HRApp.Infrastructure;
using Common.Data;
using IHRApp.Infrastructure;
namespace HRApp.ApplicationService
{
    public class SpecialSpellNameService:ISpecialSpellNameService
    {
        ISpecialSpellNameRepository spellRepository;
        public SpecialSpellNameService(ISpecialSpellNameRepository spellDal)
        {
            spellRepository = spellDal;
        }
        public string SqlConnString
        {
            get;
            set;
        }

        public JsonData Add(SpecialSpellName model)
        {
            JsonData json = new JsonData() { Result=true};
            try
            {
                json.Success = spellRepository.Add(model);
            }
            catch (Exception ex)
            {
                json.Success = false;
                json.Message = ex.Message;
            }
            return json;
        }
    }
}
