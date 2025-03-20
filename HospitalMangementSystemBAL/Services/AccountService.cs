using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Owin;
using HospitalManagementSystemDAL.Models;
using HospitalManagementSystemDAL.Repositories;
using HospitalManagementSystemShared.ViewModels;

namespace HospitalManagementSystemBAL.Services
{
    public class AccountService
    {
        private IAccountRepository _accountRepository;

        public AccountService(IOwinContext context)
        {
            _accountRepository = new AccountRepository(context);
            AutoMapperConfig.RegisterMappings();
        }


        public async Task<string> LoginAsync(LoginViewModel loginDto)
        {
            var loginModel = Mapper.Map<LoginViewModel,LoginModel>(loginDto);
            return await _accountRepository.LoginAsync(loginModel);
        }


        public async Task<string> RegisterAsync(RegisterViewModel registerDto)
        {
            var registerModel = Mapper.Map<RegisterViewModel,RegisterModel>(registerDto); 
            return  await _accountRepository.RegisterAsync(registerModel);
        }


        public void Logout()
        {
            _accountRepository.Logout();
        }

        
    }
}
