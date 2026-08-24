namespace BookRecommender.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;
    public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
