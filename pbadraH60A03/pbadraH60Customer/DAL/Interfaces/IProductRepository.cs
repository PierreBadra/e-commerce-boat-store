using pbadraH60Customer.DTO;

namespace pbadraH60Customer.DAL;

public interface IProductRepository
{
    public Task<List<ProductCustomerDisplayDTO>> GetProducts();
    public Task<ProductCustomerDisplayDTO> GetProduct(int id);
}