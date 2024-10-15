using Client.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TS.Data.Context.Models;
using TS.DTO;
using TS.DTO.Enums;

namespace Client.Controllers
{
    public class AnswerController : BaseController
    {
        public readonly IClient _client;
        public Data Data;
        
        #region Constructor
        public AnswerController()
            : base(url)
        {
            _client = new ClientRequest();
            Data = new Data();

         }
        #endregion

        #region Methods
        //method login when a user(student) answer at a questionnaire
        //we have questionnaire id and the type of questionnaire
        public ActionResult Login(string id, SetType setType)
        {
            if (Session["Username"] == null)
            {
                return View();
            }
            else
            {
                //if user is logged, display the answer view(questionnaire with questions and type of answers)
                return RedirectToAction("Answer", new
                {
                    id = id,
                    setType = setType

                });
            }
        }

        //the view with questions and answers for a questionnaire
        //we have questionnaire id and the type of questionnaire
        public ActionResult Answer(string id, SetType SetType)
        {
            if (Session["Username"] != null)
            {
                StatisticHelpDTO data = new StatisticHelpDTO();
                data.Id = new Guid(id);
                data.SetType = SetType;

                Data.Object = data;
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();
               
                var o = _client.PostAsync(url, Data, "api/statistics/details");
                if (o == null)
                    Data.Message = Message;
                else
                    Data = JsonConvert.DeserializeObject<Data>(o);

                return View(Data);

            }
            else
            {
                return View();
            }

        }

        //login method for login and answer views
        //we have questionnaire id and the type of questionnaire
        //and the username and password
        [HttpPost]
        public ActionResult LogIn(User model, string id, SetType setType)
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

                    //the username, the password and the user id are retained in the session
                    //when a user logs into the application 
                    Session["Username"] = l.UserName;
                    Session["Password"] = l.Password;
                    Session["UserId"] = l.Id;

                    //the expiration time of the current session
                    Session.Timeout = 60;

                    return RedirectToAction("Login", new
                    {
                        id = id,
                        setType = setType

                    });

                }
                else
                {
                    return Json(Data);
                }
     
            }
            else
            {
                return RedirectToAction("LogOut", "Home");
            }

        }

        //save user responses to the questionnaire involved
        [HttpPost]
        public ActionResult Answer(AnswersHelpDTO data)
        {

            if (Session["Username"] != null)
            {
                
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();
                data.User_who_answer_Id = Data.UserId;
                Data.Object = data;

                var o = _client.PostAsync(url, Data, "api/answers/answer");
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

        //get the answers for an user knowing the type of questionnaire
        [HttpGet]
        public ActionResult FirstSetAnswersUser(string id, SetType SetType)
        {
            if (Session["Username"] != null)
            {
                AnswersHelpDTO data = new AnswersHelpDTO();
                data.User_Id = new Guid(id);
                data.SetType = SetType;

                Data.Object = data;
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();
                
                var o = _client.PostAsync(url, Data, "api/answers/getemployeeanswers");
                if (o == null)
                    Data.Message = Message;
                else
                Data = JsonConvert.DeserializeObject<Data>(o);

                if (Data.Ok == true)
                {
                    data = Data.Object.ToObject<AnswersHelpDTO>();
                    if (data.List_Statistics.Count != 0)
                    {
                        return View(Data);
                    }
                    else
                    {
                        Data.Message = "This user has no assigned answers!";
                        return View(Data);

                    }
                  }
                else
                {
                    return View(Data);
                }
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

                var o = _client.PostAsync(url, Data, "api/answers/delete");
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