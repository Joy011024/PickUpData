using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using IHRApp.Infrastructure;
using HRApp.IApplicationService;
using Domain.CommonData;
namespace HRApp.ApplicationService
{
    public class EnumDataService:IEnumDataService
    {
        public IEnumDataRepository enumRepository;
        public ILogDataRepository logDal;//2018-05-24 属性可以不用再通过构造函数进行注入
        public EnumDataService(IEnumDataRepository enumDal) 
        {
            enumRepository = enumDal;
        }
        public bool Add(EnumData data)
        {
            logDal.WriteLog(ELogType.DataInDBLog, 
                string.Format(LogData.InsertDbNoteFormat(), typeof(EnumData).Name),
                "insert", true);
            return enumRepository.Add(data);
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


        public bool UpdateRemark(int id, string remark)
        {
            logDal.WriteLog(ELogType.DataInDBLog, "update dict Remark", "Update", true);
            return enumRepository.UpdateRemark(id, remark);
        }
    }
}
