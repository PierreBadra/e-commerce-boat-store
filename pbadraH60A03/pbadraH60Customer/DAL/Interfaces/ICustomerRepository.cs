using pbadraH60Customer.Models;

namespace pbadraH60Customer.DAL
{
    public interface ICustomerRepository
    {
        public Task<bool> Create(Customer newCustomer);
        public Task<List<Customer>> Read();
        public Task<bool> Update(Customer updatedCustomer);
        public Task<Customer> Find(int customerId);
        public Task<bool> Delete(Customer deletedCustomer);

        public Task<Customer> FindByUserId(string userId);
    }
}
