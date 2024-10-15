using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class Emailmodel
    {
        public string Subject { get; set; }
        public string Username { get; set; }
        public string toAddress { get; set; }
        public string Body { get; set; }
    }
}