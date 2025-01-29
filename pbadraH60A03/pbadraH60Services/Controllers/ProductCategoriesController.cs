using Microsoft.AspNetCore.Mvc;
using pbadraH60Services.Models;
using pbadraH60Services.Services;

namespace pbadraH60Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoriesController(IProductCategoryRepository productCategoryRepo)
        {
            _productCategoryRepository = productCategoryRepo;
        }

        [HttpGet]
        public ActionResult<List<ProductCategory>> AllProductCatgories()
        {
            return _productCategoryRepository.Read();
        }

        [HttpPost]
        public ActionResult<ProductCategory> CreateProductCategory(ProductCategory newProductCategory)
        {
            string errorMessage;
            if (!Validation.ValidateProductCategory(_productCategoryRepository.Read(), newProductCategory, null, out errorMessage))
            {
                return Conflict(errorMessage);
            }

            _productCategoryRepository.Create(newProductCategory);
            return CreatedAtAction(nameof(CreateProductCategory), new {id=newProductCategory.CategoryId}, newProductCategory);
        }


        [HttpGet("{id:int}")]
        public ActionResult<ProductCategory> GetProductCategory(int id)
        {
            var category = _productCategoryRepository.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return category;
        }


        [HttpPut("{id:int}")]
        public ActionResult Edit(int id, ProductCategory updatedProductCategory)
        {
            string errorMessage;
            if (!Validation.ValidateProductCategory(_productCategoryRepository.Read(), updatedProductCategory, id, out errorMessage))
            {
                return Conflict(errorMessage);
            }

            _productCategoryRepository.Update(updatedProductCategory);
            return Ok(updatedProductCategory);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            string errorMessage;
            var existingCategory = _productCategoryRepository.Find(id);
            if (existingCategory != null)
            {
                if (!_productCategoryRepository.Delete(existingCategory, out errorMessage))
                {
                    return Conflict(errorMessage);
                }
                else {
                    return NoContent();
                }
            }
            return NotFound();
        }
    }
}
