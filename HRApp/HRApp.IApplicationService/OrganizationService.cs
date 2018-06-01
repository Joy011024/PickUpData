using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using Domain.CommonData;
namespace HRApp.IApplicationService
{
    public interface IOrganizationService : ICountServiceWithConnString<Organze>
    {
        List<FieldContainerCode> QueryOrganzes(string queryKey);
    }
}
