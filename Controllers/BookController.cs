using BookRecommender.Models;
using BookRecommender.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookRecommender.Controllers;

public class BookController : Controller
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public BookController(
        IBookRepository bookRepository,
        IAuthorRepository authorRepository,
        UserManager<ApplicationUser> userManager,
        IGenreRepository genreRepository,
        IReviewRepository reviewRepository
    )
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _genreRepository = genreRepository;
        _reviewRepository = reviewRepository;
        _userManager = userManager;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index(
        int page = 1,
        int? genreId = null,
        int? authorId = null,
        string sortBy = "id"
    )
    // ?page=2&genreId=3&sortBy=rating
    {
        const int pageSize = 10; // fixed page size — could be made configurable later

        var (books, totalCount) = await _bookRepository.GetPagedAsync(
            page,
            pageSize,
            genreId,
            authorId,
            sortBy
        );
        // calls the repository method from Step 2 — this is where the actual filtered/sorted/paged query runs

        var vm = new BookIndexViewModel
        {
            Books = books,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize), // e.g. 45 books / 10 per page = 5 pages
            SelectedGenreId = genreId,
            SelectedAuthorId = authorId,
            SortBy = sortBy,
            Authors = await GetAuthorSelectList(), // existing private method, reused here for the filter dropdown
            Genres = await GetGenreSelectList(), // same — reused from Create/Edit dropdown logic
        };

        return View(vm); // sends BookIndexViewModel to Views/Book/Index.cshtml
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null)
            return NotFound();
        return View(book);
    }

    [Authorize] // only logged-in users (any role) can submit a review — no [Roles=] restriction needed
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddReview(ReviewViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            // If validation fails (e.g. rating out of range), send them back to Details
            // with the error — TempData carries a message across the redirect
            TempData["ReviewError"] = "Please provide a valid rating (1-5).";
            return RedirectToAction(nameof(Details), new { id = vm.BookId });
        }

        var currentUser = await _userManager.GetUserAsync(User); // gets the logged-in ApplicationUser
        if (currentUser == null)
            return Challenge(); // safety fallback — shouldn't happen due to [Authorize]

        // Prevent duplicate reviews — one review per user per book
        var existing = await _reviewRepository.GetByUserAndBookAsync(currentUser.Id, vm.BookId);
        if (existing != null)
        {
            TempData["ReviewError"] = "You have already reviewed this book.";
            return RedirectToAction(nameof(Details), new { id = vm.BookId });
        }

        var review = new Review
        {
            BookId = vm.BookId,
            UserId = currentUser.Id,
            Rating = vm.Rating,
            Comment = vm.Comment,
            CreatedAt = DateTime.UtcNow,
        };

        await _reviewRepository.AddAsync(review);
        return RedirectToAction(nameof(Details), new { id = vm.BookId }); // reload page, review now shows
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create()
    {
        var vm = new BookViewModel
        {
            Authors = await GetAuthorSelectList(),
            Genres = await GetGenreSelectList(),
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
            BookGenres = vm
                .SelectedGenreIds.Select(gid => new BookGenre { GenreId = gid })
                .ToList(),
        };

        await _bookRepository.AddAsync(book);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null)
            return NotFound();

        var vm = new BookViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            AuthorId = book.AuthorId,
            SelectedGenreIds = book.BookGenres.Select(bg => bg.GenreId).ToList(),
            Authors = await GetAuthorSelectList(),
            Genres = await GetGenreSelectList(),
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
        if (book == null)
            return NotFound();

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
        if (book == null)
            return NotFound();
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
        return authors
            .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name })
            .ToList();
    }

    private async Task<List<SelectListItem>> GetGenreSelectList()
    {
        var genres = await _genreRepository.GetAllAsync();
        return genres
            .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name })
            .ToList();
    }
}
