using Library.Repositories.Users;
using Library.Helpers;
using Library.Models;
using Library.Repositories.Customers;
using Library.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ISession = Library.Helpers.ISession;

namespace Library.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserAccess _userAccess;
        private readonly ISession _session;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;

        public UsersController(IUserAccess userAccess, ISession session, IUserRepository userRepository, ICustomerRepository customerRepository)
        {
            _userAccess = userAccess;
            _session = session;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
        }

        public async Task<IActionResult> Index()
        {
            var customer = _session.GetCustomer();
            if (!_userAccess.IsAdmin(customer))
            {
                return RedirectToAction("Index", "Home");
            }

            var users = await _userRepository.GetUsers();
            return View(users);
        }

        public IActionResult Register()
        {
            return View("Register", new RegisterViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> RegisterViewResult(RegisterViewModel viewModel)
        {
            viewModel.Customer.RegDate = DateTime.Now;
            if (!ModelState.IsValid) return View("Register", viewModel);
            var userInDb = await _userRepository.GetUser(viewModel.Customer.Email);
            if (userInDb != null)
            {
                ModelState.AddModelError("", @"Email already exists");
                return View("Register", viewModel);
            }

            viewModel.Customer.User = new User
            {
                Email = viewModel.Customer.Email,
                Password = viewModel.Password,
                RegDate = viewModel.Customer.RegDate,
                UserType = UserType.User
            };
            viewModel.Customer = await _customerRepository.Post(viewModel.Customer);
            return RedirectToAction("LoginViewResult", viewModel.Customer.User);
        }
        public IActionResult Login()
        {
            return View("Login");
        }
        public async Task<IActionResult> LoginViewResult(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", viewModel);
            }

            var user =
                await _userRepository.GetUser(viewModel.Email, viewModel.Password);
            if (user == null)
            {
                ModelState.AddModelError("", @"Invalid email or password");
                return View("Login", viewModel);
            }

            var customer = await _customerRepository.GetCustomerByUserId(user.Id);
            if (customer != null)
            {
                HttpContext.Session.SetString("Customer", JsonConvert.SerializeObject(customer));
            }
            user.LoginDate = DateTime.Now;
            await _userRepository.Put(user);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("Customer", string.Empty);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Details(int id)
        {
            var customer = await _customerRepository.GetCustomerByUserId(id);
            return customer == null
                ? RedirectToAction("Index", "Home")
                : View("Details", customer);
        }
    }
}