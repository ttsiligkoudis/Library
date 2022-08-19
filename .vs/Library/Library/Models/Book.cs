using Library.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        public int? Quantity { get; set; }

        [Required]
        public Categories Categories { get; set; }

        public Author? Author { get; set; }

        public int? AuthorId { get; set; }

        [Required]
        public int? ReleaseYear { get; set; }
    }
}
