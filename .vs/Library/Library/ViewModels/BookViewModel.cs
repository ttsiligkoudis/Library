using Library.Helpers;
using Library.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.ViewModels
{
    public class BookViewModel
    {
        public Book Book { get; set; }

        public IEnumerable<Author>? Authors { get; set; }

        public List<SelectListItem>? CategoriesList { get; set; }

        public int[] SelectedCategories { get; set; }
    }
}
