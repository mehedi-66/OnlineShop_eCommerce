using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class ProductTypes
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Product Type")]  // if we want to display  with space ... or own word
        public string ProductType { get; set; }
    }
}
