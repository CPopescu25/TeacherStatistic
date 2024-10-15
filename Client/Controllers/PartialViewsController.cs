using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Client.Interfaces;
using TS.DTO;
using TS.DTO.Enums;
using CommonCfg;

namespace Client.Controllers
{
    public class PartialViewsController : BaseController
    {
        public readonly IClient _client;
        public Data Data;

        #region Constructor
        public PartialViewsController()
            : base(url)
        {
            _client = new ClientRequest();
            Data = new Data();

        }
        #endregion

        #region Methods
       //dsiplay universities list
        public ActionResult PartialListUniversity()
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
                return PartialView(Data);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        

        //display the groups list
        public ActionResult PartialListGroup()
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
                return PartialView(Data);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        //display the students list
        public ActionResult PartialListStudents()
        {
            if (Session["Username"] != null)
            {
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/users/liststudents");
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

        //display the teachers list
        public ActionResult PartialListTeachers()
        {

            if (Session["Username"] != null)
            {
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/users/listteachers");
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

        //display the questions list knowing the type of questionnaire
        public ActionResult PartialMultiListQuestions(SetType setType)
        {
            if (Session["Username"] != null)
            {
                Data.Object = Convert.ToInt32(setType);
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

        //display the types of questionnaires
        public ActionResult PartialSetType()
        {
            if (Session["Username"] != null)
            {
                Dictionary<string, string> model = new Dictionary<string, string>();

                var a = EnumHelper.GetDescription(SetType.FirstSet);
                model.Add(a, ((int)SetType.FirstSet).ToString());

                var b = EnumHelper.GetDescription(SetType.SecondSet);
                model.Add(b, ((int)SetType.SecondSet).ToString());

                var c = EnumHelper.GetDescription(SetType.ThirdSet);
                model.Add(c, ((int)SetType.ThirdSet).ToString());

                return PartialView(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //display the list of study years 
        public ActionResult PartialYear()
        {
            if (Session["Username"] != null)
            {
                Dictionary<string, string> model = new Dictionary<string, string>();

                foreach (var value in Enum.GetValues(typeof(Year)))
                {
                    model.Add(Enum.GetName(typeof(Year), value), ((int)value).ToString());
                }

                return PartialView(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //display the image for current user logged
        public ActionResult PartialImage()
        {
            if (Session["Username"] != null)
            {
                Guid id = new Guid(Session["UserId"].ToString());
                UserHelpDTO model = new UserHelpDTO();
               
                model = JsonConvert.DeserializeObject<UserHelpDTO>(
                    _client.GetAsync(url, "api/users/getimageuser/" + id));
                return PartialView(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        //the method for upload a new picture for current user
        [HttpPost]
        public ActionResult Upload(string id)
        {
            if (Session["Username"] != null)
            {
                UserHelpDTO model = new UserHelpDTO();
                model.Id = new Guid(Session["UserId"].ToString());

                foreach (string file in Request.Files)
                {

                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        var stream = fileContent.InputStream;

                        using (MemoryStream ms = new MemoryStream())
                        {

                            stream.CopyTo(ms);
                            model.Image = ms.GetBuffer();
                        }
                    }
                }
                Data.Object = model;
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/users/editimage");
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

        //display the rights for the current user
        public ActionResult PartialMenu()
        {
            if (Session["Username"] != null)
            {
                Data.UserId = new Guid(Session["UserId"].ToString());

                var o =_client.PostAsync(url, Data, "api/groups/getuserformenu");

                if (o == null)
                    Data.Message = Message;
                else
                    Data = JsonConvert.DeserializeObject<Data>(o);

                GroupHelpDTO groupHelpDTO = Data.Object.ToObject<GroupHelpDTO>();

                return PartialView(groupHelpDTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
    }
}