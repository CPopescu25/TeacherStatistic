using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Data.Context.Enums;

namespace TS.Data.Context.Models
{
    public class Questions
    {
        public Questions()
        {
            this.StatisticAnswers = new HashSet<StatisticAnswers>();
            this.StatisticQuestions = new HashSet<StatisticQuestions>();
        }

        [Key]
        public Guid ID { get; set; }

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

        public virtual ICollection<StatisticAnswers> StatisticAnswers { get; set; }

        public virtual ICollection<StatisticQuestions> StatisticQuestions { get; set; }
    }
}
