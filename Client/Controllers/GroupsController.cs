
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Client.Interfaces;
using TS.DTO;

namespace Client.Controllers
{
    public class GroupsController : BaseController
    {
        public readonly IClient _client;
        public Data Data;
      
        #region Constructor
        public GroupsController()
            : base(url)
        {
            _client = new ClientRequest();
            Data = new Data();

         }
        #endregion

        #region Methods
        //display create group view only if user has permission
        public ActionResult Create()
        {
            return View();
        }

        // POST: Group/Create
        [HttpPost]
        public ActionResult Create(GroupHelpDTO groups)
        {
            if (Session["Username"] != null)
            {
                groups.Id = Guid.NewGuid();
                Data.Object = groups;
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();
               
                var o = _client.PostAsync(url, Data, "api/groups/postgroups");
                if (o == null)
                    Data.Message = Message;
                else
                    Data = JsonConvert.DeserializeObject<Data>(o);

               return Json(Data);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        //display all groups
        public ActionResult Index()
        {
            if (Session["Username"] != null)
            {
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/groups/allgroups");
                if (o == null)
                    Data.Message = Message;
                else
                    Data = JsonConvert.DeserializeObject<Data>(o);
               return View(Data);
                   
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //display the partial view details for edit a group
        [HttpGet]
        public ActionResult PartialEdit(string id)
        {
            if (Session["Username"] != null)
            {
               Data.Object = new Guid(id);
               Data.UserId = new Guid(Session["UserId"].ToString());
               Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/groups/getgroup");
                if (o == null)
                    Data.Message = Message;
                else
                    Data = JsonConvert.DeserializeObject<Data>(o);

                return PartialView(Data);


            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //edit a group
        [HttpPost]
        public ActionResult Edit(GroupHelpDTO group)
        {
            if (Session["Username"] != null)
            {
                Data.Object = group;
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                try
                {
                    var o = _client.PostAsync(url, Data, "api/groups/editgroup");
                    if (o == null)
                        Data.Message = Message;
                    else
                        Data = JsonConvert.DeserializeObject<Data>(o);
                      

                }
                catch
                {
                    Data.Ok = false;
                    Data.Message = "An error occured!Please ask for support!";
                }
                return Json(Data);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //delete a group
        [HttpPost]
        public ActionResult Delete(string id)
        {

            if (Session["Username"] != null)
            {
                Data.Object = new Guid(id);
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/groups/delete");
                if (o == null)
                    Data.Message = Message;
                else
                    Data = JsonConvert.DeserializeObject<Data>(o);
                return Json(Data);
                
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        #endregion
    }
}