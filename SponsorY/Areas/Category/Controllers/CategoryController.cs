using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SponsorY.Areas.User.Models;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess.Categories;
using SponsorY.DataAccess.Survices.Contract;
using System.Security.Claims;

namespace SponsorY.Areas.Category.Controllers
{
    [Area("Category")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly IServiceCategory categoryService;

        public CategoryController(IServiceCategory _categoryService)
        {
            categoryService = _categoryService;
        }

        public IActionResult Add()
        {

            CategoryViewModel model = new CategoryViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await categoryService.AddCategoryAync(model);

                TempData["success"] = "Category  added!";
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


    }
}
