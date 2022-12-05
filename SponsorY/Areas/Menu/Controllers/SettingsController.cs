using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SponsorY.Areas.Menu.Controllers
{
	[Area("Menu")]
    [Authorize]
	public class SettingsController : Controller
    {
        public IActionResult Settings()
        {
            return View();
        }
    }
}
