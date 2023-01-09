using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int? Age { get; set; }

        [Required]
        public string BirthPlace { get; set; }

        [Required]
        public int? BooksReleased { get; set; }
    }
}
