public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}