using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.DTO;

namespace BusinessServices.User
{
    public interface IUserLogic : IBusinessService
    {
        Data ChangePassword(ChangePasswordDTO model);
        Task<Data> Create(TS.Data.Context.Models.User u);
        Data Delete(Guid userId);
        Data Edit(TS.Data.Context.Models.User u);
        Data EditImage(TS.Data.Context.Models.User user);
        Data ForgotPassword(ChangePasswordDTO model);
        //TS.Data.Context.Models.User GetByEmail(string email);
        Data GetDetails(Guid userId);
        UserHelpDTO GetImageUser(Guid id);
        Data Index(SearchModelDTO searchModel);
        Data ListStudents();
        Data ListTeachers();
        Data ListAdmins();
    }
}
