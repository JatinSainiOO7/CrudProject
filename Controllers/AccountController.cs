using CrudProject.Data;
using CrudProject.Models;
using CrudProject.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CrudProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {

            if (HttpContext.Session.GetString("UserId") != null)
            {
                return RedirectToAction("Index", "Students");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string Pass = model.Password;
            var PassBytes = Encoding.UTF8.GetBytes(Pass);
            var LogPass = Convert.ToBase64String(PassBytes);

            model.Password = LogPass;

            var user = await _context.StudentsUsers.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(model);
            }

            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserName", user.UserName);
            HttpContext.Session.SetString("Email", user.Email);
            HttpContext.Session.SetString("UserRole", user.UserRole);

            return RedirectToAction("Index", "Students");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var existingUser = await _context.StudentsUsers
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("", "Email already exists");
                return View(model);
            }


            string Pass = model.Password;
            var PassBytes = Encoding.UTF8.GetBytes(Pass);
            var EncodedPass = Convert.ToBase64String(PassBytes);
            model.Password = EncodedPass;


            StudentUsers user = new StudentUsers
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
                CreatedDate = DateTime.Now,
                UserRole = model.UserRole,
                IsActive = model.IsActive
            };

            _context.StudentsUsers.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Account");
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.StudentsUsers.FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Something is wrong");
                    return View(model);
                }
                else
                {
                    return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
                }
            }
            return View(model);
        }

        public IActionResult ChangePassword(string username)
        {
            var user = _context.StudentsUsers.FirstOrDefault(u => u.UserName == username);
            if (user == null) return RedirectToAction("VerifyEmail");

            return View(new ChangePasswordViewModel
            {
                Email = user.Email
            });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.StudentsUsers
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "User not found");
                return View(model);
            }


            string Pass = model.NewPassword;
            var PassBytes = Encoding.UTF8.GetBytes(Pass);
            var EncodedPass = Convert.ToBase64String(PassBytes);
            model.NewPassword = EncodedPass;


            user.Password = model.NewPassword;

            _context.StudentsUsers.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Logout()
        {

            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Logout_Get()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
