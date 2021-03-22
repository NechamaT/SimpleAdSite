using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using SimpleAdAuth.Data;

namespace SimpleAdAuth.Controllers
{
    public class AccountController : Controller
    {
        private string _connectionString =
            "Data Source=.\\sqlexpress;Initial Catalog=SimpleAdsAuth;Integrated Security=true;";

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(User user, string password)
        {
            var db = new SimpleAdAuthDb(_connectionString);
            db.AddUser(user, password);
            return Redirect("/account/login");
         
        }

        public IActionResult Login()
        {
            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"];
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var db = new SimpleAdAuthDb(_connectionString);
            var user = db.Login(password, email);
            if (user == null)
            {
                TempData["message"] = "Invalid email/password combination. Please try again";
                return Redirect("account/login");
            }

            var claims = new List<Claim>
            {
                new Claim("user", email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();
            return Redirect("/Home/NewAd");

        }
    }
}
