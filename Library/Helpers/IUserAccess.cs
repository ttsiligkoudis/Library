using Library.Models;

namespace Library.Helpers
{
    public interface IUserAccess
    {
        bool IsAdmin(Customer customer);
        bool IsCustomer(Customer customer);
    }
}
