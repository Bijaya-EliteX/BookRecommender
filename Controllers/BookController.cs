using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookRecommender.Models;
using BookRecommender.ViewModels;

public class BookController : Controller
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IGenreRepository _genreRepository;

    public BookController(
        IBookRepository bookRepository,
        IAuthorRepository authorRepository,
        IGenreRepository genreRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _genreRepository = genreRepository;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var books = await _bookRepository.GetAllAsync();
        return View(books);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null) return NotFound();
        return View(book);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create()
    {
        var vm = new BookViewModel
        {
            Authors = await GetAuthorSelectList(),
            Genres = await GetGenreSelectList()
        };
        return View(vm);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Authors = await GetAuthorSelectList();
            vm.Genres = await GetGenreSelectList();
            return View(vm);
        }

        var book = new Book
        {
            Title = vm.Title,
            Description = vm.Description,
            AuthorId = vm.AuthorId,
            BookGenres = vm.SelectedGenreIds
                .Select(gid => new BookGenre { GenreId = gid })
                .ToList()
        };

        await _bookRepository.AddAsync(book);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null) return NotFound();

        var vm = new BookViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            AuthorId = book.AuthorId,
            SelectedGenreIds = book.BookGenres.Select(bg => bg.GenreId).ToList(),
            Authors = await GetAuthorSelectList(),
            Genres = await GetGenreSelectList()
        };
        return View(vm);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(BookViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Authors = await GetAuthorSelectList();
            vm.Genres = await GetGenreSelectList();
            return View(vm);
        }

        var book = await _bookRepository.GetByIdAsync(vm.Id);
        if (book == null) return NotFound();

        book.Title = vm.Title;
        book.Description = vm.Description;
        book.AuthorId = vm.AuthorId;

        book.BookGenres.Clear();
        foreach (var gid in vm.SelectedGenreIds)
            book.BookGenres.Add(new BookGenre { BookId = book.Id, GenreId = gid });

        await _bookRepository.UpdateAsync(book);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null) return NotFound();
        return View(book);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _bookRepository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task<List<SelectListItem>> GetAuthorSelectList()
    {
        var authors = await _authorRepository.GetAllAsync();
        return authors.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToList();
    }

    private async Task<List<SelectListItem>> GetGenreSelectList()
    {
        var genres = await _genreRepository.GetAllAsync();
        return genres.Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name }).ToList();
    }
}