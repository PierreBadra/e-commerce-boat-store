
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using pbadraH60Services.DTO;
using pbadraH60Services.Models;
using pbadraH60Services.Services;

namespace pbadraH60Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductsController(IProductRepository productRepo, IProductCategoryRepository productCategoryRepo)
        {
            _productRepository = productRepo;
            _productCategoryRepository = productCategoryRepo;
        }

        [HttpGet]
        public ActionResult<List<Product>> AllProducts()
        {
            return _productRepository.Read();
        }

        [HttpGet("/api/ManagerProducts/")]
        public ActionResult<List<ProductDTO>> AllProductsManager()
        {
            return _productRepository.Read().Select(p => new ProductDTO(p)).ToList();
        }

        [HttpGet("Category/{id:int}")]
        public ActionResult<CategoryProductsViewModel> AllProductsOfCategory(int id)
        {
            var productForCategory = _productRepository.ReadByCategory(id);
            if (productForCategory == null)
                return NotFound();

            return productForCategory;
        }

        [HttpPost]
        public ActionResult Create(ProductDTO newProductDto)
        {
            Dictionary<string, string> errorMessages;

            var prod = new Product()
            {
                ProductId = newProductDto.ProductId,
                ProdCatId = newProductDto.ProdCatId,
                Description = newProductDto.Description,
                BuyPrice = newProductDto.BuyPrice,
                SellPrice = newProductDto.SellPrice,
                Stock = newProductDto.Stock,
                Manufacturer = newProductDto.Manufacturer,
                ImageFile = newProductDto.ImageFile,
                ImageName = newProductDto.ImageName,
            };

            if (!_productRepository.Create(prod, out errorMessages))
            {
                return Conflict(errorMessages);
            }

            return CreatedAtAction(nameof(Create), new { id = newProductDto.ProductId }, newProductDto);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = _productRepository.FindWithCategory(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPut("{id:int}")]
        public ActionResult Edit(int id, ProductDTO updatedProductDto)
        {
            Dictionary<string, string> errorMessages;

            var updatedProduct = new Product()
            {
                ProductId = updatedProductDto.ProductId,
                ProdCatId = updatedProductDto.ProdCatId,
                Description = updatedProductDto.Description,
                Manufacturer = updatedProductDto.Manufacturer,
                BuyPrice = updatedProductDto.BuyPrice,
                SellPrice = updatedProductDto.SellPrice,
                Stock = updatedProductDto.Stock,
                ImageFile = updatedProductDto.ImageFile,
                ImageName =  updatedProductDto.ImageName
            };

            if (!_productRepository.Update(updatedProduct, out errorMessages))
            {
                return Conflict(errorMessages);
            }
            return Ok(updatedProduct);
        }


        [HttpPatch("ProductStock/{id:int}")]
        public ActionResult EditStock(int id, ProductStockDTO updateProductStockDto)
        {
            string errorMessage;
            var updatedProduct = new Product()
            {
                ProductId = updateProductStockDto.ProductId,
                Stock = updateProductStockDto.Stock
            };

            if (!_productRepository.UpdateStock(updatedProduct, out errorMessage))
            {
                return Conflict(errorMessage);
            }
            var updProduct =_productRepository.Find(id);
            return Ok(updProduct);
        }


        [HttpPatch("ProductBuyPrice/{id:int}")]
        public ActionResult EditBuyPrice(int id, ProductBuyPriceDTO updatedProductBuyPriceDto)
        {
            string errorMessage;

            var updatedProduct = new Product()
            {
                ProductId = updatedProductBuyPriceDto.ProductId,
                BuyPrice = updatedProductBuyPriceDto.BuyPrice,
                SellPrice = updatedProductBuyPriceDto.SellPrice,
            };

            if (!_productRepository.UpdateBuyPrice(updatedProduct, out errorMessage))
            {
                return Conflict(errorMessage);
            }

            return Ok(updatedProduct);
        }


        [HttpPatch("ProductSellPrice/{id:int}")]
        public ActionResult EditSellPrice(int id, ProductSellPriceDTO updatedProductSellPriceDto)
        {
            string errorMessage;

            var updatedProduct = new Product()
            {
                ProductId = updatedProductSellPriceDto.ProductId,
                BuyPrice = updatedProductSellPriceDto.BuyPrice,
                SellPrice = updatedProductSellPriceDto.SellPrice,
            };
            if (!_productRepository.UpdateSellPrice(updatedProduct, out errorMessage))
            {
                return Conflict(errorMessage);
            }
            return Ok(updatedProduct);
        }


        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var existingCategory = _productRepository.Find(id);
            if (existingCategory != null)
            {
                if (!_productRepository.Delete(existingCategory))
                {
                    return Conflict();
                }
            }
            return NoContent();
        }

        [HttpGet("/api/CustomerProducts/Details/{id:int}")]
        public ActionResult<ProductCustomerDisplayDTO> GetProductCustomerDisplay(int id)
        {
            var product = _productRepository.FindWithCategory(id);
            
            if (product == null)
            {
                return NotFound();
            }
            
            var prod = new ProductCustomerDisplayDTO() {
                ProductId = product.ProductId,
                Description = product.Description,
                categoryName = product.ProdCat.ProdCat,
                SellPrice = product.SellPrice,
                Manufacturer = product.Manufacturer,
                Stock = product.Stock,
                ImageFile = product.ImageFile
            };
            
            return prod;
        }

        [HttpGet("/api/CustomerProducts")]
        public ActionResult<List<ProductCustomerDisplayDTO>> GetProductCustomerDisplay([FromQuery] string? search)
        {
            List<ProductCustomerDisplayDTO> products = null;
            
            if (search == null)
            {
                products = _productRepository.Read().Select(x => new ProductCustomerDisplayDTO()
                {
                    ProductId = x.ProductId,
                    Description = x.Description,
                    categoryName = x.ProdCat.ProdCat,
                    SellPrice = x.SellPrice,
                    Manufacturer = x.Manufacturer,
                    Stock = x.Stock,
                    ImageFile = x.ImageFile
                }).ToList();
            }
            else
            {
                products = _productRepository.Read().Select(x => new ProductCustomerDisplayDTO()
                {
                    ProductId = x.ProductId,
                    Description = x.Description,
                    categoryName = x.ProdCat.ProdCat,
                    SellPrice = x.SellPrice,
                    Manufacturer = x.Manufacturer,
                    Stock = x.Stock,
                    ImageFile = x.ImageFile
                }).Where(x => x.categoryName.ToLower().Contains(search.ToLower())  || x.Description.ToLower().Contains(search.ToLower())).ToList();
            }

            if (products == null)
            {
                return NotFound();
            }
            
            return products;
        }
    }
}