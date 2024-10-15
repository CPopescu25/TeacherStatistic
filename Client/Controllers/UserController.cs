using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Client.Interfaces;
using Client.Models;
using TS.DTO;
using TS.DTO.Enums;
using TS.Data.Context.Models;

namespace Client.Controllers
{
    public class UserController : BaseController
    {
        public readonly IClient _client;
        public Data Data;
        public readonly IEmail _email;
        public readonly IEmailTemplate _template;
      
        #region Constructor
        public UserController()
            : base(url)
        {
            _client = new ClientRequest();
            Data = new Data();
            _email = new Email();
            _template = new EmailTemplate();
         }
        #endregion

        #region Methods
        // GET: User
        //display the create user view only if the current user have permission
        public ActionResult Create()
        {
            return View();
        }

        //generate password for a new user, knowing firstname,lastname and cnp
        private string GeneratePassword(string f, string l)
        {
            int length = 10;

            Random r = new Random();

            string fm = f.Substring(0, 3);
            string lm = l.Substring(0, 3);
            string c = "1234567890";

            string pass = fm + lm + c;

            char[] chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                chars[i] = pass[(int)((pass.Length) * r.NextDouble())];
            }

            return new string(chars);
        }

        //create new user
        [HttpPost]
        public ActionResult Create(User data, string host)
        {
            if (Session["Username"] != null)
            {                
                //check if email is valid
                    if (isEmailValid(data.Email))
                    {
                        data.UserName = data.FirstName + "." + data.LastName;
                        data.Password = GeneratePassword(data.FirstName, data.LastName);
                    data.ID = Guid.NewGuid();

                        Data.Object = data;
                        Data.UserId = new Guid(Session["UserId"].ToString());
                        Data.Username = Session["Username"].ToString();

                        var o = _client.PostAsync(url, Data, "api/users/postuser");
                        if (o == null)
                            Data.Message = Message;
                        else
                        Data = JsonConvert.DeserializeObject<Data>(o);

                        if (Data.Ok == true)
                        {
                           //when a new user is created, he recive an email with his username and password to login in this application
                            Emailmodel model = new Emailmodel();
                            model.Body = _template.BodyUserCreate(data.FirstName+" "+data.LastName,data.UserName,data.Password,host);
                            model.Subject = _template.SubjectUserCreate();
                            model.toAddress =data.Email;
                            model.Username = data.UserName;

                            _email.SendEmail(model);
                        }
                     }
                    else
                    {
                        Data.Ok = false;
                        Data.Message = "Please insert a valid email!";
                    }
                return Json(Data);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //display the list of users from admin group
        public ActionResult IndexAdmins()
        {
            return View();
        }

        //display the list of users from theacher group
        public ActionResult IndexTeachers()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchTeachers(DataSourceRequest command, List<int> data,string FirstName,string LastName,int GetAll)
        {
            if (Session["Username"] != null)
            {
                SearchModelDTO modelSearch = new SearchModelDTO();
                modelSearch.FirstName = FirstName;
                modelSearch.LastName = LastName;
                modelSearch.Group = "Teacher";
                if (data[0]==0)
                {
                    modelSearch.ids_int = null;
                }
                else
                {
                    modelSearch.ids_int = new List<int>();
                    foreach(var id in data)
                    {
                        modelSearch.ids_int.Add(id);
                    }
                }
               
                modelSearch.GetAll = GetAll;

                Data.Object = modelSearch;
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();
            
                var o = _client.PostAsync(url, Data, "api/users/index");
                if (o == null)
                    Data.Message = Message;
                else
                    Data = JsonConvert.DeserializeObject<Data>(o);

                List<UserHelpDTO> list = new List<UserHelpDTO>();
                var gridModel = new DataSourceResult();

                if (Data.Object != null)
                {
                    list = Data.Object.ToObject<List<UserHelpDTO>>();
                    gridModel.Data = list;
                    gridModel.Total = list.Count();
                }
                else
                {
                    gridModel.Data = list;
                    gridModel.Total = 0;
                }

                var jsonResult = Json(gridModel, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult SearchAdmins(DataSourceRequest command, string FirstName, string LastName, int GetAll)
        {
            if (Session["Username"] != null)
            {
                SearchModelDTO modelSearch = new SearchModelDTO();
                modelSearch.FirstName = FirstName;
                modelSearch.LastName = LastName;
                modelSearch.Group = "Admin";
                modelSearch.GetAll = GetAll;
                modelSearch.ids = null;
                modelSearch.ids_int = null;

                Data.Object = modelSearch;
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/users/index");
                if (o == null)
                    Data.Message = Message;
                else
                    Data = JsonConvert.DeserializeObject<Data>(o);

                List<UserHelpDTO> list = new List<UserHelpDTO>();
                var gridModel = new DataSourceResult();

                if (Data.Object != null)
                {
                    list = Data.Object.ToObject<List<UserHelpDTO>>();
                    gridModel.Data = list;
                    gridModel.Total = list.Count();
                }
                else
                {
                    gridModel.Data = list;
                    gridModel.Total = 0;
                }

                var jsonResult = Json(gridModel, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult SearchStudents(DataSourceRequest command,List<int> data, string FirstName, string LastName, int GetAll,int year)
        {
            if (Session["Username"] != null)
            {
                SearchModelDTO modelSearch = new SearchModelDTO();
                modelSearch.FirstName = FirstName;
                modelSearch.LastName = LastName;
                modelSearch.Group = "Student";
                modelSearch.GetAll = GetAll;
                if (data[0]==0)
                {
                    modelSearch.ids_int = null;
                }
                else
                {
                    modelSearch.ids_int = new List<int>();
                    foreach(var id in data)
                    {
                        modelSearch.ids_int.Add(id);
                    }
                }
                if (year != 0)
                    modelSearch.Year = (Year)year;

                Data.Object = modelSearch;
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/users/index");
                if (o == null)
                    Data.Message = Message;
                else
                    Data = JsonConvert.DeserializeObject<Data>(o);

                List<UserHelpDTO> list = new List<UserHelpDTO>();
                var gridModel = new DataSourceResult();

                if (Data.Object != null)
                {
                    list = Data.Object.ToObject<List<UserHelpDTO>>();
                    gridModel.Data = list;
                    gridModel.Total = list.Count();
                }
                else
                {
                    gridModel.Data = list;
                    gridModel.Total = 0;
                }

                var jsonResult = Json(gridModel, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

 //display the list of users from student group
        public ActionResult IndexStudents()
        {
            return View();
        }

        //display the details of an user
        public ActionResult Edit(string id)
        {
            if (Session["Username"] != null)
            {
                Data.Object = new Guid(id);
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/users/getdetails");
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

        //edit a user
        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (Session["Username"] != null)
            {
                Data.Object = user;
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                  //verify if the email address is valid
                    if ((user.Email != "" && isEmailValid(user.Email)))
                    {
                        var o = _client.PostAsync(url, Data, "api/users/edit");
                        if (o == null)
                            Data.Message = Message;
                        else
                        Data = JsonConvert.DeserializeObject<Data>(o);
                            
                    }
                    else
                    {
                        Data.Ok = false;
                        Data.Message = "Please insert a valid email!";
                    }

                return Json(Data);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //delete a user
        [HttpPost]
        public ActionResult Delete(string id)
        {
            if (Session["Username"] != null)
            {
                Data.Object = new Guid(id);
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/users/delete");
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

        //display the view for change password only if the current user have permission
        [HttpGet]
        public ActionResult ChangePassword()
        {
           return View();
        }

        //display the view when a user forgot his password
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //change password method
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordDTO data)
        {
            if (Session["Username"] != null)
            {
                Data.Object = data;
                Data.UserId = new Guid(Session["UserId"].ToString());
                Data.Username = Session["Username"].ToString();

                var o = _client.PostAsync(url, Data, "api/users/changepassword");
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

        //forgot password method
        [HttpPost]
        public ActionResult ForgotPassword(ChangePasswordDTO data, string host)
        {
            Data.Object = data;
           // Data.UserId = new Guid(Session["UserId"].ToString());
         
            var o = _client.PostAsync(url, Data, "api/users/forgotpassword");
            if (o == null)
                Data.Message = Message;
            else
                Data = JsonConvert.DeserializeObject<Data>(o);

                    if (Data.Ok == true)
                    {
                        UserHelpDTO model = Data.Object.ToObject<UserHelpDTO>();

                        //Send an email to the user with his password from database
                        Emailmodel model2 = new Emailmodel();
                        model2.Body = _template.BodyForgotPassword(model.UserName, model.Password, host);

                        model2.Subject = _template.SubjectForgotPassword();
                        model2.toAddress = data.Email;
                        model2.Username = model.UserName;

                        _email.SendEmail(model2);

                        return Json(Data);
                    }
                    else
                    {

                        return Json(Data);
                    }

              }
        //check if an email is valid
        private bool isEmailValid(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //check if a cnp is valid
        private bool isCnpValid(string pnc)
        {
            //var pt suma
            long suma = 0;
            //codul pt cifra de control
            const string n = "279146358279";
            //long to string
            string w = pnc;
            //verificare lungime
            if (w.Length != 13)
                return false;
            //verficare cod de control
            for (int i = 0; i < 12; i++)
                suma += long.Parse(w.Substring(i, 1)) * long.Parse(n.Substring(i, 1));
            long rest = suma - 11 * (int)(suma / 11);
            if (long.Parse(w.Substring(12, 1)) != rest)
                return false;
            //verificare judet
            long judet = long.Parse(w.Substring(7, 2));
            if (judet > 0 && judet < 47 && judet > 50 && judet < 53)
                return false;
            //verificare data
            string zi = w.Substring(5, 2);
            string luna = w.Substring(3, 2);
            string anRaw = w.Substring(1, 2);
            string an = "";
            if (w.Substring(0, 1) == "1" || w.Substring(0, 1) == "2")
                an += "19" + anRaw;
            if (w.Substring(0, 1) == "6" || w.Substring(0, 1) == "5")
                an += "20" + anRaw;
            if (w.Substring(0, 1) == "3")
                return false;
            string BiDa = zi + "/" + luna + "/" + an;
            Regex ZiNa = new Regex(@"^(?:(?:31(\/)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$");
            if (!ZiNa.IsMatch(BiDa))
                return false;
            //cod pt strainatate
            string[] strain = { "7", "8", "9" };
            foreach (string xs in strain)
            {
                if (xs.Contains(w.Substring(0, 1)))
                {
                    string strain1 = zi + "/" + luna + "/19" + anRaw;
                    if (!ZiNa.IsMatch(strain1))
                    {
                        strain1 = zi + "/" + luna + "/20" + anRaw;
                        if (!ZiNa.IsMatch(strain1))
                            return false;
                    }
                }
            }
            //verificare cod
            string cod = w.Substring(9, 3);
            if (cod.Equals("000"))
                return false;
            return true;
        }
        #endregion
    }
}