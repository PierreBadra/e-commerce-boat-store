using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using pbadraH60A01.Models;
using pbadraH60A01.Services;
using System.Text;

namespace pbadraH60A01.Controllers
{
    [Authorize(Roles = "Clerk,Manager")]
    public class ProductCategoriesController : Controller
    {
        private readonly IProductCategoryRepository _productCategoryRepository;


        public ProductCategoriesController(IProductCategoryRepository productCategoryRepo)
        {
            _productCategoryRepository = productCategoryRepo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productCategoryRepository.Read());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCategory newProductCategory)
        {
            string errorMessage = await _productCategoryRepository.Create(newProductCategory);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ModelState.AddModelError("ProdCat", errorMessage);
                return View(newProductCategory);
            }

            TempData["SuccessMessage"] = "Category created successfully!";
            return RedirectToAction("Index");
        }




        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _productCategoryRepository.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["Category"] = category.ProdCat;
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductCategory updatedProductCategory)
        {
            string errorMessage = await _productCategoryRepository.Update(updatedProductCategory);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ModelState.Remove("ProdCat");
                ModelState.AddModelError("ProdCat", errorMessage);
                var prodCat = await _productCategoryRepository.Find(updatedProductCategory.CategoryId);
                ViewData["Category"] = prodCat.ProdCat;
                return View(updatedProductCategory);
            }

            TempData["SuccessMessage"] = "Category updated successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            string errorMessage;
            var existingCategory = await _productCategoryRepository.Find(id);
            if (existingCategory != null)
            {
                errorMessage = await _productCategoryRepository.Delete(existingCategory);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    TempData["ErrorMessage"] = errorMessage;
                }
                else
                {
                    TempData["SuccessMessage"] = "Category deleted successfully!";
                }
            }
            return RedirectToAction("Index");
        }
    }
}
