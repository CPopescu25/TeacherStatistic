using System.Threading.Tasks;
using TS.DTO;

namespace BusinessServices.Login
{
    public interface ILoginLogic : IBusinessService
    {
        Data Login(UserHelpDTO userHelpDTO);
    }
}