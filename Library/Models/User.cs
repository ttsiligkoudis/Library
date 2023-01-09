using Library.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public UserType UserType { get; set; }

        public DateTime RegDate { get; set; }

        public DateTime? LoginDate { get; set; }
    }
}
