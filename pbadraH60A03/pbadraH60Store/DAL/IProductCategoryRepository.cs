using pbadraH60A01.Models;

namespace pbadraH60A01.Services
{
    public interface IProductCategoryRepository
    {
        public Task<string> Create(ProductCategory newProductCategory);
        public Task<List<ProductCategory>> Read();
        public Task<string> Update(ProductCategory updatedCategory);
        public Task<string> Delete(ProductCategory deletedCategory);
        public Task<ProductCategory> Find(int categoryId);
    }
}
