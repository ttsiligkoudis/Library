using Library.Context;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories.Rentals
{
    public class RentalRepository : IRentalRepository, IDisposable
    {
        private AppDbContext _context;
        public RentalRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
                _context = null!;
            }
        }

        public async Task<List<Rental>> GetRentals()
        {
            return await _context.Rentals.Include(r => r.Customer).Include( r => r.Book).ToListAsync();
        }

        public async Task<Rental> GetRental(int id)
        {
            return await _context.Rentals.Include(r => r.Customer).Include(r => r.Book).SingleOrDefaultAsync(r => r.Id == id);
        }
        public async Task<List<Rental>> GetRentalByCustomerId(int id)
        {
            return await _context.Rentals.Include(r => r.Customer).Include(r => r.Book).Where(r => r.CustomerId == id).ToListAsync();
        }

        public async Task<Rental> PostRental(Rental rental)
        {
            rental.RentDate = DateTime.UtcNow;
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();
            return rental;
        }

        public async Task<Rental> PutRental(Rental rental)
        {
            _context.Rentals.Attach(rental);
            _context.Entry(rental).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return rental;
        }
        public async Task<object> DeleteRental(int rentalId)
        {
            var order = await GetRental(rentalId);
            if (order != null)
            {
                _context.Rentals.Remove(order);
                await _context.SaveChangesAsync();
            }
            return null;
        }
    }
}
