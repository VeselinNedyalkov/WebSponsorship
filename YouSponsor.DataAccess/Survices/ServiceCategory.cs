using Microsoft.EntityFrameworkCore;
using SponsorY.Data;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess.Categories;
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

		public async Task DeleteCategoryAsync(int DeleteId)
		{
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == DeleteId);

            context.Categories.Remove(category);
            await context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
            return  await context.Categories.ToListAsync();
        }

        public async Task<string> GetCategoryNameAsync(int catId)
        {
            return await context.Categories.Where(x => x.Id == catId).Select(x => x.CategoryName).FirstOrDefaultAsync();
        }

		public async Task<int> GetIdByNameAsync(string Name)
		{
            var catId = await context.Categories.Where(x => x.CategoryName == Name).Select(x => x.Id).FirstOrDefaultAsync();
            return catId;
		}
	}
}
