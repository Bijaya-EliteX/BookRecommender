// Repositories/BookRepository.cs
using BookRecommender.Data;
using BookRecommender.Models;
using Microsoft.EntityFrameworkCore;

public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _context;

    public BookRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context
            .Books.Include(b => b.Author)
            .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
            .ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context
            .Books.Include(b => b.Author)
            .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
            .Include(b => b.Reviews)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task AddAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }

    //for pagination/filtering/sorting
    public async Task<(IEnumerable<Book> Books, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        int? genreId,
        int? authorId,
        string sortBy
    )
    {
        var query = _context
            .Books.Include(b => b.Author)
            .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
            .Include(b => b.Reviews) //needed to calculate average rating for sorting
            .AsQueryable();

        if (authorId.HasValue) //only filter if the user actually picked an author
            query = query.Where(b => b.AuthorId == authorId.Value);

        if (genreId.HasValue) //only filter if the user picked a genre
            query = query.Where(b => b.BookGenres.Any(bg => bg.GenreId == genreId.Value));

        query = sortBy switch
        { // dynamic ORDER BY based on user's choice
            "title" => query.OrderBy(b => b.Title),
            "nepali" => query // Nepali alphabet (क - ज्ञ) sort: Devanagari titles first, then the rest
                .OrderBy(b => string.Compare(EF.Functions.Collate(b.Title, "C"), "\u0900") < 0) // \u0900 = Devanagari block start; "C" = code-point collation so the comparison ignores ICU locale rules
                .ThenBy(b => b.Title),
            "rating" => query.OrderByDescending(b =>
                b.Reviews.Any() ? b.Reviews.Average(r => r.Rating) : 0
            ),
            _ => query.OrderBy(b => b.Id), // default sort if nothing specified
        };
        var totalCount = await query.CountAsync();

        var books = await query
            .Skip((page - 1) * pageSize) //skip previous pages's rows
            .Take(pageSize) //take only this page's row
            .ToListAsync(); //executes the actual SQL query now

        return (books, totalCount);
    }
}
