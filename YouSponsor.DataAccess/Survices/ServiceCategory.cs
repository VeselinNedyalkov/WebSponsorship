using Microsoft.EntityFrameworkCore;
using SponsorY.Data;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess;
using SponsorY.DataAccess.Survices.Contract;

namespace SponsorY.DataAccess.Survices
{
    public class ServiceCategory : IServiceCategory
    {
        private readonly ApplicationDbContext context;

        public ServiceCategory(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task AddCategoryAync(CategoryViewModel model)
        {
            Category category = new Category
            {
                CategoryName = model.CategoryName,
            };

            await context.Categories.AddAsync(category);
            context.SaveChanges();
        }

        public async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
            return  await context.Categories.ToListAsync();
        }

        public async Task<string> GetCategoryNameAsync(int catId)
        {
            return await context.Categories.Where(x => x.Id == catId).Select(x => x.CategoryName).FirstOrDefaultAsync();
        }
    }
}
