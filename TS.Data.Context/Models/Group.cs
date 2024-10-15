using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.Data.Context.Models
{
    public class Group
    {
        public Group()
        {
            this.UserGroup = new HashSet<UserGroup>();
        }

        [Key]
        public Guid ID { get; set; }

        [Required]
        [StringLength(50)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        public virtual ICollection<UserGroup> UserGroup { get; set; }
     }
}
