using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using identity.Models.ViewModels.ManageUser;
using identity.Reposetory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace identity.Controllers
{
    public class ManageUserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageUserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
      
        public IActionResult Index()
        {
            var User = _userManager.Users
                .Select(u => new Indexviewmodel() {id = u.Id, email = u.Email, Username = u.UserName}).ToList();
            return View(User);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string id, string username)
        {
            if (id == null || username == null) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.UserName = username;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> AddUserToRole(string id)
        {
            if (id == null) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var roles = _roleManager.Roles.ToList();
            var model = new AddUserToRoleViewModel()
            {
                id = id
            };
            foreach (var role in roles)
            {
                if (!await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.addrole.Add(new Addrole() {Rolename = role.Name});
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(AddUserToRoleViewModel model)
        {
            if (model == null) return NotFound();

            var user = await _userManager.FindByIdAsync(model.id);
            if (user == null) return NotFound();

            var requestRols = model.addrole.Where(x => x.Isselected).Select(x => x.Rolename).ToList();
            var result = await _userManager.AddToRolesAsync(user, requestRols);
            return View(model);
        }

        #region remove

        [HttpGet]
        public async Task<IActionResult> RemoveUserFromRole(string id)
        {
            if (id == null) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var roles = _roleManager.Roles.ToList();
            var model = new AddUserToRoleViewModel()
            {
                id = id
            };
            foreach (var role in roles)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.addrole.Add(new Addrole() {Rolename = role.Name});
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserFromRole(AddUserToRoleViewModel model)
        {
            if (model == null) return NotFound();

            var user = await _userManager.FindByIdAsync(model.id);
            if (user == null) return NotFound();

            var requestRols = model.addrole.Where(x => x.Isselected).Select(x => x.Rolename).ToList();
            var result = await _userManager.RemoveFromRolesAsync(user, requestRols);
            return View(model);
        }

        #endregion

        [HttpPost]
        public async Task<IActionResult> DeleteUser(Indexviewmodel model)
        {
            if (model.id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(model.id);
            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded) return RedirectToAction("Index");

            return NotFound();
        }


        [HttpGet]
        public async Task<IActionResult> AddUserToClaim(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var allClaim = ClaimStore.AllClaims;
            var userClaims = await _userManager.GetClaimsAsync(user);
            var validClaims =
                allClaim.Where(c => userClaims.All(x => x.Type != c.Type))
                    .Select(c => new ClaimsViewModel(c.Type)).ToList();

            var model = new AddOrRemoveClaimViewModel(id, validClaims);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserToClaim(AddOrRemoveClaimViewModel model)
        {
            if (model == null) return NotFound();
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();
            var requestClaims =
                model.UserClaims.Where(r => r.IsSelected)
                .Select(u => new Claim(u.ClaimType, true.ToString())).ToList();

            var result = await _userManager.AddClaimsAsync(user, requestClaims);

            if (result.Succeeded) return RedirectToAction("index");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeletClaim(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userClaims = await _userManager.GetClaimsAsync(user);
            var validClaims =
                userClaims.Select(c => new ClaimsViewModel(c.Type)).ToList();

            var model = new AddOrRemoveClaimViewModel(id, validClaims);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletClaim(AddOrRemoveClaimViewModel model)
        {
            if (model == null) return NotFound();
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();
            var requestClaims =
                model.UserClaims.Where(r => r.IsSelected)
                    .Select(u => new Claim(u.ClaimType, true.ToString())).ToList();

            var result = await _userManager.RemoveClaimsAsync(user, requestClaims);

            if (result.Succeeded) return RedirectToAction("index");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

    }
}