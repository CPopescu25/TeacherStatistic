using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Data.Context.Enums;

namespace TS.Data.Context.Models
{
    public class Answers
    {
        public Answers()
        {
            this.StatisticAnswers = new HashSet<StatisticAnswers>();
        }
        [Key]
        public Guid ID { get; set; }

        public System.Guid UserID { get; set; }

        public SetType SetType { get; set; }

        public System.Guid User_who_answer_ID { get; set; }

        public virtual ICollection<StatisticAnswers> StatisticAnswers { get; set; }
    }
}
