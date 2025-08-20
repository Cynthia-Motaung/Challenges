using Challenges.Data;
using Challenges.Models;
using Challenges.Models.DTOs; // New using directive for DTOs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper; // Required for AutoMapper

namespace Challenges.Controllers
{
    public class UserController : Controller
    {
        private readonly ChallengesDbContext _context;
        private readonly IMapper _mapper; // Inject AutoMapper

        public UserController(ChallengesDbContext context, IMapper mapper) // Constructor injection for IMapper
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            // Retrieve all users, order them, and project directly into UserIndexDto to send only necessary data to the view.
            var users = await _context.Users
                .AsNoTracking()
                .OrderBy(u => u.Username)
                .Select(u => _mapper.Map<UserIndexDto>(u)) // Map User entity to UserIndexDto
                .ToListAsync();
            return View(users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View(); // The Create view will now expect a UserCreateDto
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Accept UserCreateDto instead of the full User model
        public async Task<IActionResult> Create(UserCreateDto userDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Map the DTO back to the User entity
                    var user = _mapper.Map<User>(userDto);
                    user.CreatedAt = DateTime.Now; // Set CreatedAt timestamp here
                    user.UpdatedAt = DateTime.Now; // Set UpdatedAt timestamp here

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "User created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator. Error: " + ex.InnerException?.Message);
                }
            }
            // If ModelState is invalid, return the DTO to the view to preserve input
            return View(userDto);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            // Map the User entity to UserEditDto for display in the edit form
            var userDto = _mapper.Map<UserEditDto>(user);
            return View(userDto);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Accept UserEditDto as input
        public async Task<IActionResult> Edit(int id, UserEditDto userDto)
        {
            if (id != userDto.Id) // Ensure the DTO ID matches the route ID
            {
                return NotFound();
            }

            // Retrieve the existing user entity from the database
            var userToUpdate = await _context.Users.FindAsync(id);

            if (userToUpdate == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Map the DTO properties onto the existing entity
                    _mapper.Map(userDto, userToUpdate);
                    userToUpdate.UpdatedAt = DateTime.Now; // Update UpdatedAt timestamp

                    _context.Entry(userToUpdate).State = EntityState.Modified; // Ensure EF tracks changes
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "User updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(userDto.Id)) // Use DTO ID for existence check
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
            // If ModelState is invalid, return the DTO to the view to preserve input
            return View(userDto);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            // For delete, you might still want to display the full User or a UserDetailsDto
            // For simplicity, keeping it as User for now, but could be refactored to UserDetailsDto
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "User deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
                {
                    TempData["ErrorMessage"] = "The user was already deleted by another user.";
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
            return View(user);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int id)
        {
            // Project directly into UserDetailsDto for read-only details view
            var userDto = await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == id)
                .Select(u => _mapper.Map<UserDetailsDto>(u)) // Map User entity to UserDetailsDto
                .FirstOrDefaultAsync();

            if (userDto == null)
            {
                return NotFound();
            }
            return View(userDto);
        }

        // Helper method to check if a user exists
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
