using BookRecommender.Data;
using BookRecommender.Models;
using Microsoft.EntityFrameworkCore;

public class ReviewRepository : IReviewRepository
{
    private readonly ApplicationDbContext _context;

    public ReviewRepository(ApplicationDbContext context)
    {
        _context = context; // same DbContext used everywhere else — one connection, shared via DI
    }

    public async Task<IEnumerable<Review>> GetByBookIdAsync(int bookId)
    {
        // WHERE BookId = bookId — pulls only reviews belonging to this one book
        // OrderByDescending(CreatedAt) — newest reviews show first on the page
        return await _context
            .Reviews.Where(r => r.BookId == bookId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<Review?> GetByUserAndBookAsync(string userId, int bookId)
    {
        // Finds a review matching BOTH this user AND this book — used to block a second review
        return await _context.Reviews.FirstOrDefaultAsync(r =>
            r.UserId == userId && r.BookId == bookId
        );
    }

    public async Task AddAsync(Review review)
    {
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync(); // writes the INSERT to PostgreSQL
    }
}
