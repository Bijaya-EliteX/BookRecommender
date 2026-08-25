using BookRecommender.Models;

// Repositories/IBookRepository.cs
public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(int id);

    //we return a page of books + total count (needed to calculate total pages in the view)
    Task<(IEnumerable<Book> Books, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        int ? genreId,
        int? authorId,
        string sortBy
    );
}