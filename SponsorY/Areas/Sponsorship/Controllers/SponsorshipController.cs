using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SponsorY.Areas.User.Models;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess;
using SponsorY.DataAccess.Survices.Contract;
using System.Security.Claims;

namespace SponsorY.Areas.Sponsorship.Controllers
{
    [Area("Sponsorship")]
    [Authorize]
    public class SponsorshipController : Controller
    {
        private readonly IServiceCategory categoryService;
        private readonly IServiceSponsorship sponsorService;

        public SponsorshipController(IServiceSponsorship _sponsorService
            , IServiceCategory _categoryService)
        {
            sponsorService = _sponsorService;
            categoryService = _categoryService;
        }
        public async Task<IActionResult> Main()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;


            IEnumerable<SponsorViewModel> model = await sponsorService.GetAllSponsorshipsAsync(userId);

            return View(model);
        }

        public async Task<IActionResult> Add()
        {
            var category = await categoryService.GetAllCategoryAsync();

            AddSponsorViewModel model = new AddSponsorViewModel
            {
                Categories = category
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddSponsorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                await sponsorService.AddSponsorshipAsync(userId, model);
                TempData["success"] = "Sponsorship  added!";
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

        public async Task<IActionResult> Edit(int EditId)
        {
            if (EditId == 0)
            {
                return NotFound();
            }

            var model = await sponsorService.GetSponsorsEditAsync(EditId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int EditId, SponsorViewModel model)
        {

            try
            {
				await sponsorService.EditSponsorshipAsync(EditId, model);
			}
            catch (Exception e)
            {
				var error = new ErrorViewModel
				{
					RequestId = e.Message
				};

				return View("Error", error);
			}

            TempData["success"] = "Sponsorship updated!";
            return RedirectToAction(nameof(Main));
        }

        public async Task<IActionResult> Finances(int SponsorId)
        {
			if (SponsorId == 0)
			{
				return NotFound();
			}

			var model = await sponsorService.GetSponsorsEditAsync(SponsorId);

			return View(model);
        }

        [HttpPost]
		public async Task<IActionResult> Finances(int SponsorId,SponsorViewModel model)
		{
			try
			{
				await sponsorService.AddMoneyToSponsorAsync(SponsorId ,model);
			}
			catch (Exception e)
			{
				var error = new ErrorViewModel
				{
					RequestId = e.Message
				};

				return View("Error", error);
			}

			TempData["success"] = "Sponsorship finances updated!";
			return RedirectToAction(nameof(Main));
		}

        public async Task<IActionResult> Remove(int SponsorId, SponsorViewModel model)
        {
			try
			{
				await sponsorService.RemoveMoneyFromSponsorAsync(SponsorId, model);
			}
			catch (Exception e)
			{
				var error = new ErrorViewModel
				{
					RequestId = e.Message
				};

				return View("Error", error);
			}

			TempData["success"] = "Sponsorship finances updated!";
			return RedirectToAction(nameof(Main));
		}
	}


}
