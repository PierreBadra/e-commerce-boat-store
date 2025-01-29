using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pbadraH60A01.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }

        public int ProdCatId { get; set; }

        public string? Description { get; set; }

        public string? Manufacturer { get; set; }

        public int? Stock { get; set; }

        [Display(Name = "Buy Price")]
        public decimal? BuyPrice { get; set; }

        [Display(Name = "Sell Price")]
        public decimal? SellPrice { get; set; }
        public string? ImageName { get; set; }
        public byte[]? ImageFile { get; set; }

        [ValidateNever]
        public virtual ProductCategory ProdCat { get; set; } = null!;
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }

    }
}
