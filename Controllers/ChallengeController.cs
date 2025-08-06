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


        public async Task<IActionResult> Index()
        {
            var challenges = await _context.Challenges
                .Include(c => c.Category)
                .OrderBy(c => c.Title)
                .ToListAsync();
            return View(challenges);
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Change ViewData["Category"] to ViewData["Categories"]
            ViewData["Categories"] = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Challenge challenge)
        {
            // We keep this check, but add a try-catch for the database operation
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Challenges.Add(challenge);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Challenge created successfully!";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    // This will catch errors during the database update.
                    // Set a breakpoint here to inspect the 'ex' variable.
                    // The InnerException often contains the most specific error message.
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator. Error: " + ex.InnerException?.Message);
                }
            }

            // Repopulate the dropdown if we end up here
            ViewData["Categories"] = _context.Categories.ToList();
            return View(challenge);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Categories"] = _context.Categories.ToList();
            var challenge = await _context.Challenges
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (challenge == null)
            {
                return NotFound();
            }
            return View(challenge);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Challenge challenge)
        {
            if (ModelState.IsValid)
            {
                _context.Challenges.Update(challenge);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Challenge updated successfully!";
                return RedirectToAction("Index");
            }
            return View(challenge);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var challenge = await _context.Challenges
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (challenge == null)
            {
                return NotFound();
            }
            return View(challenge);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge != null)
            {
                _context.Challenges.Remove(challenge);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Challenge deleted successfully!";
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var challenge = await _context.Challenges
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (challenge == null)
            {
                return NotFound();
            }
            return View(challenge);
        }
    }
}
