using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.Areas.Admin.Models;
using OnlineShop.Data;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<IdentityUser> _userManager; 
        ApplicationDbContext _db;
        public RoleController(RoleManager<IdentityRole> roleManager, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = roles;
            return View();
        }

        public async Task<IActionResult>Create()
        {


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Create(string name)
        {
            IdentityRole role = new IdentityRole();
            role.Name = name;
            
            var isExist = await  _roleManager.RoleExistsAsync(name);
           
            if(!isExist)
            {
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    TempData["save"] = "Role Save Successfully";
                    return RedirectToAction(nameof(Index));

                }
            }



            ViewBag.mgs = "This role is alredy exist";
            ViewBag.name = name;
            return View();
        }



        // ************ Edit **************8
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            ViewBag.id = role.Id;
            ViewBag.name = role.Name; 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id , string name)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }


           //  IdentityRole role = new IdentityRole();
            role.Name = name;

            var isExist = await _roleManager.RoleExistsAsync(name);

            if (!isExist)
            {
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    TempData["save"] = "Role Updated Successfully";
                    return RedirectToAction(nameof(Index));

                }
            }



            ViewBag.mgs = "This role is alredy exist";
            ViewBag.name = name;
            return View();
        }


        // ****************** Delete 
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            ViewBag.id = role.Id;
            ViewBag.name = role.Name;
            return View();
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var result = _roleManager.DeleteAsync(role);
            if(result.IsCompletedSuccessfully)
            {
                TempData["save"] = "Role has been deleted successfully";
                return RedirectToAction(nameof(Index)); 
            }

            ViewBag.id = role.Id;
            ViewBag.name = role.Name;
            return View();



        }


        // *************** Assing role to user
        public async Task<IActionResult>Assign()
        {

            ViewData["UserId"] = new SelectList(_db.ApplicationUsers.Where(c => c.LockoutEnd < DateTime.Now || c.LockoutEnd == null).ToList(), "Id", "UsearName");
            ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Assign(RoleUserVm roleuser)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == roleuser.UserId);


            var isCheckRoleAssign = await _userManager.IsInRoleAsync(user, roleuser.RoleId);
            
            if(isCheckRoleAssign)
            {
                ViewBag.mgs = "This user already assing this role";
                return View();
            }
            var role = await _userManager.AddToRoleAsync(user, roleuser.RoleId);
            if(role.Succeeded)
            {
                TempData["save"] = "Role has assign successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        public IActionResult AssignUserRole()
        {
            // three table join 

            var result = from ur in _db.UserRoles
                         join r in _db.Roles on ur.RoleId equals r.Id
                         join a in _db.ApplicationUsers on ur.UserId equals a.Id
                         select new UserRoleMapping()
                         {
                             UserId = ur.UserId,
                             RoleId = ur.RoleId,
                             UserName = a.UserName,
                             RoleName = r.Name
                         };

            ViewBag.UserRoles = result;

            return View();
        }

         



    }
}
