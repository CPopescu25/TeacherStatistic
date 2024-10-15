using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.Data.Context.Models
{
    public class StatisticAnswers
    {
        [Key]
        [Column(Order = 1)]
        public Guid QuestionsID { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid AnswersID { get; set; }

        public string answers { get; set; }

        public int value { get; set; }
    }
}
