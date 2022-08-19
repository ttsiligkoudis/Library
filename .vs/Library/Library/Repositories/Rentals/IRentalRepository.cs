using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Models;

namespace Library.Repositories.Rentals
{
    public interface IRentalRepository
    {
        Task<List<Rental>> GetRentals();
        Task<Rental> GetRental(int id);
        Task<List<Rental>> GetRentalByCustomerId(int id);
        Task<Rental> PostRental(Rental rental);
        Task<Rental> PutRental(Rental rental);
        Task<object> DeleteRental(int rentalId);
    }
}
