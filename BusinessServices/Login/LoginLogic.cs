using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Data.Context;
using TS.DTO;
using TS.Resources.Services;

namespace BusinessServices.Login
{
    public class LoginLogic : ILoginLogic
    {
        #region Fields
        private readonly TSClientContext _dbContext;
        private readonly IResourcesLoader _resourcesLoader;

        public Data Data;
        #endregion

        #region Constructor
        public LoginLogic(TSClientContext dbContext,
          IResourcesLoader resourcesLoader)
        {
            _dbContext = dbContext;
            _resourcesLoader = resourcesLoader;

            Data = new Data();
            Data.Object = null;
        }
        #endregion

        #region ILoginLogic implementation
        public Data Login(UserHelpDTO userHelpDTO)
        {
            var display = _dbContext.Users
                 .Where(u => u.UserName == userHelpDTO.UserName 
                 && u.EncryptedPassword == userHelpDTO.EncryptedPassword 
                 && u.Enable == true)
                 .Select(u => new UserHelpDTO
                 {
                     UserName = u.UserName,
                     EncryptedPassword = u.EncryptedPassword,
                     Image = u.Image,
                     Id = u.ID
                 }).FirstOrDefault();


            if (display != null)
            {

                Data.Ok = true;
                Data.Message = _resourcesLoader.GetStringResourceByName("login_success");
                Data.Object = display;

            }
            else
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("login_error");
            }
            return Data;
        }

        

        #endregion
    }
}
