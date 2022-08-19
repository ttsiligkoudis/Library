using Library.Context;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories.Books
{
    public class BookRepository : IBookRepository, IDisposable
    {
        private AppDbContext _context;

        public BookRepository(AppDbContext context)
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

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await _context.Books.Include(b => b.Author).ToListAsync();
        }

        public async Task<Book> GetBookAsync(int bookId)
        {
            return await _context.Books.Include(b => b.Author).SingleOrDefaultAsync(b => b.Id == bookId);
        }

        public void PutBook(Book book)
        {
            _context.Books.Attach(book);
            _context.Entry(book).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public Book PostBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return book;
        }

        public async Task<Type> DeleteBook(int bookId)
        {
            var book = await GetBookAsync(bookId);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
            return typeof(void);
        }
    }
}
