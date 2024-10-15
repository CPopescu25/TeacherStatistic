using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers
{
    public abstract class BaseController : Controller
    {
        private string url1;

        public const string Message = "An error occured!Please ask for support!";

        public BaseController(string url1)
        {
            // TODO: Complete member initialization
            this.url1 = url1;
        }
        public static string url
        {
            get { return ConfigurationManager.AppSettings["ClientId"].ToString(); }
        }

       
    }
}