using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.Data.Context.Models
{
    public class University
    {
        public University()
        {
            this.UserUniversity = new HashSet<UserUniversity>();
        }

        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        [DefaultValue(0)]
        public virtual University Parent { get; set; }

        public virtual ICollection<UserUniversity> UserUniversity { get; set; }
    }
}
