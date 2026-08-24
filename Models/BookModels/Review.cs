public class Review
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }
    public string UserId { get; set; }   // FK to AspNetUsers.Id (string)
    public int Rating { get; set; }      // 1-5
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}