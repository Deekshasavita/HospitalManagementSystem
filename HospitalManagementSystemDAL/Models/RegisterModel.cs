using System.ComponentModel.DataAnnotations;
using HospitalManagementSystemShared.Constants;

namespace HospitalManagementSystemDAL.Models
{
    public class RegisterModel
    {
        [Required]
        [StringLength(50)]
        [RegularExpression(RegExPatterns.NameRegEx)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength (100)]
        [RegularExpression(RegExPatterns.EmailRegEx)]
        public string Email { get; set; }

        [Required]
        [RegularExpression(RegExPatterns.PasswordRegEx)]
        [DataType(DataType.Password)]    
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [RegularExpression(RegExPatterns.PhoneRegEx)]
        public string PhoneNumber {  get; set; }
    }
}
