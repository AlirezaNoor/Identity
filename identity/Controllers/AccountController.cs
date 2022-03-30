using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using identity._Context;
using identity.Models;
using identity.Models.ViewModels.Account;
using identity.Reposetory;
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
        private readonly IMessageSender _messageSenders;

        public AccountController(MyContext context, UserManager<IdentityUser> identity,
            SignInManager<IdentityUser> signinmanager,IMessageSender messageSenders)
        {
            _context = context;
            _identity = identity;
            _signinmanager = signinmanager;
            _messageSenders = messageSenders;
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
               
            };
            var result = await _identity.CreateAsync(user, registerViewmodel.password);
            if (result.Succeeded)
            {

                var emailConfirmationToken =
                    await _identity.GenerateEmailConfirmationTokenAsync(user);
                var emailMessage =
                    Url.Action("ConfirmEmail", "Account",
                        new { username = user.UserName, token = emailConfirmationToken },
                        Request.Scheme);
                await _messageSenders.SendEmailAsync(registerViewmodel.email, "Email confirmation", emailMessage,false);

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
        public async Task<IActionResult>  Login()
        {
         
            if (_signinmanager.IsSignedIn(User))
            {
                return RedirectToAction("index", "Home");
            }

            var model = new LoginViewModel()
            {
externalation = (await _signinmanager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (_signinmanager.IsSignedIn(User)) return RedirectToAction("Index", "Home");
            model.externalation = (await _signinmanager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var result = await _signinmanager.PasswordSignInAsync(
                    model.UserName, model.password, model.Remberme, true);

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

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userName, string token)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(token))
                return NotFound();
            var user = await _identity.FindByNameAsync(userName);
            if (user == null) return NotFound();
            var result = await _identity.ConfirmEmailAsync(user, token);

            
            return Content(result.Succeeded ? "Email Confirmed" : "Email Not Confirmed");
        }

        [HttpGet]
        public async Task<IActionResult> ForgetPsdd()
        {
            var model = new Emial();
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> ForgetPsdd(Emial command)
        {
            var user = new IdentityUser()
            {
                UserName = command.name
            };
            var emailConfirmationToken =
                await _identity.GeneratePasswordResetTokenAsync(user);
            var emailMessage =
                Url.Action("Confirmpass", "Account",
                    new { email =command.email, token = emailConfirmationToken },
                    HttpContext.Request.Scheme);
            await _messageSenders.SendEmailAsync(command.email, "pass confirmation", emailMessage, false);
            return RedirectToAction("Index","Home");

        }

        public IActionResult Confirmpass(string email, string token)
        {
            var model = new resetpas()
            {
                email = email,
                token = token

            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Confirmpass
            (string email, string token, resetpas res)
        {
            if (string.IsNullOrEmpty(email))
                return NotFound();
            var user = await _identity.FindByEmailAsync(email);
            if (user == null) return NotFound();
            var newtoken = await _identity.GeneratePasswordResetTokenAsync(user);
            var result = await _identity.ResetPasswordAsync(user, newtoken, res.pass);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost]
            public IActionResult ExternalLogin(string provider)
            {
                var redirectUrl = Url.Action("ExternalLoginCallBack", "Account"
                    );

                var properties = _signinmanager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
                return new ChallengeResult(provider, properties);
            }

            public async Task<IActionResult> ExternalLoginCallBack( string remoteError = null)
            {

                var loginViewModel = new LoginViewModel()
                {
                    externalation = (await _signinmanager.GetExternalAuthenticationSchemesAsync()).ToList()
                };

                if (remoteError != null)
                {
                    ModelState.AddModelError("", $"Error : {remoteError}");
                    return View("Login", loginViewModel);
                }

                var externalLoginInfo = await _signinmanager.GetExternalLoginInfoAsync();
                if (externalLoginInfo == null)
                {
                    ModelState.AddModelError("ErrorLoadingExternalLoginInfo", $"مشکلی پیش آمد");
                    return View("Login", loginViewModel);
                }

                var signInResult = await _signinmanager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
                    externalLoginInfo.ProviderKey, false, true);

                if (signInResult.Succeeded)
                {
                    return RedirectToAction("index","Home");
                }

                var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    var user = await _identity.FindByEmailAsync(email);
                    if (user == null)
                    {
                        var userName = email.Split('@')[0];
                        user = new IdentityUser()
                        {
                            UserName = (userName.Length <= 10 ? userName : userName.Substring(0, 10)),
                            Email = email,
                            EmailConfirmed = true
                        };

                        await _identity.CreateAsync(user);
                    }

                    await _identity.AddLoginAsync(user, externalLoginInfo);
                    await _signinmanager.SignInAsync(user, false);

                    return RedirectToAction("index", "Home");
                }

                ViewBag.ErrorTitle = "لطفا با بخش پشتیبانی تماس بگیرید";
                ViewBag.ErrorMessage = $"دریافت کرد {externalLoginInfo.LoginProvider} نمیتوان اطلاعاتی از";
                return View();
            }

        
    }
}