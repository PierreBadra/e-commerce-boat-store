using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace pbadraH60Services.Models;

public partial class Product
{
    public int ProductId { get; set; }
    public int ProdCatId { get; set; }
    public string? Description { get; set; }
    public string? Manufacturer { get; set; }
    public int? Stock { get; set; }
    public decimal? BuyPrice { get; set; }
    public decimal? SellPrice { get; set; }
    
    public string? ImageName { get; set; }
        
    public byte[]? ImageFile { get; set; }

    [JsonIgnore]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [JsonIgnore]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [JsonIgnore]
    public virtual ProductCategory ProdCat { get; set; } = null!;
}
