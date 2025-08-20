using Challenges.Data;
using Challenges.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Challenges.Controllers
{
    public class ChallengeController : Controller
    {
        private readonly ChallengesDbContext _context;

        public ChallengeController(ChallengesDbContext context)
        {
            _context = context;
        }

        // GET: Challenges
        public async Task<IActionResult> Index()
        {
            // Eager load Category and order by Title for display
            var challenges = await _context.Challenges
                .AsNoTracking()
                .Include(c => c.Category)
                .OrderBy(c => c.Title)
                .ToListAsync();
            return View(challenges);
        }

        // GET: Challenges/Create
        public async Task<IActionResult> Create()
        {
            // Provide categories for dropdown list
            ViewData["Categories"] = new SelectList(await _context.Categories.OrderBy(c => c.CategoryName).ToListAsync(), "Id", "CategoryName");
            return View();
        }

        // POST: Challenges/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,Title,Description,StartDate,EndDate,ChallengeStatus")] Challenge challenge)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Challenges.Add(challenge);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Challenge created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // Catch database update exceptions
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator. Error: " + ex.InnerException?.Message);
                }
            }
            // Re-populate categories if ModelState is invalid
            ViewData["Categories"] = new SelectList(await _context.Categories.OrderBy(c => c.CategoryName).ToListAsync(), "Id", "CategoryName", challenge.CategoryId);
            return View(challenge);
        }

        // GET: Challenges/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            // Eager load Category for display and selection in the form
            var challenge = await _context.Challenges
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (challenge == null)
            {
                return NotFound();
            }

            ViewData["Categories"] = new SelectList(await _context.Categories.OrderBy(c => c.CategoryName).ToListAsync(), "Id", "CategoryName", challenge.CategoryId);
            return View(challenge);
        }

        // POST: Challenges/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,Title,Description,StartDate,EndDate,ChallengeStatus")] Challenge challenge)
        {
            if (id != challenge.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Attach the entity and set its state to Modified for update
                    _context.Entry(challenge).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Challenge updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency conflicts
                    if (!ChallengeExists(challenge.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", "The record you attempted to edit "
                            + "was modified by another user after you got the original value. "
                            + "The edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again.");
                    }
                }
                catch (DbUpdateException ex)
                {
                    // Handle other database update errors
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator. Error: " + ex.InnerException?.Message);
                }
            }
            ViewData["Categories"] = new SelectList(await _context.Categories.OrderBy(c => c.CategoryName).ToListAsync(), "Id", "CategoryName", challenge.CategoryId);
            return View(challenge);
        }

        // GET: Challenges/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            // Eager load Category for display in the delete confirmation view
            var challenge = await _context.Challenges
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (challenge == null)
            {
                return NotFound();
            }
            return View(challenge);
        }

        // POST: Challenges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge == null)
            {
                return NotFound(); // Already deleted or never existed
            }

            try
            {
                _context.Challenges.Remove(challenge);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Challenge deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflicts during delete
                if (!ChallengeExists(challenge.Id))
                {
                    TempData["ErrorMessage"] = "The challenge was already deleted by another user.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "The record you attempted to delete "
                        + "was modified by another user after you got the original value. "
                        + "The delete operation was canceled. If you still want to delete this record, "
                        + "click the Delete button again.");
                }
            }
            catch (DbUpdateException ex)
            {
                // Handle other database update errors
                ModelState.AddModelError("", "Unable to delete. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator. Error: " + ex.InnerException?.Message);
            }
            return View(challenge); // Return to the delete view with error
        }

        // GET: Challenges/Details/5
        public async Task<IActionResult> Details(int id)
        {
            // Eager load Category and ensure no tracking for read-only details
            var challenge = await _context.Challenges
                .AsNoTracking()
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (challenge == null)
            {
                return NotFound();
            }
            return View(challenge);
        }

        // Helper method to check if a challenge exists
        private bool ChallengeExists(int id)
        {
            return _context.Challenges.Any(e => e.Id == id);
        }
    }
}
