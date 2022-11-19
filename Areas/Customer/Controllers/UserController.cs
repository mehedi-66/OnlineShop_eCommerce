using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {

        ApplicationDbContext _db;
        
        UserManager<IdentityUser> _userManager;
        public UserController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {

            return View(_db.ApplicationUsers.ToList());
        }

        public async Task<IActionResult>Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>Create(ApplicationUser user)
        {
            if(ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user, user.PasswordHash);  // encrypt password
                 
                if (result.Succeeded)
                {
                    var isSaveRole = await _userManager.AddToRoleAsync(user, "User");

                    TempData["save"] = "User Create Successfully";
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
           

            
            return View();
        }

        // udate user info 
        public async Task<IActionResult>Edit(string id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if(user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id); 
            if(userInfo == null)
            {
                return NotFound();
            }
            userInfo.FirstName = user.FirstName;
            userInfo.LastName = user.LastName;
            var result = await _userManager.UpdateAsync(userInfo);
            if(result.Succeeded)
            {
                TempData["save"] = "User data Update Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        // user Details 
        public async Task<IActionResult> Details(string id)
        {
            var user =  _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if(user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        // delete user and ... Fixed Time diactiveate 
        public async Task<IActionResult> Delete(string id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if(user == null)
            {
                return NotFound();
            }



            return View(user);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ApplicationUser user)
        {

            var userInfo = _db.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if(userInfo == null)
            {
                return NotFound();
            }

            // profile disable upto 100 years only
            userInfo.LockoutEnd = DateTime.Now.AddYears(100);

           int rowAffected =  _db.SaveChanges();
            if(rowAffected > 0)
            {
                TempData["save"] = "User has been Locout Successfylly";
                return RedirectToAction(nameof(Index));
            }

            return View(user); 
        }


        // Active the disable user
        public async Task<IActionResult> Active(string id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if(user == null)
            {
                return NotFound();
            }

            return View(user);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Active(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if(userInfo == null)
            {
                return NotFound();
            }
            userInfo.LockoutEnd = null;
            int rowAffected = _db.SaveChanges();
            if(rowAffected > 0)
            {
                TempData["save"] = "User Activated Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }



        // Delete Permanently user 
        public async Task<IActionResult> DeletePermanent(string id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePermanent(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }
          
             _db.ApplicationUsers.Remove(userInfo);
            int rowAffected = _db.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["save"] = "User Delete Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
    }
}
