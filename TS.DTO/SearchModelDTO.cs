using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.DTO.Enums;

namespace TS.DTO
{
    public class SearchModelDTO
    {
        public List<string> ids { get; set; }
        public List<int> ids_int { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Year Year { get; set; }
        public string Group { get; set; }
        public int GetAll { get; set; }
    }
}
