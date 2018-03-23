using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Todos";

            var user = await this.userManager.GetUserAsync(this.User);
            if (user != null)
            {
                ViewData["UserId"] = user.Id;
                ViewData["UserName"] = user.FullName;
                ViewData["User"] = this.User.Identity?.Name;
                ViewData["Role"] = this.User.Claims.FirstOrDefault(x => x.Type == "Role").Value;
            }

            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }
    }
}
