using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using SponsorY.Data;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess.Sponsor;
using SponsorY.DataAccess.Survices.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SponsorY.DataAccess.Survices
{
	public class ServiceSponsorship : IServiceSponsorship
	{
		private readonly ApplicationDbContext context;
		private readonly IServiceCategory categoryService;
		private readonly HtmlSanitizer sanitize = new HtmlSanitizer();

		public ServiceSponsorship(ApplicationDbContext _context,
			IServiceCategory _categoryService)
		{
			context = _context;
			categoryService = _categoryService;
		}

		public async Task AddMoneyToSponsorAsync(int SponsorId, SponsorViewModel model)
		{
			var sponsor = await context.Sponsorships.Where(x => x.Id == SponsorId).FirstOrDefaultAsync();
			sponsor.Wallet += model.ValueMoney;


			await context.SaveChangesAsync();
		}

		public async Task AddSponsorshipAsync(string userId, AddSponsorViewModel model)
		{
			Sponsorship sponsor = new Sponsorship
			{
				CompanyName = sanitize.Sanitize(model.CompanyName),
				Product = sanitize.Sanitize(model.Product),
				Url = sanitize.Sanitize(model.Url),
				Wallet = model.Wallet,
				AppUserId = userId,
				CategoryId = model.CategoryId
			};

			await context.Sponsorships.AddAsync(sponsor);
			context.SaveChanges();
		}

		public async Task DeleteSponsorshipOfferAsync(int DeletedId)
		{
			var deleteItem = await context.Sponsorships
				.FirstOrDefaultAsync(x => x.Id == DeletedId);

			context.Sponsorships.Remove(deleteItem);
			await context.SaveChangesAsync();
		}

		public async Task EditSponsorshipAsync(int EditId, SponsorViewModel model)
		{
			var original = await GetSponsorsEditAsync(EditId);

			Sponsorship edit = new Sponsorship
			{
				Id = original.Id,
				CompanyName = sanitize.Sanitize(model.CompanyName),
				Product = sanitize.Sanitize(model.Product),
				Url = sanitize.Sanitize(model.Url),
				Wallet = model.Wallet,
				CategoryId = model.CategoryId,
				AppUserId = original.AppUserId,
			};

			context.Sponsorships.Update(edit);
			await context.SaveChangesAsync();
		}


		public async Task<IEnumerable<SponsorViewModel>> GetAllSponsorshipsAsync(string userId)
		{
			var result = await context.Sponsorships
				.Where(x => x.AppUserId == userId)
				.Select(x => new SponsorViewModel
				{
					Id = x.Id,
					CompanyName = x.CompanyName,
					Product = x.Product,
					Url = x.Url,
					CategoryId = x.CategoryId,
					Wallet = x.Wallet,
				}).ToListAsync();

			foreach (var item in result)
			{
				item.CategoryName = await categoryService.GetCategoryNameAsync(item.CategoryId);

			}

			return result;
		}

		public async Task<Sponsorship> GetSingelSponsorAsync(int SponsorId)
		{
			return await context.Sponsorships.Where(x => x.Id == SponsorId).FirstOrDefaultAsync();
		}

		public async Task<SponsorViewModel> GetSponsorsEditAsync(int id)
		{
			var user = await context.Sponsorships.Where(x => x.Id == id)
				.Select(x => new SponsorViewModel
				{
					Id = x.Id,
					CompanyName = x.CompanyName,
					Product = x.Product,
					Url = x.Url,
					Wallet = x.Wallet,
					AppUserId = x.AppUserId,
				}).FirstOrDefaultAsync();

			user.CategoryName = await categoryService.GetCategoryNameAsync(user.CategoryId);
			user.Categories = await categoryService.GetAllCategoryAsync();

			return user!;
		}

		/// <summary>
		/// Reduce the money of the sponsor with ID
		/// </summary>
		/// <param name="SponsorId"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public async Task RemoveMoneyFromSponsorAsync(int SponsorId, SponsorViewModel model)
		{
			var sponsor = await context.Sponsorships.Where(x => x.Id == SponsorId).FirstOrDefaultAsync();

			sponsor.Wallet -= model.ValueMoney;

			if (sponsor.Wallet < 0)
			{
				throw new InvalidOperationException("You don`t have enough money!");
			}

			await context.SaveChangesAsync();
		}

		public async Task<IEnumerable<SponsorHistoryViewModel>> TakeAllCompletedTransactions(string userId)
		{
			var model = await context.Transactions
				.Where(x => x.AppUserId == userId && x.IsCompleted == true)
				.Select(s => new SponsorHistoryViewModel
				{
					Product = s.SponsorshipTransactions.Select(x => x.Sponsorship.Product).FirstOrDefault(),
					CompanyUrl = s.SponsorshipTransactions.Select(x => x.Sponsorship.Url).FirstOrDefault(),
					ChanelName = s.YoutuberTransactions.Select(x => x.Youtuber.ChanelName).FirstOrDefault(),
					ChanelUrl = s.YoutuberTransactions.Select(x => x.Youtuber.Url).FirstOrDefault(),
					Subscribers = s.YoutuberTransactions.Select(x => x.Youtuber.Subscribers).FirstOrDefault(),
					PricePerClip = s.YoutuberTransactions.Select(x => x.Youtuber.PricePerClip).FirstOrDefault(),
					SponroshipsClipsNum = s.QuntityClips,
					TotalPrice = s.TransferMoveney
				})
				.ToListAsync();

			return model;
		}
	}
}
