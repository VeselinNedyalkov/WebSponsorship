using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SponsorY.DataAccess.ModelsAccess;
using SponsorY.DataAccess.Survices;
using System.Security.Claims;
using SponsorY.DataAccess.Survices.Contract;
using SponsorY.Areas.User.Models;

namespace SponsorY.Areas.Menu.Controllers
{
    [Area("Menu")]
    [Authorize]
    public class MenuController : Controller
    {
        private readonly IServiceUser userService;

        public MenuController(IServiceUser _userService)
        {
            userService = _userService;
        }

        public IActionResult AddUserInfo()
        {
            var model = new UserInfoViewModel();

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



            return RedirectToAction(nameof(AddUserInfo));
        }

        //TO DO
        public IActionResult Delete()
        {

            return View();
        }

        //TO DO
        public IActionResult Settings()
        {


            return View();
        }
    }
}
