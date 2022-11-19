using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SpecialTagController : Controller
    {
        ApplicationDbContext _db;

        public SpecialTagController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            Console.WriteLine("Hi Mehedi");
            var data = _db.SpecialTag.ToList();
            return View(data);
        }

        // get the Create action 
        public IActionResult Create()
        {
            return View();
        }

        // post the Create data .. action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecialTag specialTag)
        {
            if(ModelState.IsValid)
            {
                _db.SpecialTag.Add(specialTag);
               await _db.SaveChangesAsync();
              return RedirectToAction(nameof(Index));

            }

            return View(specialTag);

        }

        // *********** Edit ************
        // get action for Edit
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var data = _db.SpecialTag.Find(id);
            if(data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        // post action for Edit 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SpecialTag specialTag)
        {
            if(ModelState.IsValid)
            {
                _db.SpecialTag.Update(specialTag);
               await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            return View(specialTag);
        }

        // *************** Details **************8
        // Get action for Details
        public IActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var data = _db.SpecialTag.Find(id);
            if(data == null)
            {
                return NotFound();
            }

            return View(data);
        }
        // post action for Details 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details()
        {
            return RedirectToAction(nameof(Index));
        }

        // **************** Delete **************
        // Get action for Delete
        public IActionResult Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var data = _db.SpecialTag.Find(id);
            if(data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        // post action for Delete 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, SpecialTag specialTag)
        {
            if(id == null)
            {
                return NotFound();
            }
            if(id != specialTag.Id)
            {
                return NotFound();
            }

            // this data present into database or not check
            var data = _db.SpecialTag.Find(id);
            if(data == null)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {                       
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(specialTag);

        }

    }
}
