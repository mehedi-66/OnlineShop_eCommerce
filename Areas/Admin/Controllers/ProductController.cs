using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        ApplicationDbContext _db;

        // get root directory wwwroot
        IHostingEnvironment _dir;
       
       public ProductController(ApplicationDbContext db, IHostingEnvironment dir)
        {
            _db = db;
            _dir = dir
 ;        }
        public IActionResult Index()
        {
            // Realtionship data ... buind and join
            // Another table data buind by Froign key .. Primary key ... 
            // how see Include( Lamda expression )   productTypes and SpecialTag table data join and buind
            
            var products = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList(); 
            
            return View(products);
        }

        // min and max Range product ... find 
        // post 
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Index(decimal lowAmount, decimal largeAmount)
        {
            var products = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).Where(c => c.Price >= lowAmount && c.Price <= largeAmount).ToList();

            return View(products);
        } 

        // *************** Create ********************
        // get create action 
        public IActionResult Create()
        {
            // For dorpdown menu we want to add                                  value   dispalay
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTag.ToList(), "Id", "TagName");

            return View();
        }

        // post create action 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile image)
        {
           
            if (ModelState.IsValid)
            {
                var searchProduct = _db.Products.FirstOrDefault(c => c.Name == product.Name);

                if(searchProduct != null)
                {
                    ViewBag.message = "This product is already exist";

                    ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
                    ViewData["TagId"] = new SelectList(_db.SpecialTag.ToList(), "Id", "TagName");
                    
                    return View();
                }
                    
                    // save image into local directory 
                if (image != null)
                {
                    var name = Path.Combine(_dir.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));

                    product.Image = "images/" + image.FileName;  // with path .. file name store into database only 

                }

                // defalut image set
                if(image == null)
                {
                    product.Image = "images/10.png";
                }
          
                _db.Products.Add(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTag.ToList(), "Id", "TagName");
            return View(product);
        }

        // ************ Edit *******************
        // get Edit Action Method 
        public IActionResult Edit(int? id)
        {

            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTag.ToList(), "Id", "TagName");
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // post Edit Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                // save image into local directory 
                if (image != null)
                {
                    var name = Path.Combine(_dir.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));

                    product.Image = "images/" + image.FileName;  // with path .. file name store into database only 

                }

                // defalut image set
                if (image == null)
                {
                    product.Image = "images/10.png";
                }

                _db.Products.Update(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTag.ToList(), "Id", "TagName");
            return View(product);
        }

        // ************** Details ********************
        // get Details page 

        public IActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);
            if(product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // post Details page

        // ******************* Delete ********************
        // get delete action method 

        public IActionResult Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).Where(c => c.Id == id).FirstOrDefault();

            if(product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // post delete action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var product = _db.Products.FirstOrDefault(c => c.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();  
            return RedirectToAction(nameof(Index));
        }

    }
}
