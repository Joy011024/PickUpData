using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IHRApp.Infrastructure;
using HRApp.Model;
namespace HRApp.Infrastructure
{
    public class AppRepository:IAppRepository
    {

        public IList<AppModel> QueryList(string cmd, Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public string SqlConnString
        {
            get;
            set;
        }

        public bool Add(AppModel entity)
        {
            entity.IsDelete = 0;
            string sqlCmd = @"INSERT INTO [dbo].[App]  ([AppName],[AppCode],[IsDelete])
                    VALUES  ({AppName},{AppCode},{IsDelete}) ";
            return CommonRepository.ExtInsert<AppModel>(sqlCmd, SqlConnString, entity);
        }

        public bool Edit(AppModel entity)
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

        public AppModel Get(object key)
        {
            throw new NotImplementedException();
        }

        public IList<AppModel> Query(string cmd)
        {

            throw new NotImplementedException();
        }


        public IList<AppModel> QueryAll()
        {
            throw new NotImplementedException();
        }
    }
}
