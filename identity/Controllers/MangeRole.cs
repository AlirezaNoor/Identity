using System.Linq;
using System.Threading.Tasks;
using identity.Models.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace identity.Controllers
{
    public class MangeRole : Controller

    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public MangeRole(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult AddRole()
        {
            return View();

        }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> AddRole(IdentityRoleViewModel user)
            {
                if (!ModelState.IsValid)
                {
                    return NotFound();
                }

                if (user.Name==null)
                {
                    return NotFound();
                }

                var role = new IdentityRole(user.Name);


                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                   return RedirectToAction("Index" ,"MangeRole");
                }

                foreach (var err in result.Errors )
                {
                    ModelState.AddModelError("",err.Description);
                }

                return View(user);
            }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteRole(string id)
            {
                if (id==null)
                {
                    return NotFound();
                }

                var role =await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return NotFound();
                }

                await _roleManager.DeleteAsync(role);
                return RedirectToAction("Index"); 
            }

            [HttpGet]
            public async Task<IActionResult> EditRole(string id)
            {
                if (id==null)
                {
                    return NotFound();
                }

                var role = await _roleManager.FindByIdAsync(id);

                if (role==null)
                {
                    return NotFound();
                }

                return View(role);
            }

        public async Task<IActionResult> EditRole( string id,string name)
        {
           var role= await _roleManager.FindByIdAsync(id);
           role.Name=name;
           await _roleManager.UpdateAsync(role);
           return RedirectToAction("Index");

        }

    }
}
