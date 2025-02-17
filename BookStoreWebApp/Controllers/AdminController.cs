using System.Security.Cryptography;
using System.Text;
using BookStoreWebApp.Data;
using BookStoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreWebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Admin Dashboard
        public async Task<IActionResult> AdminDashboard()
        {
            // Check if user is an admin
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (isAdmin != "true")
            {
                return RedirectToAction("Login");
            }

            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // ✅ Delete User (Admin Only)
        public async Task<IActionResult> DeleteUser(int id)
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (isAdmin != "true")
            {
                TempData["Error"] = "Unauthorized Access!";
                return RedirectToAction("Login");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Prevent admin from deleting themselves
            var loggedInUserId = HttpContext.Session.GetString("UserId");
            if (loggedInUserId == user.ID.ToString())
            {
                TempData["Error"] = "You cannot delete yourself!";
                return RedirectToAction("AdminDashboard");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("AdminDashboard");
        }

        // Get user details for editing
        public async Task<IActionResult> EditUser(int id)
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (isAdmin != "true")
            {
                TempData["Error"] = "Unauthorized Access!";
                return RedirectToAction("Login");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserEditModel
            {
                ID = user.ID,
                UserName = user.username,
                Email = user.email,
                IsAdmin = user.isAdmin
            };

            return View(model);
        }

        // Post method to save updated user details
        [HttpPost]
        public async Task<IActionResult> EditUser(UserEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.Users.FindAsync(model.ID);
            if (user == null)
            {
                return NotFound();
            }

            // Update user details
            user.username = model.UserName;
            user.email = model.Email;
            user.isAdmin = model.IsAdmin;

            // Only update password if provided
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                user.password = HashPassword(model.Password);
            }

            try
            {
                // Mark fields as modified to ensure EF updates them
                _context.Entry(user).Property(x => x.username).IsModified = true;
                _context.Entry(user).Property(x => x.email).IsModified = true;
                _context.Entry(user).Property(x => x.isAdmin).IsModified = true;

                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    _context.Entry(user).Property(x => x.password).IsModified = true;
                }

                await _context.SaveChangesAsync();
               


                TempData["Success"] = "User credentials updated successfully!";
                return RedirectToAction("AdminDashboard", "Admin");
            }
            catch
            {
                TempData["Error"] = "An error occurred while updating the user.";
                return View(model);
            }
        }




        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        // ✅ Helper Method: Hash Password
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
