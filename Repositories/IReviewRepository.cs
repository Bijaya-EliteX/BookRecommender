using BookRecommender.Models;

public interface IReviewRepository
{
    // Fetches all reviews for one specific book — used on Book Details page
    Task<IEnumerable<Review>> GetByBookIdAsync(int bookId);

    // Checks if a specific user already reviewed a specific book — prevents duplicate reviews
    Task<Review?> GetByUserAndBookAsync(string userId, int bookId);

    // Saves a new review to the database
    Task AddAsync(Review review);
}
