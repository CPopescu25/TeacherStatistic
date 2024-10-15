using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.DTO
{
    public class StatisticAnswersHelpDTO
    {
        public Guid AnswerId { get; set; }
        public int n1 { get; set; }
        public int n2 { get; set; }
        public int n3 { get; set; }
        public int n4 { get; set; }
        public int n5 { get; set; }

        public double P { get; set; }
        public string C { get; set; }

        public int Nr { get; set; }
        //for excel
        public string SetType { get; set; }
    }
}
