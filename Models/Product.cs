﻿
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }

        public string? Image { get; set; } // only the image name save into database ... file store in local storage
        [Required]
        [Display(Name="Product Color")]
        public string ProductColor { get; set; }
        [Required]
        [Display(Name="Available")]
        public bool IsAvailable { get; set; }
        [Required]
        [Display(Name="Product Type")]
        public int ProductTypeId { get; set; }

        [ForeignKey("ProductTypeId")]
        public ProductTypes? ProductTypes { get; set; }
        [Required]
        [Display(Name="Secial Tag")]
        public int SpecialTagId { get; set; }
        [ForeignKey("SpecialTagId")]
        public SpecialTag? SpecialTag { get; set; }    
    }
}