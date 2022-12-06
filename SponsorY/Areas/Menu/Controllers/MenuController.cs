using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SponsorY.DataAccess.Survices;
using System.Security.Claims;
using SponsorY.DataAccess.Survices.Contract;
using SponsorY.Areas.User.Models;
using SponsorY.DataAccess.ModelsAccess.User;

namespace SponsorY.Areas.Menu.Controllers
{
    [Area("Menu")]
    [Authorize]
    public class MenuController : Controller
    {
        private readonly IServiceUser userService;
        private readonly IServiceMenu serviceMenu;

        public MenuController(IServiceUser _userService,
			IServiceMenu _serviceMenu)
        {
            userService = _userService;
            serviceMenu = _serviceMenu;

		}

        public async Task<IActionResult> AddUserInfo()
        {
			var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            UserInfoViewModel model = null;

            try
            {
				model = await serviceMenu.GetUserInfo(userId);
				TempData["success"] = "User information updated";

			}
			catch 
            {
				return View("Error", new ErrorViewModel { RequestId = $"Information about user was not updated" });

			}


			return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserInfo(UserInfoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                await userService.UpdateUserInfoAsync(userId, model);
            }
            catch (Exception e)
            {
                var error = new ErrorViewModel
                {
                    RequestId = e.Message
                };

                return View("Error", error);
            }
			return RedirectToAction("Main", "Home", new { area = "Home" });

		}


		public async Task<IActionResult> DeleteUser()
		{
            try
            {
				var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                await userService.DeleteUserAsync(userId);
			}
            catch
            {
				return View("Error", new ErrorViewModel { RequestId = $"Someting go wrong" });

			}

			return RedirectToAction("Logout", "User", new { area = "User" });
		}

		//TO DO
		public IActionResult Settings()
        {


            return View();
        }
    }
}
