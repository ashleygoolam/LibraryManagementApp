using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagementApp.Data;
using LibraryManagementApp.Models;
using Microsoft.AspNetCore.Identity;
using LibraryManagementApp.Data.ViewModels;
using LibraryManagementApp.Data.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNet.Identity;

namespace LibraryManagementApp.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class LibraryCardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LibraryCardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LibraryCards
        public async Task<IActionResult> Index(int? i, string? searchString)
        {
            if (i < 1) i = 1;

            var allLibraryCards = await _context.LibraryCards.Include(l => l.ApplicationUser).ToListAsync();
            var paginatedCards = await PaginatedList<LibraryCard>.CreateAsync(allLibraryCards, i ?? 1, 8);
            
            int totalPages = PageLength.Length;
            if (i > totalPages) i = totalPages;

            return View(paginatedCards);
        }

        // GET: LibraryCards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LibraryCards == null)
            {
                return NotFound();
            }

            var libraryCard = await _context.LibraryCards
                .Include(l => l.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libraryCard == null)
            {
                return NotFound();
            }

            return View(libraryCard);
        }

        [AllowAnonymous]
        public async Task<IActionResult> PersonalCard()
        {
            string name = User.Identity!.Name!;
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == name);

            var libraryCard = await _context.LibraryCards
                .Include(l => l.ApplicationUser)
                .SingleOrDefaultAsync(m => m.ApplicationUserId == user!.Id);

            if (libraryCard == null)
            {
                return View("Not Found");
            }

            return View(libraryCard);
        }

        // GET: LibraryCards/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "UserName");
            return View();
        }

        // POST: LibraryCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Balance,isActivated,ApplicationUserId")] LibraryCard libraryCard)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == libraryCard.ApplicationUserId);

                libraryCard.CardNumber = Guid.NewGuid();
                libraryCard.Owner = user!.FullName;
                _context.Add(libraryCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "UserName", libraryCard.ApplicationUserId);
            return View(libraryCard);
        }

        // GET: LibraryCards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LibraryCards == null)
            {
                return NotFound();
            }

            var libraryCard = await _context.LibraryCards.FindAsync(id);
            if (libraryCard == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "UserName", libraryCard.ApplicationUserId);
            return View(libraryCard);
        }

        // POST: LibraryCards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CardNumber,Balance,isActivated,ApplicationUserId")] LibraryCard libraryCard)
        {
            if (id != libraryCard.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libraryCard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibraryCardExists(libraryCard.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "UserName", libraryCard.ApplicationUserId);
            return View(libraryCard);
        }

        // GET: LibraryCards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LibraryCards == null)
            {
                return NotFound();
            }

            var libraryCard = await _context.LibraryCards
                .Include(l => l.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libraryCard == null)
            {
                return NotFound();
            }

            return View(libraryCard);
        }

        // POST: LibraryCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LibraryCards == null)
            {
                return Problem("Entity set 'ApplicationDbContext.LibraryCards'  is null.");
            }
            var libraryCard = await _context.LibraryCards.FindAsync(id);
            if (libraryCard != null)
            {
                _context.LibraryCards.Remove(libraryCard);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibraryCardExists(int id)
        {
          return (_context.LibraryCards?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
