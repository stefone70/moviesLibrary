using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using moviesLibrary.Data;
using moviesLibrary.Models;

namespace moviesLibrary.Controllers
{
    public class FilmsController : Controller
    {
        private readonly moviesLibraryContext _context;

        public FilmsController(moviesLibraryContext context)
        {
            _context = context;
        }

        // GET: Films
        public async Task<IActionResult> Index()
        {
              return _context.Film != null ? 
                          View(await _context.Film.ToListAsync()) :
                          Problem("Entity set 'moviesLibraryContext.Film'  is null.");
        }

        // GET: Films/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Film == null)
            {
                return NotFound();
            }

            var film = await _context.Film
                .FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // GET: Films/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Films/Create
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titre,Genre,Realisateur,AnneeSortie,Acteur")] Film film)
        {
            if (ModelState.IsValid)
            {
                _context.Add(film);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }

        // GET: Films/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Film == null)
            {
                return NotFound();
            }

            var film = await _context.Film.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }

        // POST: Films/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titre,Genre,Realisateur,AnneeSortie,Acteur")] Film film)
        {
            if (id != film.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(film);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(film.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }

        // GET: Films/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Film == null)
            {
                return NotFound();
            }

            var film = await _context.Film
                .FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Film == null)
            {
                return Problem("Entity set 'moviesLibraryContext.Film'  is null.");
            }
            var film = await _context.Film.FindAsync(id);
            if (film != null)
            {
                _context.Film.Remove(film);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
          return (_context.Film?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        // recherche par Genre
        public async Task<IActionResult> SearchByGenre(string genre)
        {
            
            if (string.IsNullOrEmpty(genre))
            {
                ViewBag.ErrorMessage = "Veuillez entrer un genre.";
                return View("Index", new List<Film>()); 
            }

            if (_context.Film == null)
            {
                return NotFound();
            }

            var films = await _context.Film
                .Where(m => m.Genre == genre)
                .ToListAsync();

            return View("Index", films);
        }
        // Recherche par acteur
        public async Task<IActionResult> SearchByActor(string actor)
        {
           
            if (string.IsNullOrEmpty(actor))
            {
                ViewBag.ErrorMessage = "Veuillez entrer un acteur.";
                return View("Index", new List<Film>());
            }

            if (_context.Film == null)
            {
                return NotFound();
            }

            var films = await _context.Film
                .Where(m => m.Acteur == actor)
                .ToListAsync();

            return View("Index", films);
        }
        public async Task<IActionResult> SearchByRealisateur(string realisateur)
        {

            if (string.IsNullOrEmpty(realisateur))
            {
                ViewBag.ErrorMessage = "Veuillez entrer un realisateur.";
                return View("Index", new List<Film>());
            }

            if (_context.Film == null)
            {
                return NotFound();
            }

            var films = await _context.Film
                .Where(m => m.Realisateur == realisateur)
                .ToListAsync();

            return View("Index", films);
        }
        public async Task<IActionResult> SearchByAnneeSortie(int? anneeSortie)
        {

            if (!anneeSortie.HasValue)
            {
                ViewBag.ErrorMessage = "Veuillez entrer une année.";
                return View("Index", new List<Film>());
            }

            if (_context.Film == null)
            {
                return NotFound();
            }

            var films = await _context.Film
               .Where(m => m.AnneeSortie == anneeSortie.Value)
               .ToListAsync();

            return View("Index", films);
        }
        // Recherche avancée
        public async Task<IActionResult> AdvancedSearch(string titre, string genre, string realisateur, int? anneeSortie, string acteur)
        {
            if (_context.Film == null)
            {
                return NotFound();
            }

            var query = _context.Film.AsQueryable();

            if (!string.IsNullOrEmpty(titre))
            {
                query = query.Where(m => m.Titre.Contains(titre));
            }

            if (!string.IsNullOrEmpty(genre))
            {
                query = query.Where(m => m.Genre.Contains(genre));
            }

            if (!string.IsNullOrEmpty(realisateur))
            {
                query = query.Where(m => m.Realisateur.Contains(realisateur));
            }

            if (anneeSortie.HasValue)
            {
                query = query.Where(m => m.AnneeSortie == anneeSortie);
            }

            if (!string.IsNullOrEmpty(acteur))
            {
                query = query.Where(m => m.Acteur.Contains(acteur));
            }

            var films = await query.ToListAsync();

            return View("Index", films);
        }
        // GET: Films/SortByYear
        public async Task<IActionResult> SortByYear()
        {
            if (_context.Film == null)
            {
                return NotFound();
            }

            var films = await _context.Film
                .OrderBy(m => m.AnneeSortie)
                .ToListAsync();

            return View("Index", films);
        }

    }
}
