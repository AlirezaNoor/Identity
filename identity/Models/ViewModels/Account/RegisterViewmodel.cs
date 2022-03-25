using System.ComponentModel.DataAnnotations;

namespace identity.Models.ViewModels.Account
{
    public class RegisterViewmodel
    {
        [Required] 
        [Display(Name = "اسم")]
        public string name { get; set; }
        [Required]
        [Display(Name = "ایمیل")]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        [Display(Name = "رمز")]
        [DataType(DataType.Password)]

        public string password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "تکرار")]
        [Compare(nameof(password))]
        public string comfirmpassword { get; set; }
    }
}
