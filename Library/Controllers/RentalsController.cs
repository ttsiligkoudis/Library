using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Helpers;
using Library.Models;
using Library.Repositories.Books;
using Library.Repositories.Customers;
using Library.Repositories.Rentals;
using Library.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis;
using ISession = Library.Helpers.ISession;

namespace Library.Controllers
{
    public class RentalsController : Controller
    {
        private readonly IUserAccess _userAccess;
        private readonly ISession _session;
        private readonly IRentalRepository _rentalRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBookRepository _bookRepository;

        public RentalsController(IUserAccess userAccess, ISession session, IRentalRepository rentalRepository, ICustomerRepository customerRepository, IBookRepository bookRepository)
        {
            _userAccess = userAccess;
            _session = session;
            _rentalRepository = rentalRepository;
            _customerRepository = customerRepository;
            _bookRepository = bookRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Rental> rentals;
            var customer = _session.GetCustomer();
            if (!_userAccess.IsCustomer(customer))
            {
                return RedirectToAction("Index", "Home");
            }
            if (!_userAccess.IsAdmin(customer))
            {
                rentals = await _rentalRepository.GetRentalByCustomerId(customer.Id);
            }
            else
            {
                rentals = await _rentalRepository.GetRentals();
            }
            return View(rentals);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var rental = await _rentalRepository.GetRental(id);
            return View("RentalForm", rental);
        }

        public async Task<IActionResult> Create()
        {
            var books = await _bookRepository.GetBooksAsync();
            var viewModel = new RentalViewModel
            {
                Customers = await _customerRepository.GetCustomers(),
                Rental = new Rental(),
                Books = books.ToList()
            };
            var customer = _session.GetCustomer();
            if (_userAccess.IsCustomer(customer))
            {
                viewModel.Rental.Customer = customer;
                viewModel.Rental.CustomerId = customer.Id;
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Save(RentalViewModel viewModel)
        {
            viewModel.Books = (List<Book>)await _bookRepository.GetBooksAsync();
            if (viewModel.SelectedBooks.Length == 0)
            {
                viewModel.Customers = await _customerRepository.GetCustomers();
                ModelState.AddModelError("", @"You need to select at least one Book");
                return View("Create", viewModel);
            }

            if (viewModel.Rental.CustomerId == 0)
            {
                viewModel.Rental.Customer = await _customerRepository.Post(viewModel.Rental.Customer);
                viewModel.Rental.CustomerId = viewModel.Rental.Customer.Id;
            }
            
            if (viewModel.Rental.Id == 0)
            {
                viewModel.Books = viewModel.Books.Where(p => viewModel.SelectedBooks.Contains(p.Id)).ToList();
                foreach (var book in viewModel.Books)
                {
                    book.Quantity--;
                    _bookRepository.PutBook(book);
                    viewModel.Rental = await _rentalRepository.PostRental(new Rental()
                    {
                        CustomerId = viewModel.Rental.CustomerId,
                        BookId = book.Id,
                        RentDate = DateTime.Now
                    });
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var customer = _session.GetCustomer();
            if (!_userAccess.IsCustomer(customer))
            {
                RedirectToAction("Index", "Home");
            }

            var order = await _rentalRepository.GetRental(id);
            if (order == null) return RedirectToAction("Index");
            await _rentalRepository.DeleteRental(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ReturnRental(int id)
        {
            var rental = await _rentalRepository.GetRental(id);
            if (rental != null)
            {
                rental.Returned = true;
                rental.ReturnDate = DateTime.Now;
                await _rentalRepository.PutRental(rental);
                var book = await _bookRepository.GetBookAsync(rental.BookId);
                if (book != null)
                {
                    book.Quantity++;
                    _bookRepository.PutBook(book);
                }
            }
            return RedirectToAction("Index");
        }
    }
}