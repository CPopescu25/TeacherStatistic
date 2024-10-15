using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.DTO
{
    public class TeachersStatisticsDTO
    {
        public Guid TeacherID { get; set; }
        public string TeacherName { get; set; }
        public double P { get; set; }
        public string C { get; set; }
    }
}
