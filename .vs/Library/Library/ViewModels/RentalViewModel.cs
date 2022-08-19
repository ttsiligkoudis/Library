using Library.Models;

namespace Library.ViewModels
{
    public class RentalViewModel
    {
        public List<Customer> Customers { get; set; }

        public Rental Rental { get; set; }

        public List<Book> Books { get; set; }

        public int[] SelectedBooks { get; set; }

    }
}
