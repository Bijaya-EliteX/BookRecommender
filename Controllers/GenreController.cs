using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookRecommender.Models;
using BookRecommender.Models.ViewModels;

namespace BookRecommender.Controllers;

public class GenreController : Controller
{
    private readonly IGenreRepository _genreRepository;

    public GenreController(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var genres = await _genreRepository.GetAllAsync();
        return View(genres);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View(new GenreViewModel());

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GenreViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        await _genreRepository.AddAsync(new Genre { Name = vm.Name });
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var genre = await _genreRepository.GetByIdAsync(id);
        if (genre == null) return NotFound();

        return View(new GenreViewModel { Id = genre.Id, Name = genre.Name });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(GenreViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var genre = await _genreRepository.GetByIdAsync(vm.Id);
        if (genre == null) return NotFound();

        genre.Name = vm.Name;
        await _genreRepository.UpdateAsync(genre);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var genre = await _genreRepository.GetByIdAsync(id);
        if (genre == null) return NotFound();
        return View(genre);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _genreRepository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}