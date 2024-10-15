using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.Data.Context.Models
{
    public class UserUniversity
    {
        [Key]
        [Column(Order = 1)]
        public Guid UserID { get; set; }

        [Key]
        [Column(Order = 2)]
        public int UniversityID { get; set; }
    }
}
