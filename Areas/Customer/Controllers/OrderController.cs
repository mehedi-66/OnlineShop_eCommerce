using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Utiliy;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class OrderController : Controller
    {
        private ApplicationDbContext _db;

        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }
       // Get Checkout Action Method
       public IActionResult Checkout()
       {
            return View();
       }

        // Post Checkout Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order anOrder)
        {
            
                List<Product> products = HttpContext.Session.Get<List<Product>>("products");
                if (products != null)
                {
                    foreach (var product in products)
                    {
                        OrderDetails orderDetails = new OrderDetails();
                        orderDetails.ProductId = product.Id;
                        anOrder.Details.Add(orderDetails);
                    }
                }

                anOrder.OrderNo = GetOrderNo();
                if(ModelState.IsValid)
                {
                    _db.Orders.Add(anOrder);
                    await _db.SaveChangesAsync();
                 }
               

                // session data null
                HttpContext.Session.Set("Products", new List<Product>());
          
           
             
            
            return View(); 
        }

        public string GetOrderNo()
        {
            int rowCount = _db.Orders.ToList().Count() + 1;
            string s = rowCount.ToString("000");
            return s;
        }
    }
}
