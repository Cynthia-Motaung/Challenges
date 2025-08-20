using Challenges.Data;
using Challenges.Models;
using Challenges.Models.DTOs;
using Challenges.Interfaces; // New using directive for IUnitOfWork
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Challenges.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork; // Inject IUnitOfWork
        private readonly IMapper _mapper;

        // Update constructor to inject IUnitOfWork
        public UserController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            // Use _unitOfWork.Users to access the User repository
            var users = await _unitOfWork.Users
                .GetAllAsync(); // GetAllAsync already includes AsNoTracking()

            var userDtos = _mapper.Map<IEnumerable<UserIndexDto>>(users.OrderBy(u => u.Username));
            return View(userDtos);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateDto userDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _mapper.Map<User>(userDto);
                    user.CreatedAt = DateTime.Now;
                    user.UpdatedAt = DateTime.Now;

                    await _unitOfWork.Users.AddAsync(user); // Use repository to add
                    await _unitOfWork.CompleteAsync(); // Save changes via Unit of Work
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
            return View(userDto);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            // Use repository to get user by ID
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userDto = _mapper.Map<UserEditDto>(user);
            return View(userDto);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserEditDto userDto)
        {
            if (id != userDto.Id)
            {
                return NotFound();
            }

            var userToUpdate = await _unitOfWork.Users.GetByIdAsync(id); // Get existing user

            if (userToUpdate == null)
            {
                // This scenario could indicate a concurrency conflict if the item was deleted
                // by another user between the GET and POST, or if the ID was invalid.
                // For optimistic concurrency, EF Core will handle DbUpdateConcurrencyException.
                // If it's truly not found, return NotFound.
                return NotFound();
            }

            // Ensure the entity is being tracked by the context for updates
            // If GetByIdAsync tracked it, then mapping onto it will mark it as modified.
            // If it wasn't tracked, we need to attach it and mark as modified or use Update.
            // The current GenericRepository.Update(T entity) handles setting state to modified.

            if (ModelState.IsValid)
            {
                try
                {
                    _mapper.Map(userDto, userToUpdate);
                    userToUpdate.UpdatedAt = DateTime.Now;

                    _unitOfWork.Users.Update(userToUpdate); // Use repository to update
                    await _unitOfWork.CompleteAsync(); // Save changes
                    TempData["SuccessMessage"] = "User updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Use repository's ExistsAsync for existence check
                    if (!await _unitOfWork.Users.ExistsAsync(u => u.Id == userDto.Id))
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
            return View(userDto);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id); // Use repository to find
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
            var user = await _unitOfWork.Users.GetByIdAsync(id); // Use repository to find
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                _unitOfWork.Users.Remove(user); // Use repository to remove
                await _unitOfWork.CompleteAsync(); // Save changes
                TempData["SuccessMessage"] = "User deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                // Use repository's ExistsAsync for existence check
                if (!await _unitOfWork.Users.ExistsAsync(u => u.Id == user.Id))
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
            // Use FindAsync with a predicate and map to DTO
            var user = await _unitOfWork.Users
                .FindAsync(u => u.Id == id);

            // FindAsync returns IEnumerable, so get the first or default
            var userEntity = user.FirstOrDefault();

            if (userEntity == null)
            {
                return NotFound();
            }

            var userDto = _mapper.Map<UserDetailsDto>(userEntity);
            return View(userDto);
        }

        // Helper method to check if a user exists (now uses repository)
        private async Task<bool> UserExists(int id)
        {
            return await _unitOfWork.Users.ExistsAsync(e => e.Id == id);
        }
    }
}
