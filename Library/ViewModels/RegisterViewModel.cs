using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Library.Models;

namespace Library.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public Customer Customer { get; set; }

        [Required]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The passwords do not match")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
