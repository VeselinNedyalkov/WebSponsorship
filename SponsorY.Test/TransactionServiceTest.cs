using Microsoft.EntityFrameworkCore;
using SponsorY.Data;
using SponsorY.DataAccess.Survices.Contract;
using SponsorY.DataAccess.Survices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess.Transaction;
using System.Security.Cryptography;

namespace SponsorY.Test
{
	public class TransactionServiceTest
	{
		Sponsorship sponsor = null;
		Youtuber youtuber = null;
		Category category = null;
		private Transaction transaction = null;
		private Transaction transactionNC = null;
		public TransactionServiceTest()
		{
			transaction = new Transaction
			{
				Id = Guid.Parse("0c27bd01-5545-41dc-a3ee-d219ca0a5498"),
				TransferMoveney = 500,
				QuntityClips = 10,
				AppUserId = "1",
				SuccessfulCreated = true,
				HasAccepted = false,
				IsCompleted = false,
			};

			transactionNC = new Transaction
			{
				Id = Guid.Parse("48baae66-3d61-4e6f-8403-8300dd44c085"),
				TransferMoveney = 1,
				QuntityClips = 1,
				AppUserId = "1",
				SuccessfulCreated = false,
				HasAccepted = false,
				IsCompleted = false,
			};
			sponsor = new Sponsorship
			{
				Id = 1,
				CompanyName = "test",
				Product = "beer",
				Url = "More beer",
				Wallet = 2000,
				CategoryId = 1,
				AppUserId = "1",
			};
			youtuber = new Youtuber
			{
				Id = 1,
				ChanelName = "Test",
				Url = "Url",
				Subscribers = 12,
				PricePerClip = 10,
				Wallet = 5,
				CategoryId = 1,
				AppUserId = "1"
			};
			category = new Category
			{
				Id = 1,
				CategoryName = "Games"
			};
		}


		[Fact]
		public async void CreatedTransactionViewModelAsyncWorkProperlyTest()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("trans");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			IServiceSponsorship sponsorSerivece = new ServiceSponsorship(dbContext, categorySerivece);
			IServiceYoutub youtubeSerivece = new ServiceYoutube(dbContext, categorySerivece);

			var transactionService = new ServiceTransaction(dbContext,youtubeSerivece,sponsorSerivece,categorySerivece);

			dbContext.Add(youtuber);
			dbContext.Add(sponsor);
			dbContext.Add(category);
			await dbContext.SaveChangesAsync();

			var result = transactionService.CreatedTransactionViewModelAsync(1, 1);

			Assert.Equal(0 , result.Result.TotalPrice);
			Assert.Equal("test", result.Result.CompanyName);
			Assert.Equal("Test", result.Result.ChanelName);
		}

		[Fact]

		public async void CreateTransactionAsyncWorkProperly()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("trans1");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			IServiceSponsorship sponsorSerivece = new ServiceSponsorship(dbContext, categorySerivece);
			IServiceYoutub youtubeSerivece = new ServiceYoutube(dbContext, categorySerivece);

			var transactionService = new ServiceTransaction(dbContext, youtubeSerivece, sponsorSerivece, categorySerivece);

			dbContext.Add(youtuber);
			dbContext.Add(sponsor);
			dbContext.Add(category);
			await dbContext.SaveChangesAsync();

			TransactionViewModel model = new TransactionViewModel
			{
				SponsorId = 1,
				CompanyName = "1",
				Product = "beer",
				CompanyBudget = 100,
				QuantityClips = 5,
				ChanelId = 1,
				ChanelName = "test",
				ChanelUrl = "this is url",
				Subscribers = 100,
				PricePerClip = 5,
				SponroshipsClipsNum = 3,
				TotalPrice = 500,
			};

			var result = await transactionService.CreateTransactionAsync(model , "1");

			var numberTransactions = dbContext.Transactions.ToList();

			Assert.Equal(1 , numberTransactions.Count());
		}

		[Fact]
		public async void EditSaveTransactionWorkProprlytest()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("trans2");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			IServiceSponsorship sponsorSerivece = new ServiceSponsorship(dbContext, categorySerivece);
			IServiceYoutub youtubeSerivece = new ServiceYoutube(dbContext, categorySerivece);

			var transactionService = new ServiceTransaction(dbContext, youtubeSerivece, sponsorSerivece, categorySerivece);

			dbContext.Add(youtuber);
			dbContext.Add(sponsor);
			dbContext.Add(category);
			dbContext.Add(transaction);
			await dbContext.SaveChangesAsync();

			

			TransactionViewModel model = new TransactionViewModel
			{
				SponsorId = 1,
				CompanyName = "1",
				Product = "beer",
				CompanyBudget = 100,
				QuantityClips = 5,
				ChanelId = 1,
				ChanelName = "test",
				ChanelUrl = "this is url",
				Subscribers = 100,
				PricePerClip = 5,
				SponroshipsClipsNum = 3,
				TotalPrice = 1500,
				TransactionId = Guid.Parse("0c27bd01-5545-41dc-a3ee-d219ca0a5498")
			};

			dbContext.Entry(transaction).State = EntityState.Detached;
			await transactionService.EditSaveAsync(model, "1");

			var result = dbContext.Transactions
				.Where(x => x.Id == Guid.Parse("0c27bd01-5545-41dc-a3ee-d219ca0a5498"))
				.FirstOrDefault();


			Assert.Equal(1500 , result.TransferMoveney);
		}

		[Fact]
		public async void DeleteNotCompletedTransactionsProperlyWork()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("trans3");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			IServiceSponsorship sponsorSerivece = new ServiceSponsorship(dbContext, categorySerivece);
			IServiceYoutub youtubeSerivece = new ServiceYoutube(dbContext, categorySerivece);

			var transactionService = new ServiceTransaction(dbContext, youtubeSerivece, sponsorSerivece, categorySerivece);

			dbContext.Add(transaction);
			dbContext.Add(transactionNC);
			dbContext.SaveChanges();

			await transactionService.DeleteNotCompletedTransactions();

			var result = dbContext.Transactions.ToList();

			Assert.Equal(1 , result.Count);
		}

		[Fact]
		public void DeleteTransactionTest()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("trans4");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			IServiceSponsorship sponsorSerivece = new ServiceSponsorship(dbContext, categorySerivece);
			IServiceYoutub youtubeSerivece = new ServiceYoutube(dbContext, categorySerivece);

			var transactionService = new ServiceTransaction(dbContext, youtubeSerivece, sponsorSerivece, categorySerivece);

			dbContext.Add(transaction);
			dbContext.Add(transactionNC);
			dbContext.SaveChanges();

			transactionService.DeleteTransactionAsync(Guid.Parse("48baae66-3d61-4e6f-8403-8300dd44c085"));

			var result = dbContext.Transactions.ToList();
			var deleted = dbContext.Transactions
				.Where(x => x.Id == Guid.Parse("48baae66-3d61-4e6f-8403-8300dd44c085"))
				.FirstOrDefault();

			Assert.Equal(1, result.Count);
			Assert.Null(deleted);
		}


		[Fact]
		public async void GetAllUnaceptedTransactionWorkingProperlytest()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("trans5");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			IServiceSponsorship sponsorSerivece = new ServiceSponsorship(dbContext, categorySerivece);
			IServiceYoutub youtubeSerivece = new ServiceYoutube(dbContext, categorySerivece);

			var transactionService = new ServiceTransaction(dbContext, youtubeSerivece, sponsorSerivece, categorySerivece);

			dbContext.Add(transaction);
			dbContext.Add(transactionNC);
			dbContext.SaveChanges();

			var result = await transactionService.GetAllUnaceptedTransaction("1");

			Assert.Equal(2 , result.Count());
		}

		[Fact]
		public async void GetFindModelWorkPriperlyTest()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("trans6");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			IServiceSponsorship sponsorSerivece = new ServiceSponsorship(dbContext, categorySerivece);
			IServiceYoutub youtubeSerivece = new ServiceYoutube(dbContext, categorySerivece);

			var transactionService = new ServiceTransaction(dbContext, youtubeSerivece, sponsorSerivece, categorySerivece);

			dbContext.Add(youtuber);
			dbContext.Add(sponsor);
			dbContext.Add(category);
			await dbContext.SaveChangesAsync();

			var resul = await transactionService.GetFindModelAsync(1);

			Assert.Equal(1 , resul.SponsorshipId);
			Assert.Equal("Games", resul.CategoryName);
		}

		[Fact]
		public void GetTotalPriceCalculateProperly()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("trans7");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			IServiceSponsorship sponsorSerivece = new ServiceSponsorship(dbContext, categorySerivece);
			IServiceYoutub youtubeSerivece = new ServiceYoutube(dbContext, categorySerivece);

			var transactionService = new ServiceTransaction(dbContext, youtubeSerivece, sponsorSerivece, categorySerivece);

			var result = transactionService.GetTotalPrice(1, 5.5m);
			var result1 = transactionService.GetTotalPrice(1, -1);
			var result2 = transactionService.GetTotalPrice(1, 0);

			Assert.Equal(5.5m, result);
			Assert.Equal(-1m, result1);
			Assert.Equal(0m, result2);
		}

		[Fact]
		public async void UpdateCompletedTransactionAsyncWorkPorperly()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("trans8");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			IServiceSponsorship sponsorSerivece = new ServiceSponsorship(dbContext, categorySerivece);
			IServiceYoutub youtubeSerivece = new ServiceYoutube(dbContext, categorySerivece);

			var transactionService = new ServiceTransaction(dbContext, youtubeSerivece, sponsorSerivece, categorySerivece);

			dbContext.Add(youtuber);
			dbContext.Add(sponsor);
			dbContext.Add(category);
			dbContext.Add(transaction);
			await dbContext.SaveChangesAsync();
			
			await transactionService.UpdateCompletedTransactionAsync(transaction, 1, 1);

			var result = dbContext.Transactions
				.Where(x => x.Id == Guid.Parse("0c27bd01-5545-41dc-a3ee-d219ca0a5498"))
				.FirstOrDefault();

			Assert.Equal(1 , result.SponsorshipTransactions.Count);
			Assert.Equal(1 , result.YoutuberTransactions.Count);
		}

		[Fact]
		public async void UpdateTransactionAsyncWorkProperlyTest()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("trans9");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			IServiceSponsorship sponsorSerivece = new ServiceSponsorship(dbContext, categorySerivece);
			IServiceYoutub youtubeSerivece = new ServiceYoutube(dbContext, categorySerivece);

			var transactionService = new ServiceTransaction(dbContext, youtubeSerivece, sponsorSerivece, categorySerivece);

			dbContext.Add(transaction);
			await dbContext.SaveChangesAsync();

			Transaction model = new Transaction
			{
				Id = Guid.Parse("0c27bd01-5545-41dc-a3ee-d219ca0a5498"),
				TransferMoveney = 100,
				QuntityClips = 3,
				AppUserId = "1",
				SuccessfulCreated = true,
				HasAccepted = true,
				IsCompleted = true,
			};

			dbContext.Entry(transaction).State = EntityState.Detached;
			await transactionService.UpdateTransactionAsync(model);

			var result = dbContext.Transactions
				.Where(x => x.Id == Guid.Parse("0c27bd01-5545-41dc-a3ee-d219ca0a5498"))
				.FirstOrDefault();

			Assert.True(result.HasAccepted);
			Assert.True(result.IsCompleted);
		}

		[Fact]
		public async void RemoveMoneyFromSponsorAsyncWorkProperly()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("trans9");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			IServiceSponsorship sponsorSerivece = new ServiceSponsorship(dbContext, categorySerivece);
			IServiceYoutub youtubeSerivece = new ServiceYoutube(dbContext, categorySerivece);

			dbContext.Add(sponsor);
			await dbContext.SaveChangesAsync();

			var transactionService = new ServiceTransaction(dbContext, youtubeSerivece, sponsorSerivece, categorySerivece);

			await transactionService.RemoveMoneyFromSponsorAsync(1, 500);

			var result = dbContext.Sponsorships
				.Where(x => x.Id == 1)
				.FirstOrDefault();

			Assert.Equal(1500, result.Wallet);
			await Assert.ThrowsAsync<InvalidOperationException>(() => transactionService.RemoveMoneyFromSponsorAsync(1, 2000));
		}
	}
}
