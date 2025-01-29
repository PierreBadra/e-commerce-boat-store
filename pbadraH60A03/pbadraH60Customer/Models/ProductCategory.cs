using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace pbadraH60Customer.Models;

[PrimaryKey(nameof(CategoryId))]
public partial class ProductCategory
{
    public int CategoryId { get; set; }

    public string ProdCat { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
