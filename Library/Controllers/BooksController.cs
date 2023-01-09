using Library.Helpers;
using Library.Models;
using Library.Repositories.Authors;
using Library.Repositories.Books;
using Library.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ISession = Library.Helpers.ISession;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        private readonly IUserAccess _userAccess;
        private readonly ISession _session;
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public BooksController(IUserAccess userAccess,ISession session,IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _userAccess = userAccess;
            _session = session;
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        public async Task<IActionResult> Index()
        {
            var viewModels = new List<BookViewModel>();
            var books = await _bookRepository.GetBooksAsync();
            foreach (var book in books)
            {
                viewModels.Add(new BookViewModel()
                {
                    Book = book,
                    CategoriesList = (from d in Enum.GetValues(typeof(Categories)).Cast<Categories>()
                        select new SelectListItem()
                        {
                            Value = Convert.ToInt16(d).ToString(),
                            Text = d.ToString(),
                            Selected = book.Categories.HasFlag(d)
                        }).Where(c => c.Selected).ToList()
            });
            }
            return View(viewModels);
        }

        public async Task<IActionResult> Create()
        {
            var customer = _session.GetCustomer();
            if (!_userAccess.IsAdmin(customer))
            {
                RedirectToAction("Index", "Home");
            }

            var viewModel = new BookViewModel
            {
                Book = new Book(),
                Authors = await _authorRepository.GetAuthorsAsync(),
            };
            viewModel.CategoriesList = (from d in Enum.GetValues(typeof(Categories)).Cast<Categories>()
                select new SelectListItem()
                {
                    Value = Convert.ToInt16(d).ToString(),
                    Text = d.ToString(),
                    Selected = viewModel.Book.Categories.HasFlag(d)
                }).ToList();
            return View("BookForm", viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var customer = _session.GetCustomer();
            if (!_userAccess.IsAdmin(customer))
            {
                RedirectToAction("Index", "Home");
            }
            var viewModel = new BookViewModel
            {
                Book = await _bookRepository.GetBookAsync(id),
                Authors = await _authorRepository.GetAuthorsAsync()
            };
            viewModel.CategoriesList = (from d in Enum.GetValues(typeof(Categories)).Cast<Categories>()
                select new SelectListItem()
                {
                    Value = Convert.ToInt16(d).ToString(),
                    Text = d.ToString(),
                    Selected = viewModel.Book.Categories.HasFlag(d)
                }).ToList();
            if (viewModel.Book == null) return RedirectToAction("Index", "Books");

            return View("BookForm", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Save (BookViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Authors = await _authorRepository.GetAuthorsAsync();
                viewModel.CategoriesList = (from d in Enum.GetValues(typeof(Categories)).Cast<Categories>()
                    select new SelectListItem()
                    {
                        Value = Convert.ToInt16(d).ToString(),
                        Text = d.ToString(),
                        Selected = Array.Exists(viewModel.SelectedCategories, element => element == Convert.ToInt16(d))
                    }).ToList();
                return View("BookForm", viewModel);
            }

            viewModel.Book.Categories = 0;
            foreach (var category in viewModel.SelectedCategories)
            {
                viewModel.Book.Categories += category;
            }
            if (viewModel.Book.Id == 0)
            {
                viewModel.Book = _bookRepository.PostBook(viewModel.Book);
            }
            else
            {
                _bookRepository.PutBook(viewModel.Book);
            }
            return RedirectToAction("Index", "Books");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookRepository.GetBookAsync(id);
            if (book != null)
            {
                //var rental = await _rentalRepository.GetRentals(book.Id);
                //if (rental != null && rental.Any())
                //{
                //    ModelState.AddModelError("", @"Cannot delete a Book that is already rented");
                //    return View("Index", await _bookRepository.GetBooksAsync());
                //}

                await _bookRepository.DeleteBook(book.Id);
            }
            return RedirectToAction("Index", "Books");
        }
    }
}