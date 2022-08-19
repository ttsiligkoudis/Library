using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Models;

namespace Library.Repositories.Users
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsers();
        Task<User> GetUser(int id);
        Task<User> GetUser(string email, string password);
        Task<User> GetUser(string email);
        Task<User> Post(User user);
        Task<User> Put(User user);
        Task<object> Delete(int id);
    }
}