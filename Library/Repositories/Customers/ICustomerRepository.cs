using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Models;

namespace Library.Repositories.Customers
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetCustomers();
        Task<Customer> GetCustomer(int id);
        Task<Customer> GetCustomerByUserId(int id);
        Task<Customer> Post(Customer customer);
        Task<Customer> Put(Customer customer);
        Task<object> Delete(int id);
    }
}