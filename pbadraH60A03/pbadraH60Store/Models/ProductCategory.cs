using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using pbadraH60A01.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pbadraH60A01.Models
{

    public class ProductCategory
    {
        [Key]
        public int CategoryId { get; set; }

        [Display(Name = "Category Name")]
        public string ProdCat { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
