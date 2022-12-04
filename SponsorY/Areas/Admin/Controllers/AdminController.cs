using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SponsorY.Areas.User.Models;
using SponsorY.DataAccess.ModelsAccess;
using SponsorY.DataAccess.Survices.Contract;
using System.Data;

namespace SponsorY.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "admin")]
	public class AdminController : Controller
    {
		private readonly IServiceCategory categoryService;
		private readonly IServiceAdmin adminService;

		public AdminController(IServiceCategory _categoryService,
			IServiceAdmin _adminService)
        {
			categoryService = _categoryService;
			adminService = _adminService;
		}


		public async Task<IActionResult> DeleteCat()
        {
            DelCategoryViewModel model = new DelCategoryViewModel
            {
                Categories = await categoryService.GetAllCategoryAsync()
            };
			return View(model);
        }

		public async Task<IActionResult> Delete(int DeleteId)
		{
			if (DeleteId == 0 && DeleteId == null)
			{
				return NotFound();
			}


			try
			{
				await categoryService.DeleteCategoryAsync(DeleteId);
			}
			catch (Exception e)
			{
				return View("Error", new ErrorViewModel { RequestId = e.Message });
			}

			return RedirectToAction("Index", "Home", new { area = "Home" });
		}

		public async Task<IActionResult> Statistic()
		{
			AdminStatisticViewModel model = new AdminStatisticViewModel
			{
				NumSponsorhips = await adminService.GetAllSponsorsAsync(),
				NumYoutubChanels = await adminService.GetAllYoutubeChanelsAsync()
			};
			
			return View(model);
		}
	}
}
