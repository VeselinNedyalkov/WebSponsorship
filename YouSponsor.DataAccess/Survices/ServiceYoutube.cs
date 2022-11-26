﻿using Microsoft.EntityFrameworkCore;
using SponsorY.Data;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess;
using SponsorY.DataAccess.Survices.Contract;
using System.Collections.Generic;
using System.Data;

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
        public async Task<IEnumerable<YouTubeViewModel>> GetAllYoutubeChanelsAsync(string userId)
        {
            IEnumerable<Category> getCategories = await categorySerivece.GetAllCategoryAsync();

            var result = await context.Youtubers
                .Where(x => x.AppUserId == userId)
                .Select(x => new YouTubeViewModel
                {
                    Id = x.Id,
                    ChanelName = x.ChanelName,
                    Url = x.Url,
                    Subscribers = x.Subscribers,
                    PricePerClip = x.PricePerClip,
                    Wallet = x.Wallet,
                    Category = x.Category.CategoryName,
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
        public async Task<YouTubeViewModel> TakeYoutuberAsync(int id)
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
                    CategoryId = x.CategoryId,
                    AppUserId = x.AppUserId
                })
                .FirstOrDefaultAsync();

            model!.Categories = cat;

            return model;
        }



        public async Task EditYoutuberAsync(int EditId, YouTubeViewModel model)
        {
            var userYoutub = await TakeYoutuberAsync(EditId);


            var updated = new Youtuber
            {
                Id = userYoutub.Id,
                ChanelName = model.ChanelName,
                Url = model.Url,
                Subscribers = model.Subscribers,
                PricePerClip = model.PricePerClip,
                Wallet = userYoutub.Wallet,
                CategoryId = userYoutub.CategoryId,
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

		public async Task<IEnumerable<FindYoutuberViewModel>> FindAllYoutubersAsync()
		{
			var result = await context.Youtubers
			   .Select(x => new FindYoutuberViewModel
			   {
				   Id = x.Id,
				   ChanelName = x.ChanelName,
				   Url = x.Url,
				   Subscribers = x.Subscribers,
				   PricePerClip = x.PricePerClip,
			   })
			   .ToListAsync();

            return result;
		}

        public async Task<IEnumerable<YoutubersFilterCatViewModel>> GetChanelWithCategoryAsync(int categoryId)
        {
            var result = await context.Youtubers
                .Where(x => x.CategoryId == categoryId)
                .Select(x => new YoutubersFilterCatViewModel
                {
                    Id = x.Id,
                    ChanelName = x.ChanelName,
                    Url = x.Url,
                    Subscribers = x.Subscribers,
                    PricePerClip = x.PricePerClip,
                    Wallet = x.Wallet,
                    CategoryId = x.CategoryId,
                    AppUserId = x.AppUserId,
                }).ToListAsync();

            return result;
        }

		public async Task<IEnumerable<YoutuberAwaitTransactionViewModel>> GetAllTransactionsAwaitingAsync(string userId)
		{
            int youtubeId = await context.Youtubers.Where(x => x.AppUserId== userId).Select(x => x.Id).FirstOrDefaultAsync();


			var model = await context.Transactions
                .Where(x => x.YoutuberId == youtubeId && x.HasAccepted == false)
                .Select( x => new YoutuberAwaitTransactionViewModel
                {
                    MoneyOffer = x.TransferMoveney,
                    QuntityClips = x.QuntityClips,
                    TransactionId = x.Id,
                    CompanyName = x.Sponsorship.CompanyName,
                   Product = x.Sponsorship.Product,
                   ProductUrl = x.Sponsorship.Url
                })
                .ToListAsync();

            return model;
		}

		public async Task TransactionCompletedAsync(int transactionId)
		{
			var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == transactionId);

            transaction.HasAccepted = true;
            transaction.IsCompleted = true;

            context.Transactions.Update(transaction);

            var youtuber = await context.Youtubers.FirstOrDefaultAsync(x => x.Id == transaction.YoutuberId);
            var sponsor = await context.Sponsorships.FirstOrDefaultAsync(x => x.Id == transaction.SponsorshipId);

            if (sponsor.Wallet - transaction.TransferMoveney < 0)
            {
                throw new ArgumentException("Not enought money");
			}

            sponsor.Wallet -= transaction.TransferMoveney;
            youtuber.Wallet += transaction.TransferMoveney;

			context.Sponsorships.Update(sponsor);
			context.Youtubers.Update(youtuber);
            await context.SaveChangesAsync();
		}

		public async Task TransactionDenialAsync(int transactionId)
		{
			var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == transactionId);

            context.Transactions.Remove(transaction);
			await context.SaveChangesAsync();
		}
	}
}
