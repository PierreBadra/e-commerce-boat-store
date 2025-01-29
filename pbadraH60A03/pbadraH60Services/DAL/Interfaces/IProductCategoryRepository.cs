using pbadraH60Services.Models;

namespace pbadraH60Services.Services
{
    public interface IProductCategoryRepository
    {
        public bool Create(ProductCategory newProductCategory);
        public List<ProductCategory> Read();
        public bool Update(ProductCategory updatedCategory);
        public bool Delete(ProductCategory deletedCategory, out string errorMessage);
        public ProductCategory Find(int categoryId);
    }
}
