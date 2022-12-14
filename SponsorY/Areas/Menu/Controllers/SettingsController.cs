using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SponsorY.Areas.User.Models;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess.Settings;
using SponsorY.DataAccess.Survices.Contract;
using System.Security.Claims;

namespace SponsorY.Areas.Menu.Controllers
{
	[Area("Menu")]
	[Authorize]
	public class SettingsController : Controller
	{
		private readonly IServiceSettings settingSerice;

		public UserManager<AppUser> UserManager { get; }

		public SettingsController(IServiceSettings _settingSerice,
			UserManager<AppUser> _userManager)
		{
			settingSerice = _settingSerice;
			UserManager = _userManager;
		}

		public IActionResult Settings()
		{
			return View();
		}

		public IActionResult ChangePass()
		{
			ChangePassViewModel model = new ChangePassViewModel();

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> ChangePass(ChangePassViewModel model)
		{
			var user = await UserManager.GetUserAsync(this.User);

			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("Wrong password", "Wrong password");

				return View(model);
			}
			else
			{
				try
				{
					if (await UserManager.CheckPasswordAsync(user, model.Password))
					{
						await UserManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
						TempData["success"] = "Password successfully changed";
					}
					else
					{
						model.Error = "* Wrong password *";
					}
				}
				catch
				{
					var error = new ErrorViewModel
					{
						RequestId = "Sorry problem"
					};

					return View("Error", error);
				}

				return RedirectToAction("Index", "Home", new { area = "Home" });

			}

		}
	}
}
