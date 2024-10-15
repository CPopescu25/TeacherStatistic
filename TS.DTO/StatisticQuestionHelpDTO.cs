using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.DTO
{
    public class StatisticQuestionHelpDTO
    {
        public Guid Id { get; set; }
        public string Question_Name { get; set; }
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
    }
}
