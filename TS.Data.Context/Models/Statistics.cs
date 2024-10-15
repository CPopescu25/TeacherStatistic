using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Data.Context.Enums;

namespace TS.Data.Context.Models
{
    public class Statistics
    {
        public Statistics()
        {
            this.FirstStatisticQuestions = new HashSet<StatisticQuestions>();
            this.StudentsTargets = new HashSet<StudentsTargets>();
        }
        [Key]
        public Guid ID { get; set; }

        public Nullable<DateTime> AddDate { get; set; }

        public SetType SetType { get; set; }

        public Guid UserID { get; set; }

        public virtual ICollection<StatisticQuestions> FirstStatisticQuestions { get; set; }

        public virtual ICollection<StudentsTargets> StudentsTargets { get; set; }

    }
}
