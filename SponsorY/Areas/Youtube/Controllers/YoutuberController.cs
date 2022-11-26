using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SponsorY.Areas.User.Models;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess;
using SponsorY.DataAccess.Survices.Contract;
using System.Security.Claims;

namespace SponsorY.Areas.Youtube.Controllers
{
	[Area("Youtube")]
	[Authorize]
	public class YoutuberController : Controller
	{
		private readonly IServiceYoutub youtubService;
		private readonly IServiceCategory categoryService;
		public YoutuberController(IServiceYoutub _userService, IServiceCategory _categoryService)
		{
			youtubService = _userService;
			categoryService = _categoryService;
		}

		public async Task<IActionResult> Main()
		{

			var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

			IEnumerable<YouTubeViewModel> model = await youtubService.GetAllYoutubeChanelsAsync(userId);

			return View(model);
		}

		public async Task<IActionResult> Add()
		{
			AddYoutViewModel model = new AddYoutViewModel();


			model.Categories = await categoryService.GetAllCategoryAsync();

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddYoutViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			try
			{
				var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
				await youtubService.AddYoutubChanelAsync(userId, model);
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

		public async Task<IActionResult> Edit(int EditId)
		{
			if (EditId == 0)
			{
				return NotFound();
			}

			var model = await youtubService.TakeYoutuberAsync(EditId);
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int EditId, YouTubeViewModel model)
		{

			await youtubService.EditYoutuberAsync(EditId, model);


			TempData["success"] = "Youtube chanel updated!";
			return RedirectToAction(nameof(Main));
		}

		public IActionResult Delete(int DelteId)
		{
			if (DelteId == 0)
			{
				return NotFound();
			}

			youtubService.DeleteYoutuber(DelteId);
			TempData["success"] = "Youtube chanel deleted!";
			return RedirectToAction(nameof(Main));
		}

		public async Task<IActionResult> TransAwait()
		{
			IEnumerable<YoutuberAwaitTransactionViewModel> model = null;
			var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

			try
			{
				model = await youtubService.GetAllTransactionsAwaitingAsync(userId);

			}
			catch
			{
				return View(new ErrorViewModel { RequestId = "Ops something go wrong" });

			}

			return View(model);
		}

		public async Task<IActionResult> Accept(int TransId)
		{

			try
			{
				await youtubService.TransactionCompletedAsync(TransId);

			}
			catch (Exception ex)
			{
				return View(new ErrorViewModel { RequestId = ex.Message });
			}

			return RedirectToAction(nameof(TransAwait));
		}

		public async Task<IActionResult> Denial(int TransId)
		{
			try
			{
				await youtubService.TransactionDenialAsync(TransId);
			}
			catch (Exception ex)
			{
				return View(new ErrorViewModel { RequestId = ex.Message });
			}

			return RedirectToAction(nameof(TransAwait));
		}
	}
}
