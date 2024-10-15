using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Data.Context.Models;

namespace TS.DTO
{
    public class Data
    {
        public dynamic Object { get; set; }

        public string Message { get; set; }

        public bool Ok { get; set; }

        public Guid UserId{ get; set; }

        public string Username { get; set; }

        public string GroupName { get; set; }
    }
}
