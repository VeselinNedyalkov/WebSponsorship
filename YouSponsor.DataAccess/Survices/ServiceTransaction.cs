using Microsoft.EntityFrameworkCore;
using SponsorY.Data;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess;
using SponsorY.DataAccess.Survices.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
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

			var sponsor = await context.Sponsorships.Where(x => x.Id == model.ChanelId).Select(x => x.Id).FirstOrDefaultAsync();
			var youtuber = await context.Youtubers.Where(x => x.Id == model.ChanelId).Select(x => x.Id).FirstOrDefaultAsync();

			//check
			Transaction addModel = new Transaction
			{
				TransferMoveney = model.TotalPrice,
				QuntityClips = model.QuantityClips,
				UserSponsorId = userId,
				SuccessfulCreated = false,
				HasAccepted = false,
			};

			await context.Transactions.AddAsync(addModel);
			await context.SaveChangesAsync();


			return addModel;
		}

		public async Task EditSaveAsync(TransactionViewModel model, string userId)
		{
			var trans = await context.Transactions
				.Where(x => x.Id == model.TransactionId)
				.FirstOrDefaultAsync();

			var sponsor = await context.Sponsorships.Where(x => x.Id == model.ChanelId).Select(x => x.Id).FirstOrDefaultAsync();
			var youtuber = await context.Youtubers.Where(x => x.Id == model.ChanelId).Select(x => x.Id).FirstOrDefaultAsync();

			//check
			Transaction edit = new Transaction
			{
				Id = trans.Id,
				TransferMoveney = model.TotalPrice,
				QuntityClips = model.QuantityClips,
				UserSponsorId = userId,
				SuccessfulCreated = trans.SuccessfulCreated,
				SponsorshipTransactions = trans.SponsorshipTransactions,
				YoutuberTransactions = trans.YoutuberTransactions,
				HasAccepted = trans.HasAccepted,
				IsCompleted = trans.IsCompleted,
			};



			context.Transactions.Update(edit);
			await context.SaveChangesAsync();
		}


		public async Task DeleteNotCompletedTransactions()
		{
			var transactions = await context.Transactions.Where(x => x.SuccessfulCreated == false).ToListAsync();

			foreach (var item in transactions)
			{
				context.Transactions.Remove(item);
			}

			await context.SaveChangesAsync();
		}

		public void DeleteTransactionAsync(Guid TransId)
		{
			Transaction tr = context.Transactions.Single(x => x.Id == TransId);

			context.Transactions.Remove(tr);
			context.SaveChanges();
		}

		

		public async Task<TransactionViewModel> EditTransactionAsync(Guid TransId)
		{

			//check

			TransactionViewModel model = await context.Transactions
				.Where(x => x.Id == TransId)
				.Select(x => new TransactionViewModel
				{
					SponsorId = x.SponsorshipTransactions.Select(x => x.SponsorId).FirstOrDefault(),
					CompanyName = x.SponsorshipTransactions.Select(x => x.Sponsorship.CompanyName).FirstOrDefault(),
					Product = x.SponsorshipTransactions.Select(x => x.Sponsorship.Product).FirstOrDefault(),
					CompanyUrl = x.SponsorshipTransactions.Select(x => x.Sponsorship.Url).FirstOrDefault(),
					CompanyBudget = x.SponsorshipTransactions.Select(x => x.Sponsorship.Wallet).FirstOrDefault(),
					QuantityClips = x.QuntityClips,
					ChanelId = x.YoutuberTransactions.Select(x => x.YoutuberId).FirstOrDefault(),
					ChanelName = x.YoutuberTransactions.Select(x => x.Youtuber.ChanelName).FirstOrDefault(),
					ChanelUrl = x.YoutuberTransactions.Select(x => x.Youtuber.Url).FirstOrDefault(),
					Subscribers = x.YoutuberTransactions.Select(x => x.Youtuber.Subscribers).FirstOrDefault(),
					SponroshipsClipsNum = x.QuntityClips,
					TotalPrice = x.TransferMoveney,
					TransactionId = x.Id
				})
				.FirstOrDefaultAsync();

			return model;
		}

		public async Task<IEnumerable<NotAcceptedTransactionViewModel>> GetAllUnaceptedTransaction(string userId)
		{

			//check
			IEnumerable<NotAcceptedTransactionViewModel> model = await context.Transactions
				.Where(x => x.UserSponsorId == userId && x.HasAccepted == false)
				.Select(x => new NotAcceptedTransactionViewModel
				{
					Id = x.Id,
					TransferMoveney = x.TransferMoveney,
					QuntityClips = x.QuntityClips,
					SponsorshipId = x.SponsorshipTransactions.Select(x => x.SponsorId).FirstOrDefault(),
					YoutubeChanelName = x.YoutuberTransactions.Select(x => x.Youtuber.ChanelName).FirstOrDefault(),
					ChanelSubscribers = x.YoutuberTransactions.Select(x => x.Youtuber.Subscribers).FirstOrDefault(),
				})
				.ToListAsync();

			return model;
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

		public async Task<Transaction> GetTransactionAsync(Guid TranslId)
		{
			return await context.Transactions.Where(x => x.Id == TranslId)
				.Include(x => x.YoutuberTransactions)
				.Include(x => x.SponsorshipTransactions)
				.FirstOrDefaultAsync();
		}

		public async Task UpdateCompletedTransactionAsync(Transaction model, int SponsorId, int ChanelId)
		{
			SponsorshipTransaction addSponsor = new SponsorshipTransaction
			{
				TransactionId = model.Id,
				SponsorId = SponsorId
			};

			YoutuberTransaction addYoutube = new YoutuberTransaction
			{
				TransactionId = model.Id,
				YoutuberId = ChanelId
			};

			
			model.SponsorshipTransactions.Add(addSponsor);
			model.YoutuberTransactions.Add(addYoutube);


			context.Transactions.Update(model);
			await context.SaveChangesAsync();
		}

		public async Task UpdateTransactionAsync(Transaction model)
		{
			context.Transactions.Update(model);
			await context.SaveChangesAsync();
		}

		public async Task RemoveMoneyFromSponsorAsync(int SponsorId,decimal Amount)
		{
			var sponsor = await context.Sponsorships.FirstOrDefaultAsync(x => x.Id == SponsorId);

			sponsor.Wallet -= Amount;

			context.Sponsorships.Update(sponsor);
			await context.SaveChangesAsync();
		}

		public async Task<FindChanelViewModel> ReworkModelAsync(FindChanelViewModel modelInput, int SponsorId)
		{
			var category = await categoryService.GetAllCategoryAsync();
			int catId = int.Parse(modelInput.CategoryName);
			var youtubers = await youtubeService.GetChanelWithCategoryAsync(catId);

			FindChanelViewModel model = new FindChanelViewModel
			{
				Youtubers = youtubers,
				Categories = category,
				SponsorshipId = SponsorId,
				CategoryName = modelInput.CategoryName
			};

			if (modelInput.Sorting == 0)
			{
				model.Youtubers.OrderBy(x => x.PricePerClip);
			}
			else
			{
				model.Youtubers.OrderByDescending(x => x.PricePerClip);
			}

			return model;
		}
	}
}
