using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Data.Context.Models;
using TS.DTO.Enums;

namespace TS.DTO
{
    public class AnswersHelpDTO
    {
        public string User { get; set; }
        public Guid User_Id { get; set; }
        public SetType SetType { get; set; }
        public List<StatisticAnswersHelpDTO> List_Statistics { get; set; }
        public string Message { get; set; }
        public virtual List<StatisticAnswers> StatisticAnswers { get; set; }

        public Guid User_who_answer_Id { get; set; }
    }
}
