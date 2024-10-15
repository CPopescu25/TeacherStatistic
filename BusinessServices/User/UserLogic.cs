using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Data.Context;
using TS.Data.Context.Models;
using TS.DTO;
using TS.Resources.Services;

namespace BusinessServices.User
{
    public class UserLogic : IUserLogic
    {
        #region Fields

        private readonly TSClientContext _dbContext;
        private readonly IResourcesLoader _resourcesLoader;

        public Data Data;

        #endregion

        public UserLogic(TSClientContext dbContext,
          IResourcesLoader resourcesLoader)
        {
            _resourcesLoader = resourcesLoader;
            _dbContext = dbContext;

            Data = new Data();
            Data.Object = null;
        }

        //get the image of a user, knowing user id
        public UserHelpDTO GetImageUser(Guid id)
        {
            var u = _dbContext.Users.FirstOrDefault(x => x.ID == id);

            UserHelpDTO model = new UserHelpDTO();
            model.Id = u.ID;
            model.Image = u.Image;

            return model;
   
        }

        //create a new user
        public async Task<Data> Create(TS.Data.Context.Models.User u)
        {
            _dbContext.Users.Add(u);
            try
            {
               await _dbContext.SaveChangesAsync();
                Data.Ok = true;
                Data.Message = _resourcesLoader.GetStringResourceByName("user_created");
               // Data.Object = u;

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
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
                //Data.Ok = false;
                //Data.Message = raise.Message;
            }

            return Data;
        }

        //edit a user
        public Data Edit(TS.Data.Context.Models.User u)
        {
            var originals = _dbContext.Users.FirstOrDefault(x => x.ID == u.ID);
            if (originals == null)
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("user_not_exist");
            }
            else
            {
                originals.Enable = u.Enable;
                originals.FirstName = u.FirstName;
                originals.LastName = u.LastName;
                originals.UserGroup.Clear();
                originals.UserGroup = u.UserGroup;
                originals.UserName = u.UserName;
                originals.UserUniversity.Clear();
                originals.UserUniversity = u.UserUniversity;
                originals.Year = u.Year;
                originals.Email = u.Email;
                originals.Phone = u.Phone;
                try
                {
                    _dbContext.SaveChanges();
                    Data.Ok = true;
                    Data.Message = _resourcesLoader.GetStringResourceByName("user_edited");

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
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                    //Data.Ok = false;
                    //Data.Message = raise.Message;
                }

            }
            return Data;
        }

        //get all user from database knowing the current group
        public Data Index(SearchModelDTO searchModel)
        {
            List<UserHelpDTO> list = new List<UserHelpDTO>();

            //the group name
            var lists = _dbContext.UserGroups.Where(u => u.User.Enable == true && u.Group.Name == searchModel.Group)
                .Select(x => new UserHelpDTO()
                {
                    Id = x.UserID,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    UserName = x.User.UserName,
                    Year = x.User.Year,
                    Enable = x.User.Enable,
                    FullName=x.User.FirstName + "." + x.User.LastName,
                    EncryptedEmail=x.User.EncryptedEmail,
                    EncryptedPhone=x.User.EncryptedPhone
                }).ToList();

            if (searchModel.FirstName == "" && searchModel.LastName == "" && searchModel.Year == 0 && searchModel.ids_int == null && searchModel.GetAll == 0)
            {
                list = null;
            }
            else 
            //if(searchModel.GetAll == 1)
            //{
            //    list = lists.ToList();
            //}
            //else
            //{
            //    if(searchModel.ids_int != null)
            //    {
            //        List<UserHelpDTO> l_u = new List<UserHelpDTO>();
            //        l_u = lists.Where(d => _dbContext.UserUniversities.Where(p => searchModel.ids_int.Any(s => s == p.UniversityID))
            //        .Any(p => p.UserID == d.Id)).ToList();

            //        if (searchModel.FirstName == "" && searchModel.LastName == "" && searchModel.Year == 0)
            //            list = l_u;
            //        else
            //        {
            //            if (searchModel.FirstName != "" && searchModel.LastName == "" && searchModel.Year == 0)
            //                list = l_u.Where(x => x.FirstName.Contains(searchModel.FirstName)).ToList();
            //            else if (searchModel.FirstName != "" && searchModel.LastName != "" && searchModel.Year == 0)
            //                list = l_u.Where(x => x.FirstName.Contains(searchModel.FirstName) && x.LastName.Contains(searchModel.LastName)).ToList();
            //            else
            //                list = l_u.Where(x => x.FirstName.Contains(searchModel.FirstName) && x.LastName.Contains(searchModel.LastName) && x.Year.ToString()==searchModel.Year.ToString()).ToList();
            //        }

            //    }
            //    else
            //    {
            //        if (searchModel.FirstName == "" && searchModel.LastName == "" && searchModel.Year == 0)
            //            list = lists;
            //        else
            //        {
            //            if (searchModel.FirstName != "" && searchModel.LastName == "" && searchModel.Year == 0)
            //                list = lists.Where(x => x.FirstName.Contains(searchModel.FirstName)).ToList();
            //            else if (searchModel.FirstName != "" && searchModel.LastName != "" && searchModel.Year == 0)
            //                list = lists.Where(x => x.FirstName.Contains(searchModel.FirstName) && x.LastName.Contains(searchModel.LastName)).ToList();
            //            else
            //                list = lists.Where(x => x.FirstName.Contains(searchModel.FirstName) && x.LastName.Contains(searchModel.LastName) && x.Year.ToString() == searchModel.Year.ToString()).ToList();
            //        }
            //    }
            //}
            {
                if (searchModel.ids_int != null && searchModel.GetAll == 0)
                {
                    list = lists.Where(d => _dbContext.UserUniversities.Where(p => searchModel.ids_int.Any(s => s == p.UniversityID))
                    .Any(p => p.UserID == d.Id)
                    && (d.FirstName == "" || d.FirstName.Contains(searchModel.FirstName))
                    && (d.LastName == "" || d.LastName.Contains(searchModel.LastName))
                    && (d.Year == 0 || d.Year.ToString() == searchModel.Year.ToString())).ToList();
                }
                else if (searchModel.GetAll == 0 && searchModel.ids_int == null)
                {
                    list = lists.Where(d => (d.FirstName == "" || d.FirstName.Contains(searchModel.FirstName))
                    && (d.LastName == "" || d.LastName.Contains(searchModel.LastName))
                    && (d.Year == 0 || d.Year.ToString() == searchModel.Year.ToString()))
                    .ToList();
                }
                else if (searchModel.GetAll == 1)
                {
                    list = lists.ToList();
                }

            }

            Data.Ok = true;
            Data.Message = _resourcesLoader.GetStringResourceByName("message_success");
            Data.Object = list;

            return Data;
        }

        //get the users from group admin
        public Data ListAdmins()
        {
            List<UserHelpDTO> list = new List<UserHelpDTO>();
            list = _dbContext.UserGroups.Where(ug => ug.User.Enable == true)
                .Join(_dbContext.Groups.Where(g => g.Name == "Admin"), ug => ug.GroupID,
                g => g.ID, (ug, g) => new UserHelpDTO
                {
                    Id = ug.User.ID,
                    FullName = ug.User.FirstName + " " + ug.User.LastName,
                    FirstName = ug.User.FirstName,
                    LastName = ug.User.LastName,
                    EncryptedEmail = ug.User.EncryptedEmail,
                    EncryptedPhone = ug.User.EncryptedPhone,
                    UserName = ug.User.UserName,
                    Year = ug.User.Year,
                    Enable = ug.User.Enable
                }).ToList();

            Data.Ok = true;
            Data.Message = _resourcesLoader.GetStringResourceByName("message_success");
            Data.Object = list;

            return Data;
        }

        //get the users from group student
        public Data ListStudents()
        {
            List<UserHelpDTO> list = new List<UserHelpDTO>();
            list = _dbContext.UserGroups.Where(ug => ug.User.Enable == true)
                .Join(_dbContext.Groups.Where(g => g.Name == "Student"), ug => ug.GroupID,
                g => g.ID, (ug, g) => new UserHelpDTO
                {
                    Id = ug.User.ID,
                    FullName = ug.User.FirstName + " " + ug.User.LastName,
                    FirstName = ug.User.FirstName,
                    LastName = ug.User.LastName,
                    EncryptedEmail=ug.User.EncryptedEmail,
                    EncryptedPhone=ug.User.EncryptedPhone,
                    UserName = ug.User.UserName,
                    Year = ug.User.Year,
                    Enable = ug.User.Enable
                }).ToList();

            Data.Ok = true;
            Data.Message = _resourcesLoader.GetStringResourceByName("message_success");
            Data.Object = list;

            return Data;
        }

        //get the user from group teacher
        public Data ListTeachers()
        {
            List<UserHelpDTO> list = new List<UserHelpDTO>();
            list = _dbContext.UserGroups.Where(ug => ug.User.Enable == true)
                .Join(_dbContext.Groups.Where(g => g.Name == "Teacher"), ug => ug.GroupID,
                g => g.ID, (ug, g) => new UserHelpDTO
                {
                    Id = ug.User.ID,
                    FullName = ug.User.FirstName + " " + ug.User.LastName,
                    FirstName=ug.User.FirstName,
                    LastName=ug.User.LastName,
                    EncryptedPhone=ug.User.EncryptedPhone,
                    EncryptedEmail=ug.User.EncryptedEmail,
                    UserName = ug.User.UserName,
                    Year = ug.User.Year,
                    Enable = ug.User.Enable
                }).ToList();

            Data.Ok = true;
            Data.Message = _resourcesLoader.GetStringResourceByName("message_success");
            Data.Object = list;

            return Data;
        }

        //get details user by id
        public Data GetDetails(Guid userId)
        {
            UserHelpDTO model = new UserHelpDTO();
            var models = _dbContext.Users.FirstOrDefault(x => x.ID == userId);
            if (models == null)
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("user_not_exist");
            }

            model.Enable = models.Enable;
            model.FirstName = models.FirstName;
            model.Year = models.Year;
            model.Email = models.Email;
            model.Phone = models.Phone;

            //convert the list of groups for current user to a dictionary<int,string>
            Dictionary<string, string> d_group = new Dictionary<string, string>();
            d_group = (_dbContext.Groups.Where(d => d.UserGroup.Where(p => p.UserID == userId)
                .Any(p => p.GroupID == d.ID)).ToList())
                .Select(l => new
                {
                    l.ID,
                    l.Name
                }).ToDictionary(d => d.ID.ToString(), d => d.Name);

            model.groups = d_group;
            model.Id = models.ID;
            model.LastName = models.LastName;
            model.Password = models.Password;

            //convert the list of universities for current user to a dictionary<int,string>
            Dictionary<int, string> d_univ = new Dictionary<int, string>();
            var univ_list = (_dbContext.Universities.Where(d => d.UserUniversity.Where(p => p.UserID == userId)
                .Any(p => p.UniversityID == d.ID)).ToList())
                .Select(l => new University
                {
                    ID = l.ID,
                    Name = l.Name,
                    Parent = l.Parent
                }).ToList();
            foreach (University u in univ_list)
            {
                string name = "";
                if (u.Parent != null)
                {
                    University univ = u;
                    do
                    {
                        name += univ.Name + "/";
                        univ = univ.Parent;
                    } while (univ != null);
                    name = name.Remove(name.Length - 1);
                }
                else
                {
                    name = u.Name;
                }
                d_univ.Add(u.ID, name);
            }
            model.universities = d_univ;
            model.UserName = models.UserName;


            Data.Ok = true;
            Data.Message = _resourcesLoader.GetStringResourceByName("message_success");
            Data.Object = model;

            return Data;
        }

        //delete a user
        public Data Delete(Guid userId)
        {
            TS.Data.Context.Models.User user = _dbContext.Users.FirstOrDefault(x => x.ID == userId);
            if (user == null)
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("user_not_exist");
            }

            user.Enable = false;
            _dbContext.SaveChanges();
            Data.Ok = true;
            Data.Message = _resourcesLoader.GetStringResourceByName("user_disabled");

            return Data;
        }

        //edit an image
        public Data EditImage(TS.Data.Context.Models.User user)
        {
            var originals = _dbContext.Users.FirstOrDefault(x => x.ID == user.ID);
            if (originals != null)
            {
                originals.Image = user.Image;
                _dbContext.SaveChanges();

                Data.Ok = true;
                Data.Message = _resourcesLoader.GetStringResourceByName("image_uploaded");
                Data.Object = user;
            }
            else
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("user_not_exist");
            }

            return Data;
        }

        //Change the Password for current login user 
        public Data ChangePassword(ChangePasswordDTO model)
        {
            var user = _dbContext.Users
                    .Where(s => s.UserName == model.Username 
                    && s.EncryptedPassword == model.EncryptedOldPassword 
                    && s.Enable == true)
                    .FirstOrDefault();

            if (user == null)
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("user_wrong");
            }
            else
            {
                user.EncryptedPassword = model.EncryptedNewPassword;


                _dbContext.Entry(user).State = EntityState.Modified;
                try
                {
                    _dbContext.SaveChanges();
                    Data.Ok = true;
                    Data.Message = _resourcesLoader.GetStringResourceByName("password_changed");

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
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;

                }
            }

            return Data;
        }


        //ForgotPassword
        public Data ForgotPassword(ChangePasswordDTO model)
        {
            UserHelpDTO user = new UserHelpDTO();

            //get the password from database by username
            user = _dbContext.Users.Where(s => s.UserName == model.Username && s.Enable == true)
                .Select(u => new UserHelpDTO
                {
                    UserName = u.UserName,
                    EncryptedPassword = u.EncryptedPassword
                }).FirstOrDefault();

            if (user == null)
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("user_not_in_database");
            }
            else
            {
                Data.Object = user;
                Data.Ok = true;
                Data.Message = _resourcesLoader.GetStringResourceByName("email_see");
            }

            return Data;
        }


        //public TS.Data.Context.Models.User GetByEmail(string email)
        //{
        //    string hashedEmail = FormsAuthentication.HashPasswordForStoringInConfigFile(email, "SHA1");

        //    return _dbContext.Users.AsNoTracking().FirstOrDefault(p => p.EmailHashed == hashedEmail);
        //}

        //public bool CheckCredentials(string email, string password)
        //{
        //    string hashedEmail = FormsAuthentication.HashPasswordForStoringInConfigFile(email, "SHA1");
        //    string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");

        //    TS.Data.Context.Models.User patient = _dbContext.Users.FirstOrDefault(
        //        patinet => patinet.EmailHashed == hashedEmail && patinet.Password == hashedPassword);

        //    if (patient == null)
        //        return false;

        //    return true;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iD"></param>
        /// <returns></returns>
        private bool UserHasAccessInfoAdmin(Guid iD)
        {
            var userGroup = _dbContext.UserGroups
              .Where(ug => ug.UserID == iD && ug.Group.Name == "Admin")
              .FirstOrDefault();

            if (userGroup != null) return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iD"></param>
        /// <returns></returns>
        private bool UserHasAccessInfoTeachers(Guid iD)
        {
            var userGroup = _dbContext.UserGroups
              .Where(ug => ug.UserID == iD && ug.Group.Name == "Teacher")
              .FirstOrDefault();

            if (userGroup != null) return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iD"></param>
        /// <returns></returns>
        private bool UserHasAccessInfoStudents(Guid iD)
        {
            var userGroup = _dbContext.UserGroups
              .Where(ug => ug.UserID == iD && ug.Group.Name == "Student")
              .FirstOrDefault();

            if (userGroup != null) return true;

            return false;
        }

        /// <summary>
        /// Commit data
        /// </summary>
        private void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
