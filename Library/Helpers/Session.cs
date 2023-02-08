using Library.Models;
using Microsoft.AspNetCore.Http;
using static Newtonsoft.Json.JsonConvert;

namespace Library.Helpers
{
    public class Session : ISession
    {
        public IHttpContextAccessor HttpContextAccessor;

        public Session(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public Customer GetCustomer()
        {
            var customerStr = HttpContextAccessor.HttpContext?.Session.GetString("Customer");

            return !string.IsNullOrEmpty(customerStr) ? DeserializeObject<Customer>(customerStr) : null;
        }
    }
}
