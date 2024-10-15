using Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Client.Interfaces;
using TS.DTO;
using TS.DTO.Enums;
using TS.Data.Context.Models;

namespace Client.Controllers
{
    public class QuestionnaireController : BaseController
    {
        public readonly IEmail _email;
        public readonly IClient _client;
        public Data Data;
        public readonly IEmailTemplate _template;
      
        #region Constructor
        public QuestionnaireController()
            : base(url)
        {
            _client = new ClientRequest();
            Data = new Data();
            _email = new Email();
            _template = new EmailTemplate();
        }
        #endregion

        #region Methods
        // GET: Questionnaire
        //display create user if the current user have permission
        public ActionResult Create()
        {
            return View(Data);
        }

        [HttpPost]
        public ActionResult Create(Statistics data, List<StudentsTargetsHelpDTO> st, string user, string host)
        {
            if (Session["Username"] != null)
            {
                data.AddDate = DateTime.Now;
                data.ID = Guid.NewGuid();
                Data.Object = data;
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                SetType setType = (SetType)data.SetType;

                var o = _client.PostAsync(url, Data, "api/statistics/create");
                if (o == null)
                    Data.Message = Message;
                else
                    Data = JsonConvert.DeserializeObject<Data>(o);
                
                if(Data.Ok==true)
                    Data = SendEmail(st, user, data.ID, host, setType);

                return Json(Data);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //get the list of questionnaires for FirstSet
        public ActionResult Index()
        {
            if (Session["Username"] != null)
            {
                Data.Object = Convert.ToInt32(SetType.FirstSet);
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();
                
                var o = _client.PostAsync(url, Data, "api/statistics/index");
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

        //Get the list of questionnaires by teacher id
        public ActionResult TeacherQuestionnaires()
        {
            if (Session["Username"] != null)
            {
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/statistics/teacherquestionnaires");
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

        public ActionResult QuestionnaireWithQuestionsAndAnswers()
        {
            if (Session["Username"] != null)
            {
               // Server.Data = Convert.ToInt32(SetType.FirstSet);
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/answers/getquestionnaireqnswersbystudentid");
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

        //get the list of questionnaires for SecondSet
        public ActionResult IndexSecond()
        {
            if (Session["Username"] != null)
            {
                Data.Object = Convert.ToInt32(SetType.SecondSet);
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/statistics/index");
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

        [HttpPost]
        public ActionResult Delete(string id)
        {
            if (Session["Username"] != null)
            {
                Data.Object = new Guid(id);
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/statistics/delete");
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

        [HttpPost]
        public ActionResult ExportByTeacherId(string id)
        {
            if (Session["Username"] != null)
            {
                Data.Object = new Guid(id);
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/install/exportteacher");
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

       public Data SendEmail(List<StudentsTargetsHelpDTO> st, string user, Guid id, string host, SetType SetType)
        {
            try
                    {
                        //for each student involved in this questionniare
                        //send an email 
                        foreach (StudentsTargetsHelpDTO s in st)
                        {
                            Emailmodel model = new Emailmodel();
                            model.Body = _template.BodySendEmail(s.User_Name,user,host,id,SetType);
                            model.Subject =_template.SubjectSendEmail() ;
                            model.toAddress = s.Email;
                            model.Username = s.User_Name;

                            _email.SendEmail(model);
                      }
                        if (st.Count() == 1)
                        {
                            Data.Message = "The email was successfully sent!";
                        }
                        else
                        {
                            Data.Message = "Emails have been sent successfully!";
                        }
                        return Data;
                    }
                    catch (Exception)
                    {
                        Data.Ok = false;
                        Data.Message = "An error occured!Please ask for support!";
                        return Data;
                    }
          }
        #endregion
    }
}