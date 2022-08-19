using Library.Helpers;
using Library.Models;
using Library.Repositories.Customers;
using Microsoft.AspNetCore.Mvc;
using ISession = Library.Helpers.ISession;

namespace Library.Controllers
{
    public class CustomersController : Controller
    {
        private readonly IUserAccess _userAccess;
        private readonly ISession _session;
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(IUserAccess userAccess, ISession session, ICustomerRepository customerRepository)
        {
            _userAccess = userAccess;
            _session = session;
            _customerRepository = customerRepository;
        }

        public async Task<IActionResult> Index()
        {
            var customer = _session.GetCustomer();
            if (!_userAccess.IsAdmin(customer))
            {
                throw new AccessViolationException();
            }

            var customers = await _customerRepository.GetCustomers();
            return View(customers);
        }

        public IActionResult Create()
        {
            return View("CustomerForm", new Customer());
        }

        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerRepository.GetCustomer(id);
            if (customer == null)
            {
                throw new NullReferenceException();
            }
            return View("CustomerForm", customer);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View("CustomerForm", customer);
            }
            if (customer.Id == 0)
            {
                customer.RegDate = DateTime.Now;
                await _customerRepository.Post(customer);
            }
            else
            {
                await _customerRepository.Put(customer);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerRepository.GetCustomer(id);
            if (customer == null)
            {
                throw new NullReferenceException();
            }
            var loggedCustomer = _session.GetCustomer();
            if (loggedCustomer?.Id == customer.Id)
            {
                await _customerRepository.Delete(customer.Id);
                return RedirectToAction("Logout", "Users");
            }

            await _customerRepository.Delete(customer.Id);
            return RedirectToAction("Index");
        }
    }
}