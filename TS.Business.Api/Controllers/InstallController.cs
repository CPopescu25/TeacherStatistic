using BusinessServices.Install;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TS.Resources.Services;

namespace TS.Business.Api.Controllers
{
    [RoutePrefix("api/install")]
    public class InstallController : ApiController
    {
        #region Fields
        private readonly IInstallLogic _installLogic;
        private readonly IResourcesLoader _resourcesLoader;

        public DTO.Data Data;
        #endregion

        #region Constructor
        public InstallController(IInstallLogic installLogic,
          IResourcesLoader resourcesLoader)
        {
            _installLogic = installLogic;
            _resourcesLoader = resourcesLoader;

            Data = new DTO.Data();
        }
        #endregion

        #region Public Interface
        [Route("getdatasfromdatabse")]
        [HttpGet]
        public IHttpActionResult GetDatasFromDatabase()
        {
            Data = _installLogic.CkeckDatasInDatabase();

            return Ok(Data);
        }

        [Route("installdatabase")]
        [HttpGet]
        public IHttpActionResult Install()
        {
            bool ok = false;

            ok = _installLogic.Questions();

            if (ok) ok = _installLogic.Group();
            if (ok) ok = _installLogic.Admin();
            if (ok) ok = _installLogic.Universities();

            if (ok)
            {
                Data.Message = _resourcesLoader.GetStringResourceByName("install_success");
                Data.Ok = ok;
            }
            else
            {
                Data.Message = _resourcesLoader.GetStringResourceByName("install_failure");
                Data.Ok = ok;
            }

            return Ok(Data);
        }

        [Route("exportteacher")]
        [HttpPost]
        public IHttpActionResult ExportByTeacherId(TS.DTO.Data data)
        {
            Guid id = new Guid(data.Object);

            Data = _installLogic.ExportTeacher(id);

            return Ok(Data);
        }
        #endregion

    }
}
