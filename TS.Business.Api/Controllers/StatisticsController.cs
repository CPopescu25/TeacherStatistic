using BusinessServices.Statistics;
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
    [RoutePrefix("api/statistics")]
    public class StatisticsController : ApiController
    {
        #region Fields
        private readonly IStatisticsLogic _statisticsLogic;

        public DTO.Data Data;
        #endregion

        #region Constructor
        public StatisticsController(IStatisticsLogic statisticsLogic)
        {
            _statisticsLogic = statisticsLogic;

            Data = new DTO.Data();
        }
        #endregion

        #region Public Interface

        [Route("create")]
        [HttpPost]
        public IHttpActionResult Create(TS.DTO.Data data)
        {
            Statistics model = data.Object.ToObject<Statistics>();

            Data = _statisticsLogic.Create(model);

            return Ok(Data);
        }

        [Route("index")]
        [HttpPost]
        public IHttpActionResult Index(TS.DTO.Data data)
        {
            int SetType = Convert.ToInt32(data.Object);

            Data = _statisticsLogic.Index(SetType);

            return Ok(Data);
        }

        [Route("teacherquestionnaires")]
        [HttpPost]
        public IHttpActionResult TeacherQuestionnaires(TS.DTO.Data data)
        {
            Guid teacherId = data.UserId;

            Data = _statisticsLogic.TeacherQuestionnaires(teacherId);

            return Ok(Data);
        }

        [Route("getteachersstatisticsbyuniversity")]
        [HttpPost]
        public IHttpActionResult TeachersStatisticsByUniversity(TS.DTO.Data data)
        {
            //the group name
            SearchModelDTO searchModelDTO = data.Object.ToObject<SearchModelDTO>();

            List<TeachersStatisticsDTO> list = new List<TeachersStatisticsDTO>();

            /*Data*/ list = _statisticsLogic.TeachersStatisticsByUniversity(searchModelDTO);

            return Ok(list);
        }

        [Route("delete")]
        [HttpPost]
        public IHttpActionResult Delete(TS.DTO.Data data)
        {
            Guid id = new Guid(data.Object);

            Data = _statisticsLogic.Delete(id);

            return Ok(Data);
        }

        [Route("details")]
        [HttpPost]
        public IHttpActionResult Details(TS.DTO.Data data)
        {
            StatisticHelpDTO model = data.Object.ToObject<StatisticHelpDTO>();

            Data = _statisticsLogic.Details(model);

            return Ok(Data);
        }

        #endregion
    }
}
