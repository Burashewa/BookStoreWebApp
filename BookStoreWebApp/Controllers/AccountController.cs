using System.Security.Cryptography;
using System.Text;
using BookStoreWebApp.Data;
using BookStoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace BookStoreWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

       

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var hashedPassword = HashPassword(model.Password);
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.email == model.Email && u.password == hashedPassword);

                if (user != null)
                {
                    // Store user session
                    HttpContext.Session.SetString("UserId", user.ID.ToString());
                    HttpContext.Session.SetString("IsAdmin", user.isAdmin ? "true" : "false");

                    if (user.isAdmin)
                    {
                        return RedirectToAction("AdminDashboard", "Admin");
                    }
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("Email", "Invalid email or password.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email already in use.");
                    return View(model);
                }

                var newUser = new Users
                {
                    username = model.UserName,
                    email = model.Email,
                    password = HashPassword(model.Password),
                    isAdmin = false // Default new users as non-admin
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login","Account");
            }

            return View(model);
        }

        //public IActionResult Logout()
        //{
        //    HttpContext.Session.Clear();
        //    return RedirectToAction("Index", "Home");
        //}

        


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
