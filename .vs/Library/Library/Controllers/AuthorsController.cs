using Library.Helpers;
using Library.Models;
using Library.Repositories.Authors;
using Library.Repositories.Customers;
using Microsoft.AspNetCore.Mvc;
using ISession = Library.Helpers.ISession;

namespace Library.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IUserAccess _userAccess;
        private readonly ISession _session;
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(IUserAccess userAccess, ISession session, IAuthorRepository authorRepository)
        {
            _userAccess = userAccess;
            _session = session;
            _authorRepository = authorRepository;
        }

        public async Task<IActionResult> Index()
        {
            var authors = await _authorRepository.GetAuthorsAsync();
            return View(authors);
        }

        public IActionResult Create()
        {
            return View("AuthorForm", new Author());
        }

        public async Task<IActionResult> Edit(int id)
        {
            var author = await _authorRepository.GetAuthorAsync(id);
            if (author == null)
            {
                throw new NullReferenceException();
            }
            return View("AuthorForm", author);
        }

        [HttpPost]
        public IActionResult Save(Author author)
        {
            if (!ModelState.IsValid)
            {
                return View("AuthorForm", author);
            }
            if (author.Id == 0)
            {
                _authorRepository.PostAuthor(author);
            }
            else
            {
                _authorRepository.PutAuthor(author);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _authorRepository.GetAuthorAsync(id);
            if (customer == null)
            {
                throw new NullReferenceException();
            }
            await _authorRepository.DeleteAuthor(customer.Id);
            return RedirectToAction("Index");
        }
    }
}