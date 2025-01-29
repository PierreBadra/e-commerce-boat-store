using pbadraH60Services.Models;

namespace pbadraH60Services.DAL
{
    public interface ICustomerRepository
    {
        public Task<bool> Create(Customer newCustomer);
        public List<Customer> Read();
        public Task<bool> Update(Customer updatedCustomer);
        public Customer Find(int customerId);
        public Task<bool> Delete(Customer deletedCustomer);

        public Customer FindByUserId(string userId);
    }
}
