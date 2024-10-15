using Client.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TS.Data.Context.Models;
using TS.DTO;

namespace Client.Controllers
{
    public class HomeController : BaseController
    {
        public readonly IClient _client;
        public Data Data;
      
        #region Constructor
        public HomeController()
            : base(url)
        {
            _client = new ClientRequest();
            Data = new Data();

        }
        #endregion

        #region Methods
        public ActionResult Index()
        {
            if (Session["Username"] == null)
            {

                //find if exist any user in database
                var o = _client.GetAsync(url, "api/install/getdatasfromdatabse");

                if (o == null)
                    Data.Message = Message;
                else
                    Data = JsonConvert.DeserializeObject<Data>(o);

                if (Data.Message == "Null")
                {
                    //if there is no user, then display the install datas view
                    return RedirectToAction("InstallDatas");
                }
                else
                    return View();
            }
            else
            {
                return RedirectToAction("About");
            }
        }

        //login user in application
        [HttpPost]
        public ActionResult LogIn(User model)
        {
            if (Session["Username"] == null)
            {
               Data.Object = model;
               var o = _client.PostAsync(url, Data, "api/login/logins");
               if (o == null)
                   Data.Message = Message;
                else
               Data = JsonConvert.DeserializeObject<Data>(o);

               if (Data.Ok == true)
               {
                   UserHelpDTO l = Data.Object.ToObject<UserHelpDTO>();
                   Session["Username"] = l.UserName;
                   Session["Password"] = l.Password;
                   Session["UserId"] = l.Id;
                   Session.Timeout = 60;
               }
               return Json(Data);
            }
            else
            {
                return RedirectToAction("LogOut");
            }

        }

        //logout view
        public ActionResult LogOut()
        {
            if (Session["Username"] != null)
            {
                Session.Abandon();
                Session.Remove("Username");
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        public ActionResult About()
        {
            if (Session["Username"] != null)
            {
                ViewBag.Message = "Application description page.";

                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public JsonResult GetTeachersByUniversity(List<int> data)
        {
            SearchModelDTO modelSearch = new SearchModelDTO();
            if (data[0] == 0)
            {
                modelSearch.ids_int = null;
            }
            else
            {
                modelSearch.ids_int = new List<int>();
                foreach (var id in data)
                {
                    modelSearch.ids_int.Add(id);
                }
            }

            Data.Object = modelSearch;

            List<TeachersStatisticsDTO> list = new List<TeachersStatisticsDTO>();

            var o = _client.PostAsync(url, Data, "api/statistics/getteachersstatisticsbyuniversity");
            
            list = JsonConvert.DeserializeObject<List<TeachersStatisticsDTO>>(o);

            return Json(list);
        }

        //if there is no user in database, display this view, else display the login view
        public ActionResult InstallDatas()
        {
            string mesage = "";

            var o = _client.GetAsync(url, "api/install/getdatasfromdatabse");

                if (o == null)
                    Data.Message = Message;
                else
                    Data = JsonConvert.DeserializeObject<Data>(o);

            mesage = Data.Message;

            if (mesage == "Null")
            {
                return View();
            }
            else if (mesage == "Exist")
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        //method for install settings in database at first use of the application
        [HttpPost]
        public ActionResult Install()
        {
            Data = JsonConvert.DeserializeObject<Data>(_client.GetAsync(url, "api/install/installdatabase"));

            return Json(Data);
        }
        #endregion
    }
}