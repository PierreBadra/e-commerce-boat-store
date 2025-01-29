using pbadraH60Services.DTO;
using pbadraH60Services.Models;

namespace pbadraH60Services.Services
{
    public interface IProductRepository
    {
        public bool Create(Product newProduct, out Dictionary<string, string> errorMessages);
        public List<Product> Read();
        public bool Update(Product updatedProduct, out Dictionary<string, string> errorMessages);
        public bool Delete(Product deletedProduct);
        public Product Find(int productId);
        public Product FindWithCategory(int productId);
        public bool UpdateStock(Product updatedProduct, out string errorMessage);
        public bool UpdateSellPrice(Product updatedProduct, out string errorMessage);
        public bool UpdateBuyPrice(Product updatedProduct, out string errorMessage);
        public CategoryProductsViewModel ReadByCategory(int categoryId);
    }
}
