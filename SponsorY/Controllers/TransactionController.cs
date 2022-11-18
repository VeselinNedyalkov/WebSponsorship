using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SponsorY.DataAccess.ModelsAccess;
using SponsorY.DataAccess.Survices.Contract;

namespace SponsorY.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {

		private readonly IServiceTransaction tranService;
		private readonly IServiceCategory categoryService;
		private readonly IServiceYoutub youtubeService;
		private readonly IServiceSponsorship sponsorService;

        public TransactionController(IServiceTransaction _tranService
			, IServiceCategory _categoryService,
			IServiceYoutub _youtubeService,
			IServiceSponsorship _sponsorCategory)
		{
			tranService = _tranService;
			categoryService = _categoryService;
			youtubeService = _youtubeService;
			sponsorService = _sponsorCategory;
		}

		[AllowAnonymous]
        public async Task<IActionResult> Find(int SponsorId)
        {
			var category = await categoryService.GetAllCategoryAsync();
			var sponsorCategory = await sponsorService.GetSingelSponsorAsync(SponsorId);
			var youtubers = await youtubeService.GetChanelWithCategoryAsync(sponsorCategory.CategoryId);


            FindChanelViewModel model = new FindChanelViewModel
			{
				Youtubers = youtubers,
				Categories = category,
				SponsorshipId = SponsorId,
			};

			return View(model);
        }

		[AllowAnonymous]
		[HttpPost]
		public IActionResult Find(FindChanelViewModel model, int SponsorId)
		{
			model.SponsorshipId = SponsorId;

			return RedirectToAction("Search", model);
		}


		
	}
}
