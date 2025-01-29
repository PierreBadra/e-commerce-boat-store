using Microsoft.EntityFrameworkCore;
using pbadraH60Services.DTO;
using pbadraH60Services.Models;

namespace pbadraH60Services.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly H60assignment2DbPbadraContext _context;

        public ProductRepository(H60assignment2DbPbadraContext context)
        {
            _context = context;
        }

        public bool Create(Product newProduct, out Dictionary<string, string> errorMessages)
        {
            try
            {
                if (Validation.ValidateProductForCreate(newProduct, out errorMessages))
                {
                    newProduct.ProdCat = null;
                    _context.Products.Add(newProduct);
                    _context.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                Validation.ValidateProductForCreate(newProduct, out errorMessages);
                return false;
            }
        }

        public List<Product> Read()
        {
            try {
                return _context.Products.Include(c => c.ProdCat).OrderBy(x => x.ProdCat.ProdCat).ThenBy(x => x.Description).ToList();
            }
            catch (Exception)
            {
                return new List<Product>();
            }
        }

        public bool Update(Product updatedProduct, out Dictionary<string, string> errorMessages)
        {
            var existingProduct = _context.Products.FirstOrDefault(x => x.ProductId == updatedProduct.ProductId);
            try
            {
                if (Validation.ValidateProductForUpdate(existingProduct, updatedProduct, out errorMessages))
                {
                    existingProduct.Description = updatedProduct.Description;
                    existingProduct.Manufacturer = updatedProduct.Manufacturer;
                    existingProduct.ProdCatId = updatedProduct.ProdCatId;
                    existingProduct.ProdCat = null;
                    existingProduct.BuyPrice =  updatedProduct.BuyPrice;
                    existingProduct.SellPrice =  updatedProduct.SellPrice;
                    existingProduct.Stock += updatedProduct.Stock;
                    existingProduct.ImageFile = updatedProduct.ImageFile;
                    existingProduct.ImageName = updatedProduct.ImageName;
                    _context.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                Validation.ValidateProductForUpdate(existingProduct, updatedProduct, out errorMessages);
                return false;
            }
        }

        public bool Delete(Product deletedProduct)
        {
            try
            {
                var productToDelete = _context.Products.Find(deletedProduct.ProductId);
                if (productToDelete == null)
                {
                    return false;
                }

                _context.Products.Remove(productToDelete);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Product Find(int productId)
        {
            var existingProduct = _context.Products.Find(productId);
            return existingProduct;
        }

        public CategoryProductsViewModel ReadByCategory(int categoryId)
        {
            try
            {
                var products = _context.Products.Include(x => x.ProdCat).Where(x => x.ProdCatId == categoryId).OrderBy(x => x.Description).ToList();

                var categoryName = _context.ProductCategories.FirstOrDefault(x => x.CategoryId == categoryId).ProdCat;
                return new CategoryProductsViewModel
                {
                    Products = products,
                    CategoryName = categoryName
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Product FindWithCategory(int productId)
        {
            return _context.Products.Include(x => x.ProdCat).FirstOrDefault(x => x.ProductId == productId);
        }

        public bool UpdateStock(Product updatedProduct, out string errorMessage)
        {
            var existingProduct = _context.Products.Find(updatedProduct.ProductId);
            try
            {
                if (Validation.ValidateStock(updatedProduct.Stock.ToString(), existingProduct.Stock.ToString(), out errorMessage))
                {
                    existingProduct.Stock += updatedProduct.Stock;
                    _context.SaveChanges();
                    errorMessage = "";
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Validation.ValidateStock(updatedProduct.Stock.ToString(), existingProduct.Stock.ToString(), out errorMessage);
                return false;
            }
        }

        public bool UpdateSellPrice(Product updatedProduct, out string errorMessage)
        {
            var existingProduct = _context.Products.Find(updatedProduct.ProductId);
            try
            {
               
                if (Validation.ValidateSellPrice(updatedProduct.SellPrice.ToString(), updatedProduct.BuyPrice.ToString(), out errorMessage))
                {
                    updatedProduct.SellPrice = Math.Round((decimal)updatedProduct.SellPrice, 2, MidpointRounding.AwayFromZero);
                    existingProduct.SellPrice = updatedProduct.SellPrice;
                    _context.SaveChanges();
                    errorMessage = "";
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Validation.ValidateSellPrice(updatedProduct.SellPrice.ToString(), updatedProduct.BuyPrice.ToString(), out errorMessage);
                return false;
            }
        }

        public bool UpdateBuyPrice(Product updatedProduct, out string errorMessage)
        {
            var existingProduct = _context.Products.Find(updatedProduct.ProductId);
            try
            {
                if (Validation.ValidateBuyPrice(updatedProduct.SellPrice.ToString(), updatedProduct.BuyPrice.ToString(), out errorMessage))
                {
                    updatedProduct.BuyPrice = Math.Round((decimal)updatedProduct.BuyPrice, 2, MidpointRounding.AwayFromZero);
                    existingProduct.BuyPrice = updatedProduct.BuyPrice;
                    _context.SaveChanges();
                    errorMessage = "";
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Validation.ValidateBuyPrice(updatedProduct.SellPrice.ToString(), updatedProduct.BuyPrice.ToString(), out errorMessage);
                return false;
            }
        }
    }
}
