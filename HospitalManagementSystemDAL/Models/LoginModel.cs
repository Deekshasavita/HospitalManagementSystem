using System.ComponentModel.DataAnnotations;
using HospitalManagementSystemShared.Constants;

namespace HospitalManagementSystemDAL.Models
{
    public class LoginModel 
    {
        [Required]
        [EmailAddress]
        [RegularExpression(RegExPatterns.EmailRegEx)]
        public string Email { get; set; }

        [Required]
        [RegularExpression(RegExPatterns.PasswordRegEx)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

}
