using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Rental
    {
        [Key]
        public int Id { get; set; }

        public Customer Customer { get; set; }

        public int CustomerId { get; set; }

        public Book Book { get; set; }

        public int BookId { get; set; }

        public DateTime RentDate { get; set; }

        public bool Returned { get; set; }

        public DateTime? ReturnDate { get; set; }
    }
}
