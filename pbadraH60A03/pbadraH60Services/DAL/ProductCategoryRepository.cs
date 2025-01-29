using pbadraH60Services.Models;

namespace pbadraH60Services.Services
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly H60assignment2DbPbadraContext _context;

        public ProductCategoryRepository(H60assignment2DbPbadraContext context)
        {
            _context = context;
        }

        public bool Create(ProductCategory newProductCategory)
        {
            try
            {
                _context.ProductCategories.Add(newProductCategory);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<ProductCategory> Read()
        {
            try
            {
                return _context.ProductCategories.OrderBy(x => x.ProdCat).ToList();
            }
            catch (Exception)
            {
                return new List<ProductCategory>();
            }
        }

        public bool Update(ProductCategory updatedCategory)
        {
            try
            {
                var existingCategory = _context.ProductCategories.Find(updatedCategory.CategoryId);
                if (existingCategory == null)
                {
                    return false;
                }

                _context.Entry(existingCategory).CurrentValues.SetValues(updatedCategory);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(ProductCategory deletedCategory, out string errorMessage)
        {
            try
            {
                var categoryToDelete = _context.ProductCategories.Find(deletedCategory.CategoryId);
                if (categoryToDelete == null)
                {
                    throw new Exception("The product category was not found.");
                }

                if (_context.Products.Any(p => p.ProdCatId == deletedCategory.CategoryId))
                {
                    throw new Exception("One or many products are associated with the category. To successfully delete a category, you must delete all products from the category beforehand.");
                }

                if (_context.ProductCategories.Count() == 1)
                {
                    throw new Exception("The last remaining category cannot be deleted. There must always be at least one category.");
                }

                _context.ProductCategories.Remove(categoryToDelete);
                _context.SaveChanges();
                errorMessage = "";
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public ProductCategory Find(int categoryId)
        {
            var existingCategory = _context.ProductCategories.Find(categoryId);
            return existingCategory;
        }
    }
}
