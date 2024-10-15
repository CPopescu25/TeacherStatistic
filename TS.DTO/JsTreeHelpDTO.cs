using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.DTO
{
    public class JsTreeHelpDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Parent { get; set; }

        public string ListParent { get; set; }
    }
}
