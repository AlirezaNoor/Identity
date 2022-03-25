using System.Threading.Tasks;
using identity._Context;
using identity.Models.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly MyContext _context;
        private readonly UserManager<IdentityUser> _identity;

        public AccountController(MyContext context , UserManager<IdentityUser> identity)
        {
            _context = context;
            _identity = identity;
        }

        [HttpGet]
        public IActionResult Register()
        {
            var viewmodel = new RegisterViewmodel();
            return View(viewmodel);
        }
        [HttpPost]
        public async  Task<IActionResult> Register(RegisterViewmodel registerViewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewmodel);
            }

            var user = new IdentityUser()
            {
                Email = registerViewmodel.email,
                UserName = registerViewmodel.name,
                EmailConfirmed = true,

            };
            var result = await _identity.CreateAsync(user, registerViewmodel.password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("",err.Description);
                }
            }

            return View();
        }
    }
}
