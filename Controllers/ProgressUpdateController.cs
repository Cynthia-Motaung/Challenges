using Challenges.Data;
using Challenges.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Challenges.Controllers
{
    public class ProgressUpdateController : Controller
    {
        private readonly ChallengesDbContext _context;

        public ProgressUpdateController(ChallengesDbContext context)
        {
            _context = context;
        }

        // GET: ProgressUpdate
        public async Task<IActionResult> Index()
        {
            // Eager load Challenge and User, order by CreatedAt descending for display
            var progressUpdates = await _context.Progresses
                .AsNoTracking()
                .Include(p => p.Challenge)
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return View(progressUpdates);
        }

        // GET: ProgressUpdate/Create
        public async Task<IActionResult> Create()
        {
            // Provide Challenges and Users for dropdown lists
            ViewData["Challenges"] = new SelectList(await _context.Challenges.OrderBy(c => c.Title).ToListAsync(), "Id", "Title");
            ViewData["Users"] = new SelectList(await _context.Users.OrderBy(u => u.Username).ToListAsync(), "Id", "Username");
            return View();
        }

        // POST: ProgressUpdate/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ChallengeId,ProgressDetails,ProgressPercentage")] Progress progress)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Progresses.Add(progress);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Progress update created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator. Error: " + ex.InnerException?.Message);
                }
            }
            // Re-populate dropdowns if ModelState is invalid
            ViewData["Challenges"] = new SelectList(await _context.Challenges.OrderBy(c => c.Title).ToListAsync(), "Id", "Title", progress.ChallengeId);
            ViewData["Users"] = new SelectList(await _context.Users.OrderBy(u => u.Username).ToListAsync(), "Id", "Username", progress.UserId);
            return View(progress);
        }

        // GET: ProgressUpdate/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            // Eager load Challenge and User for display and selection in the form
            var progress = await _context.Progresses
                .Include(p => p.Challenge)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (progress == null)
            {
                return NotFound();
            }

            ViewData["Challenges"] = new SelectList(await _context.Challenges.OrderBy(c => c.Title).ToListAsync(), "Id", "Title", progress.ChallengeId);
            ViewData["Users"] = new SelectList(await _context.Users.OrderBy(u => u.Username).ToListAsync(), "Id", "Username", progress.UserId);
            return View(progress);
        }

        // POST: ProgressUpdate/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ChallengeId,ProgressDetails,ProgressPercentage,CreatedAt")] Progress progress)
        {
            if (id != progress.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update UpdatedAt timestamp before saving
                    progress.UpdatedAt = DateTime.Now;
                    _context.Entry(progress).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Progress update updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgressExists(progress.Id))
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
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator. Error: " + ex.InnerException?.Message);
                }
            }
            ViewData["Challenges"] = new SelectList(await _context.Challenges.OrderBy(c => c.Title).ToListAsync(), "Id", "Title", progress.ChallengeId);
            ViewData["Users"] = new SelectList(await _context.Users.OrderBy(u => u.Username).ToListAsync(), "Id", "Username", progress.UserId);
            return View(progress);
        }

        // GET: ProgressUpdate/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            // Eager load Challenge and User for display in the delete confirmation view
            var progress = await _context.Progresses
                .Include(p => p.Challenge)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (progress == null)
            {
                return NotFound();
            }
            return View(progress);
        }

        // POST: ProgressUpdate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var progress = await _context.Progresses.FindAsync(id);
            if (progress == null)
            {
                return NotFound();
            }

            try
            {
                _context.Progresses.Remove(progress);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Progress update deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProgressExists(progress.Id))
                {
                    TempData["ErrorMessage"] = "The progress update was already deleted by another user.";
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
                ModelState.AddModelError("", "Unable to delete. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator. Error: " + ex.InnerException?.Message);
            }
            return View(progress);
        }

        // GET: ProgressUpdate/Details/5
        public async Task<IActionResult> Details(int id)
        {
            // Eager load Challenge and User, and ensure no tracking for read-only details
            var progress = await _context.Progresses
                .AsNoTracking()
                .Include(p => p.Challenge)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (progress == null)
            {
                return NotFound();
            }
            return View(progress);
        }

        // Helper method to check if a progress update exists
        private bool ProgressExists(int id)
        {
            return _context.Progresses.Any(e => e.Id == id);
        }
    }
}
