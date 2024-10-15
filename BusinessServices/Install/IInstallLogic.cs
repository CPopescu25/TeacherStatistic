using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.DTO;

namespace BusinessServices.Install
{
    public interface IInstallLogic : IBusinessService
    {
        bool Admin();
        Data CkeckDatasInDatabase();
        bool ExportStatisticsById(System.Collections.Generic.List<StatisticAnswersHelpDTO> list, string name);
        bool Group();
        bool Questions();
        bool Universities();
        Data ExportTeacher(Guid teacherId);
    }
}
