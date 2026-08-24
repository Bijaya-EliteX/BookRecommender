using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookRecommender.Models.ViewModels;

public class BookViewModel
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    public int AuthorId { get; set; }

    public List<int> SelectedGenreIds { get; set; } = new List<int>();

    //comes from the controller side
    public IEnumerable<SelectListItem> Authors { get; set; } = [];
    public IEnumerable<SelectListItem> Genres { get; set; } = [];
}
