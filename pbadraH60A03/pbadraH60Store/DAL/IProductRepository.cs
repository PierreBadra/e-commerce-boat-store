using pbadraH60A01.Models;

namespace pbadraH60A01.Services
{
    public interface IProductRepository
    {
        public Task<bool> Create(Product newProduct);
        public Task<List<Product>> Read();
        public Task<bool> Update(Product updatedProduct);
        public Task<bool> Delete(Product deletedProduct);
        public Task<Product> Find(int productId);
        public Task<Product> FindWithCategory(int productId);
        public Task<bool> UpdateStock(Product updatedProduct);
        public Task<bool> UpdateSellPrice(Product updatedProduct);
        public Task<bool> UpdateBuyPrice(Product updatedProduct);
        public Task<CategoryProductsViewModel> ReadByCategory(int categoryId);
    }
}
