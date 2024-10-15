using BusinessServices.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TS.Data.Context.Models;
using TS.DTO;

namespace TS.Business.Api.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        #region Fields
        private readonly IUserLogic _userLogic;

        public DTO.Data Data;
        #endregion

        #region Constructor
        public UsersController(IUserLogic userLogic)
        {
            _userLogic = userLogic;

            Data = new DTO.Data();
        }
        #endregion

        #region Public Interface

        [Route("getimageuser/{userId:Guid}")]
        [HttpGet]
        public IHttpActionResult GetImageUser(Guid userId)
        {
            UserHelpDTO model = new UserHelpDTO();
            model = _userLogic.GetImageUser(userId);

            return Ok(model);
        }

        [Route("postuser")]
        [HttpPost]
        public async Task<IHttpActionResult> Create(TS.DTO.Data data)
        {
            User user = data.Object.ToObject<User>();

            Data = await _userLogic.Create(user);

            return Ok(Data);
        }

        [Route("edit")]
        [HttpPost]
        public IHttpActionResult Edit(TS.DTO.Data data)
        {
            User user = data.Object.ToObject<User>();

            Data = _userLogic.Edit(user);

            return Ok(Data);
        }

        [Route("index")]
        [HttpPost]
        public IHttpActionResult Index(TS.DTO.Data data)
        {
            //the group name
            SearchModelDTO searchModelDTO = data.Object.ToObject<SearchModelDTO>();

            Data = _userLogic.Index(searchModelDTO);

            return Ok(Data);
        }

        [Route("liststudents")]
        [HttpPost]
        public IHttpActionResult ListStudents()
        {
            Data = _userLogic.ListStudents();

            return Ok(Data);
        }

        [Route("listteachers")]
        [HttpPost]
        public IHttpActionResult ListTeachers()
        {
            Data = _userLogic.ListTeachers();

            return Ok(Data);
        }

        [Route("getdetails")]
        [HttpPost]
        public IHttpActionResult GetDetails(TS.DTO.Data data)
        {
            Guid id = new Guid(data.Object);

            Data = _userLogic.GetDetails(id);

            return Ok(Data);
        }

        [Route("delete")]
        [HttpPost]
        public IHttpActionResult Delete(TS.DTO.Data data)
        {
            Guid id = new Guid(data.Object);

            Data = _userLogic.Delete(id);

            return Ok(Data);
        }

        [Route("editimage")]
        [HttpPost]
        public IHttpActionResult EditImage(TS.DTO.Data data)
        {
            User user = data.Object.ToObject<User>();

            Data = _userLogic.EditImage(user);

            return Ok(Data);
        }

        [Route("changepassword")]
        [HttpPost]
        public IHttpActionResult ChangePassword(TS.DTO.Data data)
        {
            ChangePasswordDTO model = data.Object.ToObject<ChangePasswordDTO>();

            Data = _userLogic.ChangePassword(model);

            return Ok(Data);
        }

        [Route("forgotpassword")]
        [HttpPost]
        public IHttpActionResult ForgotPassword(TS.DTO.Data data)
        {
            ChangePasswordDTO model = data.Object.ToObject<ChangePasswordDTO>();

            Data = _userLogic.ForgotPassword(model);

            return Ok(Data);
        }
        #endregion
    }
}
