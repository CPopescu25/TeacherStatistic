using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.Core.Api.Data
{
    /// <summary>
    /// Handles the request header content
    /// </summary>
    public class HeaderContent
    {
        /// <summary>
        /// The content of the request header 
        /// </summary>
        public Dictionary<string, string> Content { get; set; }

    }
}
