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

        public async Task<IActionResult> Index()
        {
            var progressUpdates = await _context.Progresses
                .Include(p => p.Challenge)
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return View(progressUpdates);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["Challenges"] = new SelectList(await _context.Challenges.OrderBy(c => c.Title).ToListAsync(), "Id", "Title");
            ViewData["Users"] = new SelectList(await _context.Users.OrderBy(u => u.Username).ToListAsync(), "Id", "Username");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Progress progress)
        {
            if (ModelState.IsValid)
            {
                _context.Progresses.Add(progress);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Progress update created successfully!";
                return RedirectToAction("Index");
            }
            
            ViewData["Challenges"] = new SelectList(await _context.Challenges.OrderBy(c => c.Title).ToListAsync(), "Id", "Title", progress.ChallengeId);
            ViewData["Users"] = new SelectList(await _context.Users.OrderBy(u => u.Username).ToListAsync(), "Id", "Username", progress.UserId);
            return View(progress);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var progress = await _context.Progresses
                .FirstOrDefaultAsync(p => p.Id == id);

            if (progress == null)
            {
                return NotFound();
            }

            ViewData["Challenges"] = new SelectList(await _context.Challenges.OrderBy(c => c.Title).ToListAsync(), "Id", "Title", progress.ChallengeId);
            ViewData["Users"] = new SelectList(await _context.Users.OrderBy(u => u.Username).ToListAsync(), "Id", "Username", progress.UserId);
            return View(progress);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Progress progress)
        {
            if (ModelState.IsValid)
            {
                _context.Progresses.Update(progress);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Progress update updated successfully!";
                return RedirectToAction("Index");
            }
            
            ViewData["Challenges"] = new SelectList(await _context.Challenges.OrderBy(c => c.Title).ToListAsync(), "Id", "Title", progress.ChallengeId);
            ViewData["Users"] = new SelectList(await _context.Users.OrderBy(u => u.Username).ToListAsync(), "Id", "Username", progress.UserId);
            return View(progress);
        }

        public async Task<IActionResult> Delete(int id)
        {
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

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var progress = await _context.Progresses.FindAsync(id);
            if (progress != null)
            {
                _context.Progresses.Remove(progress);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Progress update deleted successfully!";
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
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
    }
}