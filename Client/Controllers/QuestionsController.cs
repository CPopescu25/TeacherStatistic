using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Client.Interfaces;
using TS.DTO;
using TS.DTO.Enums;

namespace Client.Controllers
{
    public class QuestionsController : BaseController
    {
        public readonly IClient _client;
        public Data Data;
      
        #region Constructor
        public QuestionsController()
            : base(url)
        {
            _client = new ClientRequest();
            Data = new Data();

        }
        #endregion

        #region Methods
        // GET: Questions
        //display the questions for first type questionnaire
        public ActionResult Index()
        {
            if (Session["Username"] != null)
            {
                Data.Object = Convert.ToInt32(SetType.FirstSet);
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/questions/index");
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

        //display the questions for second type questionnaire
        public ActionResult PartialSecondIndex()
        {
            if (Session["Username"] != null)
            {
                Data.Object = Convert.ToInt32(SetType.SecondSet);
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/questions/index");
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

        //display details for the selected  question
        public ActionResult PartialEdit(string id)
        {
            if (Session["Username"] != null)
            {
                Data.Object = new Guid(id);
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/questions/getquestions");
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

        //edit a question
        [HttpPost]
        public ActionResult Edit(QuestionsHelpDTO q)
        {
            if (Session["Username"] != null)
            {
                Data.Object = q;
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/questions/edit");
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

        //if current user have permission, then display the partial view for create a new question
        public ActionResult PartialCreate()
        {
            return PartialView();
        }

        //create a question
        [HttpPost]
        public ActionResult Create(QuestionsHelpDTO q)
        {
            if (Session["Username"] != null)
            {
                q.Id = Guid.NewGuid();
                Data.Object = q;
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/questions/create");
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

        //delete a question
        [HttpPost]
        public ActionResult Delete(string id)
        {

            if (Session["Username"] != null)
            {
                Data.Object = new Guid(id);
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/questions/delete");
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