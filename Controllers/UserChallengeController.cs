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

        // GET: Lists all assignments
        public async Task<IActionResult> Index()
        {
            var userChallenges = await _context.UserChallenges
                .Include(uc => uc.User)
                .Include(uc => uc.Challenge)
                .OrderBy(uc => uc.User.Username)
                .ThenBy(uc => uc.Challenge.Title)
                .ToListAsync();
            return View(userChallenges);
        }

        // GET: Shows the form to assign a user to a challenge
        public async Task<IActionResult> Assign()
        {
            ViewData["Users"] = new SelectList(await _context.Users.OrderBy(u => u.Username).ToListAsync(), "Id", "Username");
            ViewData["Challenges"] = new SelectList(await _context.Challenges.OrderBy(c => c.Title).ToListAsync(), "Id", "Title");
            return View();
        }

        // POST: Processes the new assignment
        [HttpPost]
        public async Task<IActionResult> Assign(UserChallenge userChallenge)
        {
            // The model binder will create the UserChallenge with UserId and ChallengeId
            // The AssignedDate is already set by default in the model.

            if (ModelState.IsValid)
            {
                try
                {
                    _context.UserChallenges.Add(userChallenge);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Challenge assigned successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    // This likely means the user is already assigned to this challenge,
                    // which violates the primary key constraint.
                    ModelState.AddModelError("", "This user is already assigned to this challenge.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["Users"] = new SelectList(await _context.Users.OrderBy(u => u.Username).ToListAsync(), "Id", "Username", userChallenge.UserId);
            ViewData["Challenges"] = new SelectList(await _context.Challenges.OrderBy(c => c.Title).ToListAsync(), "Id", "Title", userChallenge.ChallengeId);
            return View(userChallenge);
        }

        // POST: Deletes (un-assigns) a user from a challenge
        [HttpPost]
        public async Task<IActionResult> Delete(int userId, int challengeId)
        {
            var userChallenge = await _context.UserChallenges.FindAsync(userId, challengeId);
            if (userChallenge != null)
            {
                _context.UserChallenges.Remove(userChallenge);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Challenge un-assigned successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}