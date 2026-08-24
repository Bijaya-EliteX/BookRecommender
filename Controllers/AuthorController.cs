using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookRecommender.Models;
using BookRecommender.Models.ViewModels;

namespace BookRecommender.Controllers;

public class AuthorController : Controller
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorController(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var authors = await _authorRepository.GetAllAsync();
        return View(authors);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View(new AuthorViewModel());

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AuthorViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        await _authorRepository.AddAsync(new Author { Name = vm.Name });
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var author = await _authorRepository.GetByIdAsync(id);
        if (author == null) return NotFound();

        return View(new AuthorViewModel { Id = author.Id, Name = author.Name });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AuthorViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var author = await _authorRepository.GetByIdAsync(vm.Id);
        if (author == null) return NotFound();

        author.Name = vm.Name;
        await _authorRepository.UpdateAsync(author);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var author = await _authorRepository.GetByIdAsync(id);
        if (author == null) return NotFound();
        return View(author);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _authorRepository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}