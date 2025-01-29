using pbadraH60Services.Models;

namespace pbadraH60Services.DTO;

public class ProductCustomerDisplayDTO
{
    public int? ProductId { get; set; }
    public string? categoryName { get; set; }
    
    public string? Description { get; set; }
    
    public string? Manufacturer { get; set; }
    
    public int? Stock { get; set; }
    
    public decimal? SellPrice { get; set; }
    
    public byte[]? ImageFile { get; set; }
    
    public ProductCustomerDisplayDTO() { }

    public ProductCustomerDisplayDTO(Product product)
    {
        ProductId = product.ProductId;
        Description = product.Description;
        categoryName = product.ProdCat.ProdCat;
        Manufacturer = product.Manufacturer;
        Stock = product.Stock;
        SellPrice = product.SellPrice;
        ImageFile = product.ImageFile;
    }
}