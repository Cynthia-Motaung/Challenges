using Challenges.Data;
using Challenges.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Challenges.Controllers
{
    public class UserChallengeController : Controller
    {
        private readonly ChallengesDbContext _context;

        public UserChallengeController(ChallengesDbContext context)
        {
            _context = context;
        }

        // GET: UserChallenges
        public async Task<IActionResult> Index()
        {
            // Eager load User and Challenge, order by User and then Challenge title for display
            var userChallenges = await _context.UserChallenges
                .AsNoTracking()
                .Include(uc => uc.User)
                .Include(uc => uc.Challenge)
                .OrderBy(uc => uc.User.Username)
                .ThenBy(uc => uc.Challenge.Title)
                .ToListAsync();
            return View(userChallenges);
        }

        // GET: UserChallenges/Assign
        public async Task<IActionResult> Assign()
        {
            // Provide Users and Challenges for dropdown lists
            ViewData["Users"] = new SelectList(await _context.Users.OrderBy(u => u.Username).ToListAsync(), "Id", "Username");
            ViewData["Challenges"] = new SelectList(await _context.Challenges.OrderBy(c => c.Title).ToListAsync(), "Id", "Title");
            return View();
        }

        // POST: UserChallenges/Assign
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign([Bind("UserId,ChallengeId,AssignedDate")] UserChallenge userChallenge)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.UserChallenges.Add(userChallenge);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Challenge assigned successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // Specific handling for unique constraint violation (e.g., user already assigned to challenge)
                    if (ex.InnerException?.Message.Contains("Cannot insert duplicate key row in object") == true)
                    {
                        ModelState.AddModelError("", "This user is already assigned to this challenge.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to assign challenge. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator. Error: " + ex.InnerException?.Message);
                    }
                }
            }

            // Re-populate dropdowns if ModelState is invalid
            ViewData["Users"] = new SelectList(await _context.Users.OrderBy(u => u.Username).ToListAsync(), "Id", "Username", userChallenge.UserId);
            ViewData["Challenges"] = new SelectList(await _context.Challenges.OrderBy(c => c.Title).ToListAsync(), "Id", "Title", userChallenge.ChallengeId);
            return View(userChallenge);
        }

        // POST: UserChallenges/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int userId, int challengeId)
        {
            var userChallenge = await _context.UserChallenges.FindAsync(userId, challengeId);
            if (userChallenge == null)
            {
                return NotFound(); // Already un-assigned or never existed
            }

            try
            {
                _context.UserChallenges.Remove(userChallenge);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Challenge un-assigned successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                // Note: Concurrency for composite keys can be tricky with FindAsync. 
                // A more robust check might involve reloading and re-checking.
                // For simplicity here, we assume if it's not found, it was deleted concurrently.
                if (!UserChallengeExists(userId, challengeId))
                {
                    TempData["ErrorMessage"] = "The challenge assignment was already deleted by another user.";
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
                ModelState.AddModelError("", "Unable to un-assign. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator. Error: " + ex.InnerException?.Message);
            }
            // If an error occurs and we're still in the method, redirect to index
            return RedirectToAction(nameof(Index));
        }

        // Helper method to check if a UserChallenge exists (composite key)
        private bool UserChallengeExists(int userId, int challengeId)
        {
            return _context.UserChallenges.Any(e => e.UserId == userId && e.ChallengeId == challengeId);
        }
    }
}
