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

        public async Task<IActionResult> Index()
        {
            var profiles = await _context.Profiles
                .AsNoTracking()
                .Include(p => p.User)
                .OrderBy(p => p.FirstName).ToListAsync();
            return View(profiles);
        }

        public IActionResult Create()
        {
            ViewBag.Users = new SelectList(_context.Users.ToList(), "Id", "Username");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Profile profile)
        {
            if (ModelState.IsValid)
            {
                _context.Profiles.Add(profile);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Profile created successfully!";
                return RedirectToAction("Index");
            }
            return View(profile);
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Users = new SelectList(_context.Users.ToList(), "Id", "Username");
            var profile = _context.Profiles.Find(id);
            if (profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Profile profile)
        {
            if (ModelState.IsValid)
            {
                _context.Profiles.Update(profile);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction("Index");
            }
            return View(profile);
        }

        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.Users = new SelectList(_context.Users.ToList(), "Id", "Username");
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }
            return RedirectToAction("profile");
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Profile profile)
        {

            if (profile != null)
            {
                _context.Profiles.Remove(profile);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Profile deleted successfully!";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
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



    }


}
        
