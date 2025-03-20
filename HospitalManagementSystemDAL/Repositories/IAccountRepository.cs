using System.Threading.Tasks;
using HospitalManagementSystemDAL.Models;

namespace HospitalManagementSystemDAL.Repositories
{
    public interface IAccountRepository
    {
        Task<string> LoginAsync(LoginModel loginModel);
        void Logout();
        Task<string> RegisterAsync(RegisterModel registerModel);

    }
}
