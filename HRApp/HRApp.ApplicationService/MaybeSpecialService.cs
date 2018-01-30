using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Data;
using IHRApp.Infrastructure;
using HRApp.IApplicationService;
namespace HRApp.ApplicationService
{
    public class MaybeSpecialService:IMaybeSpecialService
    {
        IMaybeSpecialRepository maybeSpecialSpellRepository;
        public MaybeSpecialService(IMaybeSpecialRepository maybeRepository) 
        {
            maybeSpecialSpellRepository = maybeRepository;
        }
        public JsonData AddMaybeSpecialChinese(char name, string code)
        {
            JsonData json = new JsonData() { Result = true };
            object outParam;
            try
            {
                json.Success = maybeSpecialSpellRepository.SaveMaybeSpecialWord(name, code, out outParam);
                json.Data = outParam;
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
            }
            return json;
        }
    }
}
