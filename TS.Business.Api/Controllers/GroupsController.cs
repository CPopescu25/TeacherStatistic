using BusinessServices.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TS.Data.Context.Models;

namespace TS.Business.Api.Controllers
{
    [RoutePrefix("api/groups")]
    public class GroupsController : ApiController
    {
        #region Fields
        private readonly IGroupsLogic _groupsLogic;

        public DTO.Data Data;
        #endregion

        #region Constructor
        public GroupsController(IGroupsLogic groupsLogic)
        {
            _groupsLogic = groupsLogic;

            Data = new DTO.Data();
        }
        #endregion

        #region Public Interface

        [Route("postgroups")]
        [HttpPost]
        public async Task<IHttpActionResult> GetQuestionnaireAnswersByStudentId(TS.DTO.Data data)
        {
            Group group = data.Object.ToObject<Group>();

            Data = await _groupsLogic.PostGroups(group);

            return Ok(Data);
        }

        [Route("allgroups")]
        [HttpPost]
        public IHttpActionResult AllGroups()
        {
            Data = _groupsLogic.AllGroups();

            return Ok(Data);
        }

        [Route("getgroup")]
        [HttpPost]
        public async Task<IHttpActionResult> GetGroup(TS.DTO.Data data)
        {
            Guid id = new Guid(data.Object);

            Data = await _groupsLogic.GetGroup(id);

            return Ok(Data);
        }

        [Route("editgroup")]
        [HttpPost]
        public async Task<IHttpActionResult> EditGroup(TS.DTO.Data data)
        {
            Group group = data.Object.ToObject<Group>();

            Data = await _groupsLogic.EditGroup(group);

            return Ok(Data);
        }

        [Route("delete")]
        [HttpPost]
        public IHttpActionResult Delete(TS.DTO.Data data)
        {
            Guid id = new Guid(data.Object);

            Data = _groupsLogic.Delete(id);

            return Ok(Data);
        }

        [Route("getuserformenu")]
        [HttpPost]
        public IHttpActionResult GetUserForMenu(TS.DTO.Data data)

        {
            Data = _groupsLogic.GetUserForMenu(data.UserId);

            return Ok(Data);
        }
        #endregion
    }
}
