﻿using Microsoft.EntityFrameworkCore;
using SponsorY.Data;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess;
using SponsorY.DataAccess.Survices.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.Survices
{
	public class ServiceTransaction : IServiceTransaction
	{
		private readonly ApplicationDbContext context;
		private readonly IServiceYoutub youtubeService;
		private readonly IServiceSponsorship sponsorService;
		private readonly IServiceCategory categoryService;

		public ServiceTransaction(ApplicationDbContext _context,
			IServiceYoutub _youtubeService,
			IServiceSponsorship _sponsorCategory,
			IServiceCategory _categoryService)
		{
			context = _context;
			youtubeService = _youtubeService;
			sponsorService = _sponsorCategory;
			categoryService = _categoryService;
		}

		public async Task<TransactionViewModel> CreatedTransactionViewModelAsync(int ChanelId, int SponsorId)
		{
			var youtub = await youtubeService.TakeYoutuberAsync(ChanelId);
			var sponsor = await sponsorService.GetSponsorsEditAsync(SponsorId);

			TransactionViewModel model = new TransactionViewModel
			{
				SponsorId = SponsorId,
				CompanyName = sponsor.CompanyName,
				Product = sponsor.Product,
				CompanyUrl = sponsor.Url,
				CompanyBudget = sponsor.Wallet,
				QuantityClips = 1,
				ChanelId = ChanelId,
				ChanelName = youtub.ChanelName,
				ChanelUrl = youtub.Url,
				Subscribers = youtub.Subscribers,
				PricePerClip = youtub.PricePerClip,
				SponroshipsClipsNum = 1,
				TotalPrice = 0
			};

			return model;
		}

		public async Task<Transaction> CreateTransactionAsync(TransactionViewModel model, string userId)
		{

			Transaction addModel = new Transaction
			{
				TransferMoveney = model.TotalPrice,
				QuntityClips = model.QuantityClips,
				SponsorshipId = model.SponsorId,
				YoutuberId = model.ChanelId,
				AllUserSponsor = userId,
				SubmiteToYoutuber = false,
				HasAccepted = false,
			};

			await context.Transactions.AddAsync(addModel);
			await context.SaveChangesAsync();

			return addModel;
		}

		public async Task<FindChanelViewModel> GetFindModelAsync(int SponsorId)
		{
			var category = await categoryService.GetAllCategoryAsync();
			var sponsorCategory = await sponsorService.GetSingelSponsorAsync(SponsorId);
			var youtubers = await youtubeService.GetChanelWithCategoryAsync(sponsorCategory.CategoryId);
			string catName = await categoryService.GetCategoryNameAsync(sponsorCategory.CategoryId);

			FindChanelViewModel model = new FindChanelViewModel
			{
				Youtubers = youtubers,
				Categories = category,
				SponsorshipId = SponsorId,
				CategoryName = catName
			};

			return model;
		}

		public decimal GetTotalPrice(int quantity, decimal PricePerClip)
		{
			return quantity * PricePerClip;
		}

		public async Task<Transaction> GetTransactionAsync(int TranslId)
		{
			return await context.Transactions.FirstOrDefaultAsync(x => x.Id == TranslId);
		}

		public async Task UpdateTransaction(Transaction model)
		{
			context.Transactions.Update(model);
			await context.SaveChangesAsync();
		}
	}
}
