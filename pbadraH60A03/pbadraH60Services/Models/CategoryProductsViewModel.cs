namespace pbadraH60Services.Models
{
    public class CategoryProductsViewModel
    {
        public string CategoryName { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
