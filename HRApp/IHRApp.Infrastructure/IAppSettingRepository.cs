using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
namespace IHRApp.Infrastructure
{
    public interface IAppSettingRepository:IBaseListRepository<CategoryItems>
    {
        List<CategoryItems> GetNodeListByParent(string parentNodeCode);
        /// <summary>
        /// 验证内容是否存在
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        int ValideExists(string key);
        bool ChangeSpell(int id, string spell);
        int BatchChangeSpell(Dictionary<int, string> idWithSpells);
        List<CategoryItems> QueryNodesByIndex(string keySpell);
    }
}
