using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess;
using SponsorY.DataAccess.Survices.Contract;
using SponsorY.Models;
using System.Security.Claims;

namespace SponsorY.Controllers
{
    [Authorize]
    public class YoutuberController : Controller
    {
        private readonly IServiceYoutub userService;
        private readonly IServiceCategory categoryService;
        public YoutuberController(IServiceYoutub _userService, IServiceCategory _categoryService)
        {
            userService = _userService;
            categoryService = _categoryService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Main()
        {

            IEnumerable<YouTubeViewModel> model = await userService.GetAllYoutubeChanlesAsync();

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Add()
        {
            AddYoutViewModel model = new AddYoutViewModel();

            model.Categories = await categoryService.GetAllCategoryAsync();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Add(AddYoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                await userService.AddYoutubChanelAsync(userId, model);
                TempData["success"] = "Youtube chanel added!";
            }
            catch (Exception e)
            {
                var error = new ErrorViewModel
                {
                    RequestId = e.Message
                };

                return View("Error", error);
            }


            return RedirectToAction(nameof(Main));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Edit(int EditId)
        {
            if (EditId == 0)
            {
                return NotFound();
            }

            var model = await userService.GetYoutuberEditAsync(EditId);
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int EditId, YouTubeViewModel model)
        {

            await userService.EditYoutuberAsync(EditId, model);


            TempData["success"] = "Youtube chanel updated!";
            return RedirectToAction(nameof(Main));
        }

        public IActionResult Delete(int DelteId)
        {
            if (DelteId == 0)
            {
                return NotFound();
            }

            userService.DeleteYoutuber(DelteId);
            TempData["success"] = "Youtube chanel deleted!";
            return RedirectToAction(nameof(Main));
        }
    }
}
