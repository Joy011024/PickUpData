using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using IHRApp.Infrastructure;
namespace HRApp.Infrastructure
{
    public class SpecialSpellNameRepository:ISpecialSpellNameRepository
    {
        public string SqlConnString
        {
            get;
            set;
        }

        public bool Add(SpecialSpellName entity)
        {
            string cmd = @"INSERT INTO [HrApp].[dbo].[SpecialSpellName] ([Name],[Code],[IsDeleted],[IsErrorSpell])
     VALUES ({Name},{Code},{IsDeleted},{IsErrorSpell}) ";
            return CommonRepository.ExtInsert<SpecialSpellName>(cmd, SqlConnString, entity);
        }

        public bool Edit(SpecialSpellName entity)
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

        public SpecialSpellName Get(object key)
        {
            throw new NotImplementedException();
        }

        public IList<SpecialSpellName> Query(string cmd)
        {
            throw new NotImplementedException();
        }
    }
}
