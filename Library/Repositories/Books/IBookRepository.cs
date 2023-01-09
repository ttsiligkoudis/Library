using Library.Models;

namespace Library.Repositories.Books;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetBooksAsync();
    Task<Book> GetBookAsync(int bookId);
    void PutBook(Book book);
    Book PostBook(Book book);
    Task<Type> DeleteBook(int bookId);
}