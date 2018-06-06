using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using IHRApp.Infrastructure;
using HRApp.IApplicationService;
using Domain.CommonData;
using Infrastructure.ExtService;
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


        public int BatchInsert<EnumFieldAttribute>(Enum e) where EnumFieldAttribute:Attribute
        {
            Dictionary<string, string> dict = e.EnumFieldDescDict<EnumFieldAttribute>("Description");
            if (dict.Count == 0)
            {
                return -1;
            } 
            Type en=e.GetType();
            List<string> insert = new List<string>();
            List<string> error = new List<string>();
            //枚举入库
            EnumData ep = new EnumData()
            {
                Name = en.Name,
                Code = en.Name
            };
            ep.Init();
            //进行数据库中存储[返回主键id]
            int id = enumRepository.AddReturnId(ep);
            //存储子项
            foreach (var item in dict)
            {
                string enumName = en.Name;
                string name = enumName + "." + item.Key;
                try
                {
                    object val = Enum.Parse(en, item.Key);
                    int v = val.GetHashCode();

                    EnumData ed = new EnumData()
                    {
                        Code = item.Key,
                        Name = name,
                        Remark = item.Value,
                        ParentId = id,
                        CreateTime = DateTime.Now,
                        IsDelete = false,
                        Value = v
                    };
                    enumRepository.AddReturnId(ed);
                    insert.Add(name);
                }
                catch (Exception ex)
                {
                    error.Add(name+"【"+ex.Message+"】");
                }
            }
            logDal.WriteLog(ELogType.DataInDBLog, "success:" + string.Join(" ", insert) + ",error: " + string.Join(" ", error), "InsertLog", error.Count > 0);
            return -1;
        }
    }
}
