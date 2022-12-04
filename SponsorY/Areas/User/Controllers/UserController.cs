using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SponsorY.Areas.User.Models;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.Survices.Contract;

namespace SponsorY.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]

    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IServiceUser serviceUser;

        public UserController(RoleManager<IdentityRole> _roleManager,
            UserManager<AppUser> _userManager,
            SignInManager<AppUser> _signInManager,
            IServiceUser _serviceUser)
        {
            roleManager = _roleManager;
            userManager = _userManager;
            signInManager = _signInManager;
            serviceUser = _serviceUser;

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home", new { area = "Home" });

            }

            var model = new RegisterViewModel();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new AppUser()
            {
                Email = model.Email,
                UserName = model.UserName
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded && model.IsDeleted)
            {
                var error = new ErrorViewModel
                {
                    RequestId = "The User is deleted"
                };

                return View("Error", error);
            }
            else if (!result.Succeeded)
            {
                var error = new ErrorViewModel
                {
                    RequestId = "The User is already registered!"
                };

                return View("Error", error);
            }


            switch (model.Role)
            {
                case 0:
                    await this.userManager.AddToRoleAsync(user, "youtuber");
                    break;

                case 1:
                    await this.userManager.AddToRoleAsync(user, "sponsor");
                    break;

                default:
                    break;
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home", new { area = "Home" });
            }

            var model = new LoginViewModel();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }



            var user = await userManager.FindByNameAsync(model.UserName);

            if (user != null && user.IsDeleted)
            {
                return View(model);
            }

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Main", "Home", new { area = "Home" });
                }
            }

            ModelState.AddModelError("", "Invalid login");

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home", new { area = "Home" });

        }
    }
}
