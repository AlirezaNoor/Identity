using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using identity._Context;
using identity.Models.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp;

namespace identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly MyContext _context;
        private readonly UserManager<IdentityUser> _identity;
        private readonly SignInManager<IdentityUser> _signinmanager;

        public AccountController(MyContext context, UserManager<IdentityUser> identity,
            SignInManager<IdentityUser> signinmanager)
        {
            _context = context;
            _identity = identity;
            _signinmanager = signinmanager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            var viewmodel = new RegisterViewmodel();
            return View(viewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewmodel registerViewmodel)
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
                    ModelState.AddModelError("", err.Description);
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginViewModel();
            if (_signinmanager.IsSignedIn(User))
            {
                return RedirectToAction("index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (_signinmanager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                var result = await _signinmanager.PasswordSignInAsync(
                    model.Email, model.password, model.Remberme, true);

                if (result.Succeeded)
                {


                    return RedirectToAction("Index", "Home");
                }

                if (result.IsLockedOut)
                {
                    ViewData["ErrorMessage"] = "اکانت شما به دلیل پنج بار ورود ناموفق به مدت پنج دقیق قفل شده است";
                    return View(model);
                }

                ModelState.AddModelError("", "رمزعبور یا نام کاربری اشتباه است");
            }
            return View(model);
      
    }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _signinmanager.SignOutAsync();
            return RedirectToAction("index", "Home");
        }


        public async Task<IActionResult>  IsnameUSe(string name)
        {
            var user = await _identity.FindByNameAsync(name);
            if (user==null)
            {
                return Json(true); 
            }

            return Json("هستتتتتتتت");
        }
    }
}