using BusinessServices.Universities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TS.DTO;

namespace TS.Business.Api.Controllers
{
    [RoutePrefix("api/universities")]
    public class UniversitiesController : ApiController
    {
        #region Fields
        private readonly IUniversitiesLogic _universitiesLogic;

        public DTO.Data Data;
        #endregion

        #region Constructor
        public UniversitiesController(IUniversitiesLogic universitiesLogic)
        {
            _universitiesLogic = universitiesLogic;

            Data = new DTO.Data();
        }
        #endregion

        #region Public Interface

        [Route("index")]
        [HttpPost]
        public IHttpActionResult Index()
        {
            Data = _universitiesLogic.Index();

            return Ok(Data);
        }

        [Route("getuniversity/{id}")]
        [HttpGet]
        public IHttpActionResult GetUniversity(int id)
        {
            Data = _universitiesLogic.GetUniversity(id);

            return Ok(Data);
        }

        [Route("create")]
        [HttpPost]
        public IHttpActionResult Create(TS.DTO.Data data)
        {
            JsTreeFormatDTO model = data.Object.ToObject<JsTreeFormatDTO>();

            Data = _universitiesLogic.Create(model);

            return Ok(Data);
        }

        [Route("update")]
        [HttpPost]
        public IHttpActionResult Update(TS.DTO.Data data)
        {
            JsTreeFormatDTO model = data.Object.ToObject<JsTreeFormatDTO>();

            Data = _universitiesLogic.Update(model);

            return Ok(Data);
        }

        [Route("delete")]
        [HttpPost]
        public IHttpActionResult Delete(TS.DTO.Data data)
        {
            int id = Convert.ToInt32(data.Object);

            Data = _universitiesLogic.Delete(id);

            return Ok(Data);
        }

        #endregion
    }
}
