using System.ComponentModel.DataAnnotations;

namespace identity.Models.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required,Display(Name = "نام کاربری")]
        public string UserName { get; set; }
        [Required, Display(Name = "نام کاربری")]
        public string Email { get; set; }
        [Required, Display(Name = "پسورد")]
        public string password { get; set; }
        [Display(Name = "مرا به خاطر پسار")]
        public bool Remberme { get; set; }

    }
}
