using pbadraH60Services.Models;

namespace pbadraH60Services.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public int ProdCatId { get; set; }
        
        public string? CategoryName { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }

        public int? Stock { get; set; }

        public decimal? BuyPrice { get; set; }

        public decimal? SellPrice { get; set; }
        
        public string? ImageName { get; set; }
        
        public byte[]? ImageFile { get; set; }
        
        public ProductDTO() { }

        public ProductDTO(Product product)
        {
            CategoryName = product.ProdCat.ProdCat;
            ProductId = product.ProductId;
            ProdCatId = product.ProdCatId;
            Description = product.Description;
            Manufacturer = product.Manufacturer;
            Stock = product.Stock;
            BuyPrice = product.BuyPrice;
            SellPrice = product.SellPrice;
            ImageName = product.ImageName;
            ImageFile = product.ImageFile;
        }
    }
}
