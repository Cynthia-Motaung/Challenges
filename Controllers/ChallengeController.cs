using Challenges.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Challenges.Models;

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
                .Include(c => c.Categories)
                .OrderBy(c => c.Title)
                .ToListAsync();
            return View(challenges);
        }


        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.OrderBy(c => c.CategoryName).ToListAsync();
            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Challenge challenge)
        {
            if (ModelState.IsValid)
            {
                _context.Challenges.Add(challenge);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Challenge created successfully!";
                return RedirectToAction("Index");
            }
            return View(challenge);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var challenge = await _context.Challenges
                .Include(c => c.Categories)
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
                .Include(c => c.Categories)
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
                .Include(c => c.Categories)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (challenge == null)
            {
                return NotFound();
            }
            return View(challenge);
        }
    }
}
