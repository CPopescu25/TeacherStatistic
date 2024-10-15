using BusinessServices.Answers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TS.Data.Context.Enums;
using TS.Data.Context.Models;
using TS.DTO;

namespace TS.Business.Api.Controllers
{
    [RoutePrefix("api/answers")]
    public class AnswersController : ApiController
    {
        #region Fields
        private readonly IAnswersLogic _answersLogic;

        public TS.DTO.Data Data;
        #endregion

        #region Constructor
        public AnswersController(IAnswersLogic answersLogic)
        {
            _answersLogic = answersLogic;
            Data = new DTO.Data();
        }
        #endregion

        #region Public Interface

        [Route("getquestionnaireqnswersbystudentid")]
        [HttpPost]
        public IHttpActionResult GetQuestionnaireAnswersByStudentId(DTO.Data data)
        {
            Data = _answersLogic.GetQuestionnaireAnswersByStudentId(data.UserId);

            return Ok(Data);
        }

        [Route("answer")]
        [HttpPost]
        public IHttpActionResult Answer(TS.DTO.Data data)
        {
            AnswersHelpDTO o = data.Object.ToObject<AnswersHelpDTO>();

            Answers model = new Answers();

            model.ID = Guid.NewGuid();
            model.SetType = (SetType)o.SetType;
            model.StatisticAnswers = o.StatisticAnswers;
            model.UserID = o.User_Id;
            model.User_who_answer_ID = o.User_who_answer_Id;

            Data = _answersLogic.Answer(model);

            return Ok(Data);
        }

        [Route("getemployeeanswers")]
        [HttpPost]
        public IHttpActionResult GetEmployeeAnswers(TS.DTO.Data data)
        {
            AnswersHelpDTO model = data.Object.ToObject<AnswersHelpDTO>();

            Data = _answersLogic.GetEmployeeAnswers(model);

            return Ok(Data);
        }

        [Route("delete")]
        [HttpPost]
        public IHttpActionResult Delete(TS.DTO.Data data)
        {
            Guid id = new Guid(data.Object);

            Data = _answersLogic.Delete(id);

            return Ok(Data);
        }
        #endregion
    }
}
