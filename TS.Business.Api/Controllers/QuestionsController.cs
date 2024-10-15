using BusinessServices.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TS.Data.Context.Models;

namespace TS.Business.Api.Controllers
{
    [RoutePrefix("api/questions")]
    public class QuestionsController : ApiController
    {
        #region Fields
        private readonly IQuestionsLogic _questionsLogic;

        public DTO.Data Data;
        #endregion

        #region Constructor
        public QuestionsController(IQuestionsLogic questionsLogic)
        {
            _questionsLogic = questionsLogic;

            Data = new DTO.Data();
        }
        #endregion

        #region Public Interface

        [Route("index")]
        [HttpPost]
        public IHttpActionResult Index(TS.DTO.Data data)
        {
            int SetType = Convert.ToInt32(data.Object);

            Data = _questionsLogic.Index(SetType);

            return Ok(Data);
        }

        [Route("getquestions")]
        [HttpPost]
        public IHttpActionResult Get(TS.DTO.Data data)
        {
            Guid id = new Guid(data.Object);

            Data = _questionsLogic.Get(id);

            return Ok(Data);
        }

        [Route("edit")]
        [HttpPost]
        public IHttpActionResult Edit(TS.DTO.Data data)
        {
            Questions question = data.Object.ToObject<Questions>();

            Data = _questionsLogic.Edit(question);

            return Ok(Data);
        }

        [Route("create")]
        [HttpPost]
        public IHttpActionResult Create(TS.DTO.Data data)
        {
            Questions question = data.Object.ToObject<Questions>();

            Data = _questionsLogic.Create(question);

            return Ok(Data);
        }

        [Route("delete")]
        [HttpPost]
        public IHttpActionResult Delete(TS.DTO.Data data)
        {
            Guid id = new Guid(data.Object);

            Data = _questionsLogic.Delete(id);

            return Ok(Data);
        }
        #endregion
    }
}
