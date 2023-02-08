using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
