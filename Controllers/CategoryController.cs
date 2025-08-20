using Challenges.Data;
using Challenges.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Challenges.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ChallengesDbContext _context;
        public CategoryController(ChallengesDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            // Retrieve all categories without tracking changes for read-only operation
            var categories = await _context.Categories.AsNoTracking().OrderBy(c => c.CategoryName).ToListAsync();
            return View(categories);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents Cross-Site Request Forgery attacks
        public async Task<IActionResult> Create([Bind("Id,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Categories.Add(category); // Add the new category to the context
                    await _context.SaveChangesAsync(); // Save changes to the database
                    TempData["SuccessMessage"] = "Category created successfully!"; // Provide user feedback
                    return RedirectToAction(nameof(Index)); // Redirect to the Index action
                }
                catch (DbUpdateException ex)
                {
                    // Catch database update exceptions and provide user-friendly error message
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator. Error: " + ex.InnerException?.Message);
                }
            }
            return View(category); // Return to the view with the current model if validation fails
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            // Find the category by ID
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound(); // Return 404 if category not found
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryName")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound(); // Ensure ID matches
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Use OriginalValues to handle concurrency if a RowVersion/Timestamp is present in the model
                    _context.Entry(category).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Category updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency conflicts (another user modified the data)
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        // Add a model error to inform the user about the conflict
                        ModelState.AddModelError("", "The record you attempted to edit "
                            + "was modified by another user after you got the original value. "
                            + "The edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again.");
                    }
                }
                catch (DbUpdateException ex)
                {
                    // Handle other database update errors
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator. Error: " + ex.InnerException?.Message);
                }
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            // Find the category to be deleted
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound(); // Return 404 if category not found (already deleted or never existed)
            }

            try
            {
                _context.Categories.Remove(category); // Remove the category from the context
                await _context.SaveChangesAsync(); // Save changes to the database
                TempData["SuccessMessage"] = $"{category.CategoryName} deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflicts during delete
                if (!CategoryExists(category.Id))
                {
                    TempData["ErrorMessage"] = "The category was already deleted by another user.";
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
                // Handle other database update errors
                ModelState.AddModelError("", "Unable to delete. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator. Error: " + ex.InnerException?.Message);
            }
            return View(category); // Return to the delete view with error
        }

        // Helper method to check if a category exists
        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
