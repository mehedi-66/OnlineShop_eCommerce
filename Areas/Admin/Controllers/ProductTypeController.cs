using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductTypeController : Controller
    {
        private ApplicationDbContext _db;

        public ProductTypeController(ApplicationDbContext db)
        {
            _db = db;
           
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            // var data = _db.ProductTypes.ToList();
             
            return View(_db.ProductTypes.ToList());
        }

        // Create Get action Method
       
        public IActionResult Create()
        {
            return View();
        }

        // Create Post action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypes productTypes)
        {
            if(ModelState.IsValid) // server side validation based on the model anotation to the feild 
            {
                _db.ProductTypes.Add(productTypes);
                await _db.SaveChangesAsync();
                TempData["save"] = "Save product";
                return RedirectToAction(nameof(Index));

            }

            return View(productTypes);
        }


        // get Edit page
     
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            // search the proudct in database and bring that one 
            var productType = _db.ProductTypes.Find(id); 
            if(productType == null)
            {
                return NotFound();
            }
            

            return View(productType);
        }

        // post and update Edit page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductTypes productTypes)
        {
            if(ModelState.IsValid)
            {
                _db.Update(productTypes);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(productTypes);
        }

        // get the Details page
        public IActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var productType = _db.ProductTypes.Find(id);
            if(productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        // back or redirect page only not doing anything now 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(ProductTypes productTypes)
        {
            return RedirectToAction(nameof(Index));
        }


        // get delete 
        public IActionResult Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var productType = _db.ProductTypes.Find(id);
            if(productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        // post delete 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, ProductTypes productTypes)
        {
            if(id == null)
            {
                return NotFound();
            }

            if(id != productTypes.Id)
            {
                return NotFound();  // route id and product Id not matcing then .. return not found
            }

            var productType = _db.ProductTypes.Find(id);
            if(productType == null)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                _db.Remove(productType); // this find product remove from the db
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(productTypes);
        }


    }
}
