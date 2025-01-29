using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace pbadraH60A01.Models
{
    public class Validation
    {
        public static bool ValidateSellPrice(string? updatedProductSellPrice, string? updatedProductBuyPrice, out string errorMessage)
        {

            if (string.IsNullOrEmpty(updatedProductSellPrice))
            {
                errorMessage = "The sell price is required.";
                return false;
            }

            if (!decimal.TryParse(updatedProductSellPrice, out decimal sellPriceValue))
            {
                errorMessage = "The sell price must be a number.";
                return false;
            }

            if (decimal.Parse(updatedProductSellPrice) >= 1000000)
            {
                errorMessage = "The sell price cannot be or exceed $1,000,000.";
                return false;
            }


            if (decimal.Parse(updatedProductSellPrice) < 0)
            {
                errorMessage = "The sell price cannot be negative.";
                return false;
            }

            if (decimal.Parse(updatedProductSellPrice) < decimal.Parse(updatedProductBuyPrice))
            {
                errorMessage = "The sell price cannot be lower than the product's buy price.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        public static bool ValidateBuyPrice(string? updatedProductSellPrice, string? updatedProductBuyPrice, out string errorMessage)
        {
            if (string.IsNullOrEmpty(updatedProductBuyPrice))
            {
                errorMessage = "The buy price is required.";
                return false;
            }

            if (!decimal.TryParse(updatedProductBuyPrice, out decimal buyPriceValue))
            {
                errorMessage = "The buy price must be a number.";
                return false;
            }

            if (decimal.Parse(updatedProductBuyPrice) >= 1000000)
            {
                errorMessage = "The buy price cannot be or exceed $1,000,000.";
                return false;
            }


            if (decimal.Parse(updatedProductBuyPrice) < 0)
            {
                errorMessage = "The buy price cannot be negative.";
                return false;
            }

            if (decimal.Parse(updatedProductBuyPrice) > decimal.Parse(updatedProductSellPrice))
            {
                errorMessage = "The buy price cannot be higher than the product's sell price.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        public static bool ValidateStock(string? updatedProductStock, string? existingProductStock, out string errorMessage)
        {
            if (updatedProductStock.IsNullOrEmpty())
            {
                errorMessage = "The stock increment/decrement is required.";
                return false;
            }

            if (!int.TryParse(updatedProductStock, out int value))
            {
                errorMessage = "Stock must be an integer number.";
                return false;
            }

            if (int.Parse(existingProductStock) + int.Parse(updatedProductStock) < 0)
            {
                errorMessage = "Insufficient stock. Stock cannot be less than zero.";
                return false;
            }

            if (int.Parse(existingProductStock) + int.Parse(updatedProductStock) >= 2000000)
            {
                errorMessage = "The stock cannot be or exceed 2,000,000.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        public static bool ValidateProductForCreate(Product product, out Dictionary<string, string> errorMessages)
        {
            errorMessages = new Dictionary<string, string>();

            if (product.Description == null)
            {
                errorMessages.Add("Description", "The description field is required");
            }

            if (product.Manufacturer == null)
            {
                errorMessages.Add("Manufacturer", "The manufacturer field is required");
            }

            if (product.Stock == null)
            {
                errorMessages.Add("Stock", "The stock field is required.");
            } else if (!int.TryParse(product.Stock.ToString(), out int result)) {
                errorMessages.Add("Stock", "The stock must be a number.");
            } else if (product.Stock < 0) {
                errorMessages.Add("Stock", "The stock cannot be negative.");
            } else if (product.Stock >= 2000000){    
                errorMessages.Add("Stock", "The stock cannot be or exceed 2,000,000.");
            }

            if (product.SellPrice == null)
            {
                errorMessages.Add("SellPrice", "The sell price field is required.");
            } else if (!decimal.TryParse(product.SellPrice.ToString(), out decimal value))
            {
                errorMessages.Add("SellPrice", "The sell price must be a number.");
            } else if (product.SellPrice < 0)
            {
                errorMessages.Add("SellPrice", "The sell price cannot be negative.");

            } else if (product.SellPrice >= 1000000)
            {
                errorMessages.Add("SellPrice", "The sell price cannot be or exceed $1,000,000.");
            } else if (product.SellPrice < product.BuyPrice)
            {
                errorMessages.Add("SellPrice", "The sell price cannot be lower than the buy price.");
            }

            if (product.BuyPrice == null)
            {
                errorMessages.Add("BuyPrice", "The buy price field is required.");
            }
            else if (!decimal.TryParse(product.BuyPrice.ToString(), out decimal value))
            {
                errorMessages.Add("BuyPrice", "The buy price must be a number.");
            }
            else if (product.BuyPrice < 0)
            {
                errorMessages.Add("BuyPrice", "The buy price cannot be negative.");

            }
            else if (product.BuyPrice >= 1000000)
            {
                errorMessages.Add("BuyPrice", "The buy price cannot be or exceed $1,000,000.");
            }
            else if (product.BuyPrice > product.SellPrice)
            {
                errorMessages.Add("BuyPrice", "The buy price cannot be higher than the sell price.");
            }

            if (product.ProdCatId == 0)
            {
                errorMessages.Add("ProdCatId", "The category field is required.");
            }
            
            else if (product.ImageFile != null && product.ImageFile.Length > 0)
            {
                if (!IsValidImageFile(product.ImageFile))
                {
                    errorMessages.Add("ImageFile", "The image file type is invalid. Allowed types are .jpg, .jpeg, .png, .gif, .bmp, .tiff, .webp.");
                }
            }
            
            return errorMessages.Count() == 0;
        }

        public static bool ValidateProductForUpdate(Product existingProduct, Product updatedproduct, out Dictionary<string, string> errorMessages)
        {
            errorMessages = new Dictionary<string, string>();

            if (updatedproduct.Description == null)
            {
                errorMessages.Add("Description", "The description field is required.");
            }

            if (updatedproduct.Manufacturer == null)
            {
                errorMessages.Add("Manufacturer", "The manufacturer field is required.");
            }

            if (updatedproduct.Stock == null)
            {
                errorMessages.Add("Stock", "The stock field is required.");
            }
            else if (!int.TryParse(updatedproduct.Stock.ToString(), out int result))
            {
                errorMessages.Add("Stock", "The stock must be a number.");
            }
            else if (updatedproduct.Stock + existingProduct.Stock < 0)
            {
                errorMessages.Add("Stock", "The stock increment/decrement cannot be negative.");
            }
            else if (updatedproduct.Stock + existingProduct.Stock  >= 2000000)
            {
                errorMessages.Add("Stock", "The stock increment/decrement cannot be or exceed 2,000,000.");
            }

            if (updatedproduct.SellPrice == null)
            {
                errorMessages.Add("SellPrice", "The sell price field is required.");
            }
            else if (!decimal.TryParse(updatedproduct.SellPrice.ToString(), out decimal value))
            {
                errorMessages.Add("SellPrice", "The sell price must be a number.");
            }
            else if (updatedproduct.SellPrice < 0)
            {
                errorMessages.Add("SellPrice", "The sell price cannot be negative.");

            }
            else if (updatedproduct.SellPrice >= 1000000)
            {
                errorMessages.Add("SellPrice", "The sell price cannot be or exceed $1,000,000.");
            }
            else if (updatedproduct.SellPrice < updatedproduct.BuyPrice)
            {
                errorMessages.Add("SellPrice", "The sell price cannot be lower than the buy price.");
            }

            if (updatedproduct.BuyPrice == null)
            {
                errorMessages.Add("BuyPrice", "The buy price field is required.");
            }
            else if (!decimal.TryParse(updatedproduct.BuyPrice.ToString(), out decimal value))
            {
                errorMessages.Add("BuyPrice", "The buy price must be a number.");
            }
            else if (updatedproduct.BuyPrice < 0)
            {
                errorMessages.Add("BuyPrice", "The buy price cannot be negative.");

            }
            else if (updatedproduct.BuyPrice >= 1000000)
            {
                errorMessages.Add("BuyPrice", "The buy price cannot be or exceed $1,000,000.");
            }
            else if (updatedproduct.BuyPrice > updatedproduct.SellPrice)
            {
                errorMessages.Add("BuyPrice", "The buy price cannot be higher than the sell price.");
            }

            if (updatedproduct.ProdCatId == 0)
            {
                errorMessages.Add("ProdCatId", "The category field is required.");
            } 
            
            if (updatedproduct.ImageFile != null && updatedproduct.ImageFile.Length > 0)
            {
                if (!IsValidImageFile(updatedproduct.ImageFile))
                {
                    errorMessages.Add("ImageFile", "The image file type is invalid. Allowed types are .jpg, .jpeg, .png, .gif, .bmp, .tiff, .webp.");
                }
            }

            return errorMessages.Count() == 0;
        }

        public static bool ValidateProductCategory(List<ProductCategory> existingProductCategories, ProductCategory newProductCategory, int? categoryIdBeingEdited, out string errorMessage)
        {
            if (string.IsNullOrEmpty(newProductCategory.ProdCat))
            {
                errorMessage = "The category name field is required.";
                return false;
            }

            if (existingProductCategories
                .Where(p => !categoryIdBeingEdited.HasValue || p.CategoryId != categoryIdBeingEdited.Value)
                .Any(p => p.ProdCat.Equals(newProductCategory.ProdCat, StringComparison.OrdinalIgnoreCase)))
            {
                errorMessage = "The category already exists";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
        
        private static bool IsValidImageFile(byte[] fileBytes)
        {
            var imageSignatures = new Dictionary<string, byte[]>
            {
                { "jpeg", new byte[] { 0xFF, 0xD8, 0xFF } },
                { "png", new byte[] { 0x89, 0x50, 0x4E, 0x47 } },
                { "gif", new byte[] { 0x47, 0x49, 0x46, 0x38 } },
                { "bmp", new byte[] { 0x42, 0x4D } },
                { "tiff-le", new byte[] { 0x49, 0x49, 0x2A, 0x00 } },
                { "tiff-be", new byte[] { 0x4D, 0x4D, 0x00, 0x2A } },
                { "webp", new byte[] { 0x52, 0x49, 0x46, 0x46 } }
            };
            
            foreach (var signature in imageSignatures.Values)
            {
                if (fileBytes.Take(signature.Length).SequenceEqual(signature))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
