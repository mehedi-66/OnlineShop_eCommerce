using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Utiliy;
using System.Diagnostics;
using X.PagedList;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private ApplicationDbContext _db;

  

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index(int? page)
        {
            return View(_db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList().ToPagedList(page??1, 9));
        }
         
        // Get product Details action method
        public IActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var products = _db.Products.Include(c => c.ProductTypes).FirstOrDefault(c => c.Id == id);
            if(products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // post product details action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Details")]
        public IActionResult ProductDetail(int? id)
        {
            // when use add product to card we .. store into list 
            List<Product>? products = new List<Product>();
            if(id == null)
            {
                return NotFound();
            }
            var data = _db.Products.Include(c => c.ProductTypes).FirstOrDefault(c => c.Id == id);
            if(data == null)
            {
                return NotFound();
            }

            // first of we have to get session data ... then .. add new data 
            products = HttpContext.Session.Get<List<Product>>("products");
            
            if(products == null)
            {
                products = new List<Product>();
            }
            // after geting the product data from the databse store into ... session
            products.Add(data);
            HttpContext.Session.Set("products", products); // sotre that list

            return View(data);

        }

        // Get Remove to Cart 
        [ActionName("Remove")]
        public IActionResult RemoveToCart(int? id)
        {
            List<Product>? products = HttpContext.Session.Get<List<Product>>("products");

            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    // remove data from the list of product
                    products.Remove(product);

                    // after removing the data list session data will be updated
                    HttpContext.Session.Set("products", products);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // Remove from the  Cart 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int? id)
        {
            List<Product>? products = HttpContext.Session.Get<List<Product>>("products");
           
            if(products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if(product != null)
                {
                    // remove data from the list of product
                    products.Remove(product);

                    // after removing the data list session data will be updated
                    HttpContext.Session.Set("products", products);
                }
            }
            
            return RedirectToAction(nameof(Index));
        }

        // Get product Cart action method
        public IActionResult Cart()
        {
            // retive Data from session 
            List<Product>? products = HttpContext.Session.Get<List<Product>>("products");
            if(products == null)
            {
                products = new List<Product>();
            }

            return View(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}