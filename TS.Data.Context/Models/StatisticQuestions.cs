using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.Data.Context.Models
{
    public class StatisticQuestions
    {
        [Key]
        [Column(Order = 1)]
        public Guid StatisticsID { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid QuestionsID { get; set; }
    }
}
