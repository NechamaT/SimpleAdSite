using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleAdAuth.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SimpleAdAuth.Data;

namespace SimpleAdAuth.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString =
            "Data Source=.\\sqlexpress;Initial Catalog=SimpleAdsAuth;Integrated Security=true;";

        public IActionResult Index()
        {
            var db = new SimpleAdAuthDb(_connectionString);
            var vm = new IndexViewModel();
            vm.Ads = db.GetAllAds();
            if (User.Identity.IsAuthenticated)
            {
                var email = User.Identity.Name;
                vm.CurrentUser = db.GetUserByEmail(email);
            }

            return View(vm);
        }

        [Authorize]

        public IActionResult NewAd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewAd(Ad ad)
        {
            var db = new SimpleAdAuthDb(_connectionString);
            var vm = new NewAdViewModel()
            {
                IsAuthenticated = User.Identity.IsAuthenticated
            };
            if (User.Identity.IsAuthenticated)
            {
                var email = User.Identity.Name;
                User user = db.GetUserByEmail(email);
                ad.UserId = user.Id;
                vm.CurrentUser = user;
                db.NewAd(ad);
            }
           
            return Redirect("/Home/Index");
        }

       [HttpPost]
        public IActionResult Delete(int id)
        {
            var db = new SimpleAdAuthDb(_connectionString);
            db.Delete(id);
            return Redirect("/");
        }

      
        

     
    }
}
