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
    public class SponsorshipController : Controller
    {

        private readonly IServiceSponsorship sponsorService;

        public SponsorshipController(IServiceSponsorship _sponsorService)
        {
            sponsorService = _sponsorService;
        }
        public async Task<IActionResult> Main()
        {
            IEnumerable<SponsorViewModel> model = await sponsorService.GetAllSponsorshipsAsync();

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Add()
        {
            AddSponsorViewModel model = new AddSponsorViewModel();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
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

        [AllowAnonymous]
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
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int EditId, Sponsorship model)
        {

            await sponsorService.EditSponsorshipAsync(EditId, model);


            TempData["success"] = "Sponsorship updated!";
            return RedirectToAction(nameof(Main));
        }
    }

    
}
