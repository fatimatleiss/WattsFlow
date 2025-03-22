using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WattsFlowProject.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using WattsFlowProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;  

namespace WattsFlowProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly WattsFlowDbContext _context;

        public AccountController(WattsFlowDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                 var passwordHasher = new PasswordHasher<User>();
                var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

                if (result == PasswordVerificationResult.Success)
                {
                     var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                        new Claim(ClaimTypes.Role, user.Role.RoleName),
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true, 
                    };

                     await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                     return RedirectToAction("Index", "Customer");
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid email or password.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "User not found.";
            }

             return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Clear();
            Response.Cookies.Delete(".AspNetCore.Cookies");
            Response.Cookies.Delete("YourCustomSessionCookie"); 

            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
