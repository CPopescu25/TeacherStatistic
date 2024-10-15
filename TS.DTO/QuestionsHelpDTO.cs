using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Data.Context.Models;
using TS.DTO.Enums;

namespace TS.DTO
{
    public class QuestionsHelpDTO
    {
        public Guid Id { get; set; }

        public string QuestionName { get; set; }

        public SetType SetType { get; set; }

        public string A1 { get; set; }

        public string A2 { get; set; }

        public string A3 { get; set; }

        public string A4 { get; set; }

        public string A5 { get; set; }

        public int V1 { get; set; }

        public int V2 { get; set; }

        public int V3 { get; set; }

        public int V4 { get; set; }

        public int V5 { get; set; }

        public List<StatisticAnswers> StatisticAnswers { get; set; }

        public List<StatisticQuestions> StatisticQuestions { get; set; }
    }
}
