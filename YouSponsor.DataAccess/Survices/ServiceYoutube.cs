using Microsoft.EntityFrameworkCore;
using SponsorY.Data;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess.Youtube;
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


			if (model.Categories.Count() == 0)
			{
				model.Categories = cat;
			}
		

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
			//check
			var model = await context.Transactions
			.Where(x => x.YoutuberTransactions.Select(x => x.Youtuber.AppUserId).FirstOrDefault() == userId && x.HasAccepted == false)
			.Select(x => new YoutuberAwaitTransactionViewModel
			{
				MoneyOffer = x.TransferMoveney,
				QuntityClips = x.QuntityClips,
				TransactionId = x.Id,
				CompanyName = x.SponsorshipTransactions.Select(x => x.Sponsorship.CompanyName).FirstOrDefault(),
				Product = x.SponsorshipTransactions.Select(x => x.Sponsorship.Product).FirstOrDefault(),
				ProductUrl = x.SponsorshipTransactions.Select(x => x.Sponsorship.Url).FirstOrDefault(),
			})
			.ToListAsync();

			return model;
		}

		public async Task TransactionCompletedAsync(Guid transactionId)
		{
			var transaction = await context.Transactions.Where(x => x.Id == transactionId).Include(x => x.YoutuberTransactions)
				.Include(y => y.SponsorshipTransactions).FirstOrDefaultAsync();
			var youtubeId = transaction.YoutuberTransactions.Select(x => x.YoutuberId).FirstOrDefault();
			var sponsorId = transaction.SponsorshipTransactions.Select(x => x.SponsorId).FirstOrDefault();

			transaction.HasAccepted = true;
			transaction.IsCompleted = true;

			context.Transactions.Update(transaction);

			

			var youtuber = await context.Youtubers.FirstOrDefaultAsync(x => x.Id == youtubeId);
			var sponsor = await context.Sponsorships.FirstOrDefaultAsync(x => x.Id == sponsorId);


			youtuber.Wallet += transaction.TransferMoveney;
			string userId = youtuber.AppUserId;

			var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);

			user.Wallet += youtuber.Wallet;

			context.Youtubers.Update(youtuber);
			context.Users.Update(user);
			await context.SaveChangesAsync();
		}

		public async Task TransactionDenialAsync(Guid transactionId)
		{
			var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == transactionId);
			var sponsorId = transaction.SponsorshipTransactions.Select(x => x.SponsorId).FirstOrDefault();
			var sponsor = await context.Sponsorships.FirstOrDefaultAsync(x => x.Id == sponsorId);

			sponsor.Wallet += transaction.TransferMoveney;

			context.Sponsorships.Update(sponsor);
			context.Transactions.Remove(transaction);
			await context.SaveChangesAsync();
		}

		public async Task<YoutubeFinancesViewModel> GetAllFinancesaAsync(string userId)
		{
			var usersFinances = await context.Users.Where(x => x.Id == userId)
				.Select(x => new YoutubeFinancesViewModel
				{
					Wallet = x.Wallet,
				})
				.FirstOrDefaultAsync();


			return usersFinances;
		}

		public async Task WithdrawMOneyAsync(string userId, YoutubeFinancesViewModel model)
		{
			var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);

			if (user.Wallet < model.Wallet)
			{
				throw new InvalidOperationException("Not possible to transfer more money than you have in your account");
			}

			user.Wallet -= model.Wallet;

			context.Users.Update(user);
			await context.SaveChangesAsync();
		}

		public async Task<IEnumerable<YoutuberAwaitTransactionViewModel>> GetallCompletedTransactionsAsync(string userId)
		{
			var model = await context.Transactions
			.Where(x => x.YoutuberTransactions.Select(x => x.Youtuber.AppUserId).FirstOrDefault() == userId && x.IsCompleted == true)
			.Select(x => new YoutuberAwaitTransactionViewModel
			{
				MoneyOffer = x.TransferMoveney,
				QuntityClips = x.QuntityClips,
				TransactionId = x.Id,
				CompanyName = x.SponsorshipTransactions.Select(s => s.Sponsorship.CompanyName).FirstOrDefault(),
				Product = x.SponsorshipTransactions.Select(s => s.Sponsorship.Product).FirstOrDefault(),
				ProductUrl = x.SponsorshipTransactions.Select(s => s.Sponsorship.Url).FirstOrDefault(),
			})
			.ToListAsync();

			return model;
		}
	}
}
