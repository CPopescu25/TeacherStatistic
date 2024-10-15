using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Client.Interfaces;
using TS.DTO;

namespace Client.Controllers
{
    public class UniversitiesController : BaseController
    {
        public readonly IClient _client;
        public Data Data;
      
        #region Constructor
        public UniversitiesController()
            : base(url)
        {
            _client = new ClientRequest();
            Data = new Data();

        }
        #endregion

        #region Methods
        // GET: Location
        public ActionResult Index()
        {
            if (Session["Username"] != null)
            {
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/universities/index");
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


        // POST: Location/Create
        [HttpPost]
        public ActionResult Create()
        {
            if (Session["Username"] != null)
            {
                JsTreeFormatDTO model = new JsTreeFormatDTO();

                string parent = " ";
                string name = Request.Form["text"];
                string p = Request.Form["parent"];
                if (p != null)
                {
                    parent = Request.Form["parent"];
                }

                model.text = name;
                model.parent = parent;

                Data.Object = model;
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/universities/create");
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


        // POST: Location/Edit/5
        [HttpPost]
        public ActionResult Edit(int id)
        {
            if (Session["Username"] != null)
            {
                JsTreeFormatDTO model = new JsTreeFormatDTO();
                string parent = " ";
                string name = Request.Form["text"];
                string p = Request.Form["parent"];
                if (p != " ")
                {
                    parent = Request.Form["parent"];
                }

                model.text = name;
                model.parent = parent;
                model.id = id.ToString();

                Data.Object = model;
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/universities/update");
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

        // POST: Location/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (Session["Username"] != null)
            {
                Data.Object = id;
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/universities/delete");
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