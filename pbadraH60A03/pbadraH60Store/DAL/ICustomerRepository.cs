using pbadraH60A01.Models;

namespace pbadraH60A01.DAL
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
