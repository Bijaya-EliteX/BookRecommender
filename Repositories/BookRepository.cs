// Repositories/BookRepository.cs
using Microsoft.EntityFrameworkCore;
using BookRecommender.Data;
using BookRecommender.Models;

public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _context;

    public BookRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
            .ToListAsync();
    }

    public async Task<Book> GetByIdAsync(int id)
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
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
}