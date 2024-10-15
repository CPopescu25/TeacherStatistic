using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Data.Context.Models;

namespace TS.DTO
{
    public class GroupHelpDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<UserGroup> UserGroup { get; set; }
    }
}
