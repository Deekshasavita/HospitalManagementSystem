using System.Linq;
using System.Threading.Tasks;
using HospitalManagementSystemDAL.Context;
using HospitalManagementSystemDAL.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin;
using HospitalManagementSystemShared.Constants;

namespace HospitalManagementSystemDAL.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly AppDbContext _appDbContext;

        
        public AccountRepository(IOwinContext context)
        {
            _appDbContext = new AppDbContext();
            _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_appDbContext));
            _signInManager = new ApplicationSignInManager(_userManager, context.Authentication);
            _authenticationManager = context.Authentication;

        }

        public async Task<string> LoginAsync(LoginModel loginModel)
        {
           var user = await _userManager.FindByEmailAsync(loginModel.Email);
           
            if (user == null)
            {
                return NotificationMessages.IncorrectEmail ;
            }

            var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, shouldLockout: false);

            if (result == SignInStatus.Success)
            {
                return "Success";
            }
            else
            {
                return NotificationMessages.IncorrectPassword;
            }
        }

        public void Logout()
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        public async Task<string> RegisterAsync(RegisterModel registerModel)
        {
            var user = new ApplicationUser { UserName = registerModel.Email, Email = registerModel.Email, FullName = registerModel.FullName, PhoneNumber = registerModel.PhoneNumber };

            var result = await _userManager.CreateAsync(user, registerModel.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user.Id, "Doctor");
                await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                return "true";
            }
            else
            {
                var error = result.Errors.FirstOrDefault();
                return error;
            }
                             
        }      
    }
}
