using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int? Age { get; set; }

        public string Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public DateTime RegDate { get; set; }

        public User? User { get; set; }
        public int? UserId { get; set; }
    }
}
