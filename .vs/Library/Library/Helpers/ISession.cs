using Library.Models;

namespace Library.Helpers
{
    public interface ISession
    {
        Customer GetCustomer();
    }
}
