using Library.Context;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories.Authors
{
    public class AuthorRepository : IAuthorRepository, IDisposable
    {
        private AppDbContext _context;

        public AuthorRepository(AppDbContext context)
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

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<Author> GetAuthorAsync(int authorId)
        {
            return await _context.Authors.SingleOrDefaultAsync(b => b.Id == authorId);
        }

        public void PutAuthor(Author author)
        {
            _context.Authors.Attach(author);
            _context.Entry(author).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public Author PostAuthor(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();
            return author;
        }

        public async Task<Type> DeleteAuthor(int authorId)
        {
            var author = await GetAuthorAsync(authorId);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
            }
            return typeof(void);
        }
    }
}
