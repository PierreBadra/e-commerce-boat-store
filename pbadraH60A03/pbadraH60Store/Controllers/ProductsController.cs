using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pbadraH60A01.Models;
using pbadraH60A01.Services;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace pbadraH60A01.Controllers
{
    [Authorize(Roles = "Clerk,Manager")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductsController(IProductRepository productRepo, IProductCategoryRepository productCategoryRepo, HttpClient httpClient)
        {
            _productRepository = productRepo;
            _productCategoryRepository = productCategoryRepo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productRepository.Read());
        }

        public async Task<IActionResult> Category(int id)
        {
            var productForCategory = await _productRepository.ReadByCategory(id);
            if (productForCategory == null)
                return NotFound();

            return View(productForCategory);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Categories"] = await _productCategoryRepository.Read();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product newProduct, IFormFile? ImageFile)
        {   
            Dictionary<string, string> errorMessages = new Dictionary<string, string>();
            
            if (ImageFile is { Length: > 0 })
            {
                using (var memoryStream = new MemoryStream())
                {
                    await ImageFile.CopyToAsync(memoryStream);
                    newProduct.ImageFile = memoryStream.ToArray();
                    newProduct.ImageName = ImageFile.FileName;
                }
            }
            
            if (!Validation.ValidateProductForCreate(newProduct, out errorMessages))
            {
                foreach (var error in errorMessages)
                {
                    ModelState.Remove(error.Key);
                    ModelState.AddModelError(error.Key, error.Value);
                }
                ViewData["Categories"] = await _productCategoryRepository.Read();
                return View(newProduct);
            }
            
            if (await _productRepository.Create(newProduct))
            {
                TempData["SuccessMessage"] = "Product created successfully!";
                return RedirectToAction("Index");
            }

            ViewData["Categories"] = await _productCategoryRepository.Read();
            return View(newProduct);
        }

        
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["Categories"] = await _productCategoryRepository.Read();
            var existingProduct = await _productRepository.Find(product.ProductId);
            ViewData["Description"] = existingProduct.Description;
            ViewData["ImageFile"] = existingProduct.ImageFile;
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product updatedProduct, IFormFile? ImageFile, string? deleteImage)
        {   
            Dictionary<string, string> errorMessages = new Dictionary<string, string>();
            bool isRemovedImage = !string.IsNullOrEmpty(deleteImage);
            
            if (isRemovedImage) {
                updatedProduct.ImageFile = null;
                updatedProduct.ImageName = null;
            }
            if (ImageFile is { Length: > 0 }) {
                using (var memoryStream = new MemoryStream()) {
                    await ImageFile.CopyToAsync(memoryStream);
                    updatedProduct.ImageFile = memoryStream.ToArray();
                    updatedProduct.ImageName = ImageFile.FileName;
                }
            }
            
            var existingProduct = await _productRepository.Find(updatedProduct.ProductId);
            
            if (!isRemovedImage && ImageFile == null)
            {
                updatedProduct.ImageFile = existingProduct.ImageFile;
                updatedProduct.ImageName = existingProduct.ImageName;
            }
            
            if (!Validation.ValidateProductForUpdate(existingProduct, updatedProduct, out errorMessages))
            {
                foreach (var error in errorMessages)
                {
                    ModelState.Remove(error.Key);
                    ModelState.AddModelError(error.Key, error.Value);
                }
                
                ViewData["Categories"] = await _productCategoryRepository.Read();
                ViewData["Description"] = existingProduct.Description;
                ViewData["ImageFile"] = existingProduct.ImageFile;
                return View(updatedProduct);
            }

            
            
            if (await _productRepository.Update(updatedProduct))
            {
                TempData["SuccessMessage"] = "Product updated successfully!";
                return RedirectToAction("Index");
            }

            ViewData["Categories"] = await _productCategoryRepository.Read();
            ViewData["Description"] = existingProduct.Description;
            ViewData["ImageFile"] = existingProduct.ImageFile;
            return View(updatedProduct);
        }


        [HttpGet]
        public async Task<IActionResult> EditStock(int id)
        {
            var product = await _productRepository.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStock(Product updatedProduct)
        {
            string errorMessage;
            var existingProduct = await _productRepository.Find(updatedProduct.ProductId);
            if (!Validation.ValidateStock(updatedProduct.Stock.ToString(), existingProduct.Stock.ToString(), out errorMessage))
            {
                ModelState.Remove("Stock");
                ModelState.AddModelError("Stock", errorMessage);
 
            }

            if (await _productRepository.UpdateStock(updatedProduct))
            {
                TempData["SuccessMessage"] = "Product stock updated successfully!";
                return RedirectToAction("Index");
            }

            ViewData["Description"] = existingProduct.Description;
            return View(updatedProduct);
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> EditBuyPrice(int id)
        {
            var product = await _productRepository.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> EditBuyPrice(Product updatedProduct)
        {
            string errorMessage;
            var existingProduct = await _productRepository.Find(updatedProduct.ProductId);
            if (!Validation.ValidateBuyPrice(updatedProduct.SellPrice.ToString(), updatedProduct.BuyPrice.ToString(), out errorMessage))
            {
                ModelState.Remove("BuyPrice");
                ModelState.AddModelError("BuyPrice", errorMessage);

            }

            if (await _productRepository.UpdateBuyPrice(updatedProduct))
            {
                TempData["SuccessMessage"] = "Product buy price updated successfully!";
                return RedirectToAction("Index");
            }

            ViewData["Description"] = existingProduct.Description;
            return View(updatedProduct);
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> EditSellPrice(int id)
        {
            var product = await _productRepository.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> EditSellPrice(Product updatedProduct)
        {
            string errorMessage;
            var existingProduct = await _productRepository.Find(updatedProduct.ProductId);
            if (!Validation.ValidateSellPrice(updatedProduct.SellPrice.ToString(), updatedProduct.BuyPrice.ToString(), out errorMessage))
            {
                ModelState.Remove("SellPrice");
                ModelState.AddModelError("SellPrice", errorMessage);

            }

            if (await _productRepository.UpdateSellPrice(updatedProduct))
            {
                TempData["SuccessMessage"] = "Product sell price updated successfully!";
                return RedirectToAction("Index");
            }

            ViewData["Description"] = existingProduct.Description;
            return View(updatedProduct);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.FindWithCategory(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var existingCategory = await _productRepository.Find(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            if (!await _productRepository.Delete(existingCategory))
            {
                TempData["ErrorMessage"] = "Could not delete product.";
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = "Product deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}