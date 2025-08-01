using Challenges.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Challenges.Models;

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
                .Include(p => p.User)
                .OrderBy(p => p.FullName).ToListAsync();
            return View(profiles);
        }

        public async Task<IActionResult> Create()
        {
            var users = await _context.Users.OrderBy(u => u.Username).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Profile profile)
        {
            if (ModelState.IsValid)
            {
                _context.Profiles.Add(profile);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(profile);
        }


        public IActionResult Edit(int id)
        {
            var users = _context.Users.OrderBy(u => u.Username).ToList();
            var profile = _context.Profiles.Find(id);
            if (profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }

        public async Task<IActionResult> Edit(Profile profile)
        {
            if (ModelState.IsValid)
            {
                _context.Profiles.Update(profile);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(profile);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var users = await _context.Users.OrderBy(u => u.Username).ToListAsync();
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
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var profile = await _context.Profiles
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
        
