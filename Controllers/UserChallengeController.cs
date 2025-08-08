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

        public async Task<IActionResult> Index()
        {
            var userChallenges = await _context.UserChallenges
                .AsNoTracking()
                .Include(uc => uc.User)
                .Include(uc => uc.Challenge)
                .OrderBy(uc => uc.User.Username)
                .ThenBy(uc => uc.Challenge.Title)
                .ToListAsync();
            return View(userChallenges);
        }

        public async Task<IActionResult> Assign()
        {
            ViewData["Users"] = new SelectList(await _context.Users.OrderBy(u => u.Username).ToListAsync(), "Id", "Username");
            ViewData["Challenges"] = new SelectList(await _context.Challenges.OrderBy(c => c.Title).ToListAsync(), "Id", "Title");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Assign(UserChallenge userChallenge)
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
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "This user is already assigned to this challenge.");
                }
            }

            ViewData["Users"] = new SelectList(await _context.Users.OrderBy(u => u.Username).ToListAsync(), "Id", "Username", userChallenge.UserId);
            ViewData["Challenges"] = new SelectList(await _context.Challenges.OrderBy(c => c.Title).ToListAsync(), "Id", "Title", userChallenge.ChallengeId);
            return View(userChallenge);
        }

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