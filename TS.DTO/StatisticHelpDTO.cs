using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Data.Context.Models;
using TS.DTO.Enums;

namespace TS.DTO
{
    public class StatisticHelpDTO
    {
        public Guid Id { get; set; }
        public string AddDate { get; set; }
        public string User { get; set; }
        public Guid User_Id { get; set; }
        public TS.DTO.Enums.SetType SetType { get; set; }
        public List<StudentsTargetsHelpDTO> List_Students { get; set; }
        public List<StatisticQuestionHelpDTO> List_Questions { get; set; }
        public string Message { get; set; }

        public int CountStudents { get; set; }

        public DateTime addDate { get; set; }

        public virtual List<StatisticQuestions> FirstStatisticQuestions { get; set; }

        public virtual List<StudentsTargets> StudentsTargets { get; set; }
    }
}
