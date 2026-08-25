using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookRecommender.Models.ViewModels
{
    public class BookIndexViewModel
    {
        public IEnumerable<Book> Books { get; set; } = []; // the current page's books — filled by BookController.cs

        public int CurrentPage { get; set; } // used by Index.cshtml to highlight/calculate page links
        public int TotalPages { get; set; } // calculated as ceil(TotalCount / PageSize) in controller
        public int? SelectedGenreId { get; set; } // remembers what filter is active, to keep dropdown selected
        public int? SelectedAuthorId { get; set; }
        public string SortBy { get; set; } = string.Empty; // remembers current sort, to keep it selected in the view

        public IEnumerable<SelectListItem> Genres { get; set; } = [];
        public IEnumerable<SelectListItem> Authors { get; set; } = [];
    }
}
