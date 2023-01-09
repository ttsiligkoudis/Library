using Library.Models;

namespace Library.Repositories.Authors;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAuthorsAsync();
    Task<Author> GetAuthorAsync(int authorId);
    void PutAuthor(Author author);
    Author PostAuthor(Author author);
    Task<Type> DeleteAuthor(int authorId);
}