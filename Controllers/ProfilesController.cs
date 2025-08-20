using Challenges.Data;
using Challenges.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Challenges.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly ChallengesDbContext _context;
        public ProfilesController(ChallengesDbContext context)
        {
            _context = context;
        }

        // GET: Profiles
        public async Task<IActionResult> Index()
        {
            // Eager load User and order by FirstName for display
            var profiles = await _context.Profiles
                .AsNoTracking()
                .Include(p => p.User)
                .OrderBy(p => p.FirstName).ToListAsync();
            return View(profiles);
        }

        // GET: Profiles/Create
        public async Task<IActionResult> Create()
        {
            // Provide users for dropdown list
            ViewBag.Users = new SelectList(await _context.Users.OrderBy(u => u.Username).ToListAsync(), "Id", "Username");
            return View();
        }

        // POST: Profiles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,FirstName,LastName,DateOfBirth,ProfilePicture")] Profile profile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Profiles.Add(profile);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Profile created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator. Error: " + ex.InnerException?.Message);
                }
            }
            // Re-populate users if ModelState is invalid
            ViewBag.Users = new SelectList(await _context.Users.OrderBy(u => u.Username).ToListAsync(), "Id", "Username", profile.UserId);
            return View(profile);
        }

        // GET: Profiles/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            // Eager load User for display and selection in the form
            var profile = await _context.Profiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (profile == null)
            {
                return NotFound();
            }
            ViewBag.Users = new SelectList(await _context.Users.OrderBy(u => u.Username).ToListAsync(), "Id", "Username", profile.UserId);
            return View(profile);
        }

        // POST: Profiles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,FirstName,LastName,DateOfBirth,ProfilePicture")] Profile profile)
        {
            if (id != profile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(profile).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Profile updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileExists(profile.Id))
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
            ViewBag.Users = new SelectList(await _context.Users.OrderBy(u => u.Username).ToListAsync(), "Id", "Username", profile.UserId);
            return View(profile);
        }

        // GET: Profiles/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            // Eager load User for display in the delete confirmation view
            var profile = await _context.Profiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }

        // POST: Profiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            try
            {
                _context.Profiles.Remove(profile);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Profile deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(profile.Id))
                {
                    TempData["ErrorMessage"] = "The profile was already deleted by another user.";
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
            return View(profile);
        }

        // GET: Profiles/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            // Eager load User and ensure no tracking for read-only details
            var profile = await _context.Profiles
                .AsNoTracking()
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }

        // Helper method to check if a profile exists
        private bool ProfileExists(int id)
        {
            return _context.Profiles.Any(e => e.Id == id);
        }
    }
}
