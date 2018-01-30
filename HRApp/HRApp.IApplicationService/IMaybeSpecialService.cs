using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Data;
namespace HRApp.IApplicationService
{
    public interface IMaybeSpecialService
    {
        JsonData AddMaybeSpecialChinese(char name, string code);
    }
}
