using BusinessServices.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TS.Data.Context.Models;
using TS.DTO;

namespace TS.Business.Api.Controllers
{
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        #region Fields
        private readonly ILoginLogic _loginLogic;
        #endregion

        #region Constructor
        public LoginController(ILoginLogic loginLogic)
        {
            _loginLogic = loginLogic;
        }
        #endregion

        #region Public Interface

        [Route("logins")]
        [HttpPost]
        public IHttpActionResult Authenticate(TS.DTO.Data data)
        {
            User user= data.Object.ToObject<User>();

            UserHelpDTO userHelpDTO = new UserHelpDTO();
            userHelpDTO.UserName = user.UserName;
            userHelpDTO.Password = user.Password;

            data = _loginLogic.Login(userHelpDTO);

            return Ok(data);
        }

        #endregion

    }
}
