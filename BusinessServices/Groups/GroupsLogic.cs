using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Data.Context;
using TS.DTO;
using TS.Resources.Services;

namespace BusinessServices.Groups
{
    public class GroupsLogic : IGroupsLogic
    {
        #region Fields
        private readonly TSClientContext _dbContext;
        private readonly IResourcesLoader _resourcesLoader;

        public Data Data;
        #endregion

        #region Constructor
        public GroupsLogic(TSClientContext dbContext,
          IResourcesLoader resourcesLoader)
        {
            _dbContext = dbContext;
            _resourcesLoader = resourcesLoader;

            Data = new Data();
            Data.Object = null;
        }
        #endregion

        #region IGroupsLogic implementation
        //create a new group of users with selected permissions
        public async Task<Data> PostGroups(TS.Data.Context.Models.Group group)
        {
            group.ID = Guid.NewGuid();
            _dbContext.Groups.Add(group);

            try
            {
               await _dbContext.SaveChangesAsync();

                Data.Ok = true;
                Data.Message = String.Format(_resourcesLoader.GetStringResourceByName("group_created"), group.Name);
                Data.Object = group;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
                //Data.Ok = false;
                //Data.Message = raise.Message;
            }

            return Data;
        }

        //get all groups from database
        public Data AllGroups()
        {
            List<GroupHelpDTO> list = new List<GroupHelpDTO>();
            list = _dbContext.Groups.OrderBy(q => q.Name).Select(g => new GroupHelpDTO
            {
                Id = g.ID,
                Name = g.Name,
                UserGroup = g.UserGroup.ToList()
            }).ToList();

            Data.Ok = true;
            Data.Message = _resourcesLoader.GetStringResourceByName("message_success");
            Data.Object = list;

            return Data;
        }

        //get a group by id
        public async Task<Data> GetGroup(Guid groupId)
        {
            TS.Data.Context.Models.Group group =
            await _dbContext.Groups.Where(z => z.ID == groupId).FirstOrDefaultAsync();

            if (group == null)
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("group_does_not_exist");
            }
            else
            {
                Data.Ok = true;
                Data.Message = _resourcesLoader.GetStringResourceByName("message_success");
                Data.Object = group;
            }

            return Data;
        }

        //edit a group
        public async Task<Data> EditGroup(TS.Data.Context.Models.Group group)
        {
            var originals = await _dbContext.Groups.FindAsync(group.ID);
            if (originals == null)
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("group_does_not_exist");
            }
            originals.Name = group.Name;

            _dbContext.Entry(originals).State = EntityState.Modified;
            try
            {
                _dbContext.SaveChanges();

                Data.Ok = true;
                Data.Message = String.Format(_resourcesLoader.GetStringResourceByName("group_edited"), group.Name);
                Data.Object = originals;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;

                //Data.Ok = false;
                //Data.Message = raise.Message;
            }

            return Data;
        }

        //delete a group
        public Data Delete(Guid groupId)
        {
            TS.Data.Context.Models.Group group = _dbContext.Groups.FirstOrDefault(g => g.ID == groupId);
            if (group == null)
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("group_does_not_exist");
            }

            _dbContext.Groups.Remove(group);
            try
            {
                _dbContext.SaveChanges();
                Data.Ok = true;
                Data.Message = String.Format(_resourcesLoader.GetStringResourceByName("group_deleted"), group.Name);

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
                //Data.Ok = false;
                //Data.Message = raise.Message;
            }

            return Data;
        }

        public Data GetUserForMenu(Guid userId)
        {
            var x = _dbContext.Users.Where(u => u.ID == userId)
                .FirstOrDefault()
                .UserGroup.Join(_dbContext.Groups, ug => ug.GroupID, g => g.ID, (ug, g) => new GroupHelpDTO()
                {
                    Id=g.ID,
                    Name=g.Name
                }).FirstOrDefault();

            Data.Object = x;

            return Data;
        }

        #endregion
    }
}
