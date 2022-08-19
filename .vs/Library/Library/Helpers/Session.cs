using Library.Models;
using Microsoft.AspNetCore.Http;
using static Newtonsoft.Json.JsonConvert;

namespace Library.Helpers
{
    public class Session : ISession
    {
        public User User { get; set; }
        public Customer Customer { get; set; }
        public IHttpContextAccessor HttpContextAccessor;

        public Session(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public Customer GetCustomer()
        {
            var customerStr = HttpContextAccessor.HttpContext?.Session.GetString("Customer");

            if (!string.IsNullOrEmpty(customerStr))
            {
                Customer = DeserializeObject<Customer>(customerStr)!;
            }

            return Customer;
        }
    }
}
