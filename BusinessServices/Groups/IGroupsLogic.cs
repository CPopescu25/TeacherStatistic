using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Data.Context.Models;
using TS.DTO;

namespace BusinessServices.Groups
{
    public interface IGroupsLogic : IBusinessService
    {
        Data AllGroups();
        Data Delete(Guid groupId);
        Task<Data> EditGroup(Group group);
        Task<Data> GetGroup(Guid groupId);
        Task<Data> PostGroups(Group group);
        Data GetUserForMenu(Guid userId);
    }
}
