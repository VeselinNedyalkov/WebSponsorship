using Microsoft.EntityFrameworkCore;
using SponsorY.Data;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess;
using SponsorY.DataAccess.Survices.Contract;
using System.Collections.Generic;

namespace SponsorY.DataAccess.Survices
{
    public class ServiceYoutube : IServiceYoutub
    {
        private readonly ApplicationDbContext context;
        private readonly IServiceCategory categorySerivece;

        public ServiceYoutube(ApplicationDbContext _context,
            IServiceCategory _categorySerivece)
        {
            context = _context;
            categorySerivece = _categorySerivece;
        }



        /// <summary>
        /// Get all youtube chanels 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<YouTubeViewModel>> GetAllYoutubeChanlesAsync()
        {
            IEnumerable<Category> getCategories = await categorySerivece.GetAllCategoryAsync();

            var result = await context.Youtubers
                .Select(x => new YouTubeViewModel
                {
                    Id = x.Id,
                    ChanelName = x.ChanelName,
                    Url = x.Url,
                    Subscribers = x.Subscribers,
                    PricePerClip = x.PricePerClip,
                    Wallet = x.Wallet,
                    Category = x.Category.CategoryName,
                    TransferId = x.TransferId,
                    CategoryId = x.CategoryId,
                    AppUserId = x.AppUserId,
                })
                .ToListAsync();

            foreach (var item in result)
            {
                item.Categories = getCategories;
            }

            return result;
        }




        /// <summary>
        /// Add new youtuber
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task AddYoutubChanelAsync(string userId, AddYoutViewModel model)
        {

            Youtuber chanel = new Youtuber
            {
                ChanelName = model.ChanelName,
                Url = model.Url,
                Subscribers = model.Subscribers,
                PricePerClip = model.PricePerClip,
                Wallet = 0,
                CategoryId = model.CategoryId,
                TransferId = null,
                AppUserId = userId
            };

            await context.Youtubers.AddAsync(chanel);
            context.SaveChanges();
        }

        /// <summary>
        /// Get youtuber for editing
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<YouTubeViewModel> GetYoutuberEditAsync(int id)
        {
            var cat = await categorySerivece.GetAllCategoryAsync();

            YouTubeViewModel model = await context.Youtubers
                .Include(x => x.Category)
                .Where(x => x.Id == id)
                .Select(x => new YouTubeViewModel
                {
                    Id = x.Id,
                    ChanelName = x.ChanelName,
                    Url = x.Url,
                    Subscribers = x.Subscribers,
                    PricePerClip = x.PricePerClip,
                    Wallet = x.Wallet,
                    Category = x.Category.CategoryName,
                    TransferId = x.TransferId,
                    CategoryId = x.CategoryId,
                    AppUserId = x.AppUserId
                })
                .FirstOrDefaultAsync();

            model!.Categories = cat;

            return model;
        }



        public async Task EditYoutuberAsync(int EditId, YouTubeViewModel model)
        {
            var userYoutub = await GetYoutuberEditAsync(EditId);


            var updated = new Youtuber
            {
                Id = userYoutub.Id,
                ChanelName = model.ChanelName,
                Url = model.Url,
                Subscribers = model.Subscribers,
                PricePerClip = model.PricePerClip,
                Wallet = userYoutub.Wallet,
                CategoryId = userYoutub.CategoryId,
                TransferId = userYoutub.TransferId,
                AppUserId = userYoutub.AppUserId
            };
            context.Youtubers.Update(updated);
            context.SaveChanges();
        }

        public void DeleteYoutuber(int DelteId)
        {
            var user = context.Youtubers.Single(x => x.Id == DelteId);

            context.Youtubers.Remove(user);
            context.SaveChanges();
        }
    }
}
