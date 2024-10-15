using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.DTO;

namespace BusinessServices.Statistics
{
    public interface IStatisticsLogic : IBusinessService
    {
        Data Create(TS.Data.Context.Models.Statistics model);
        Data Delete(Guid id);
        Data Details(StatisticHelpDTO model);
        Data Index(int setType);
        Data TeacherQuestionnaires(Guid teacherId);
        List<TeachersStatisticsDTO> TeachersStatisticsByUniversity(SearchModelDTO model);
    }
}
