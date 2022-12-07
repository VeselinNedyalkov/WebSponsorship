using Microsoft.EntityFrameworkCore;
using SponsorY.Data;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess.Youtube;
using SponsorY.DataAccess.Survices;
using SponsorY.DataAccess.Survices.Contract;
using System.Net.Http.Headers;

namespace SponsorY.Test
{
	public class YoutubeTest
	{
		Category category = null;
		Youtuber youtuber = null;
		Youtuber youtuber1 = null;
		Youtuber youtuber2 = null;
		public YoutubeTest()
		{
			category = new Category
			{
				Id = 1,
				CategoryName = "Games"
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

			youtuber1 = new Youtuber
			{
				Id = 2,
				ChanelName = "Test1",
				Url = "Url",
				Subscribers = 12,
				PricePerClip = 10,
				Wallet = 10,
				CategoryId = 1,
				AppUserId = "1"
			};

			youtuber2 = new Youtuber
			{
				Id = 3,
				ChanelName = "Test1",
				Url = "Url",
				Subscribers = 12,
				PricePerClip = 10,
				Wallet = 0,
				CategoryId = 1,
				AppUserId = "2"
			};

		}

		[Fact]
		public void GetAllYoutubersTest()
		{
			
			//Arrange

			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("test");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);
			dbContext.Categories.Add(category);
			dbContext.Youtubers.Add(youtuber);
			dbContext.Youtubers.Add(youtuber1);
			dbContext.Youtubers.Add(youtuber2);
			dbContext.SaveChanges();


			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			var youtubeService = new ServiceYoutube(dbContext, categorySerivece);

			var result = youtubeService.GetAllYoutubeChanelsAsync("1");
			var result1 = youtubeService.GetAllYoutubeChanelsAsync("2");
			var result2 = youtubeService.GetAllYoutubeChanelsAsync("3");

			Assert.Equal(2 , result.Result.Count());
			Assert.Equal(1, result1.Result.Count());
			Assert.Equal(0, result2.Result.Count());
		}

		[Fact]
		public async void TestAddingYoutuberAsync()
		{

			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("test1");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);
			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			var youtubeService = new ServiceYoutube(dbContext, categorySerivece);

			AddYoutViewModel model = new AddYoutViewModel
			{
				ChanelName = "test",
				Url = "Url",
				Subscribers = 12,
				PricePerClip = 10,
				CategoryId = 1,
			};

			await youtubeService.AddYoutubChanelAsync("1", model);

			var resutl = dbContext.Youtubers.Any(x => x.ChanelName == "test");
			var resutlt = dbContext.Youtubers.Any(x => x.ChanelName == "test123");

			Assert.True(resutl);
			Assert.False(resutlt);
		}

		[Fact]
		public async void TestTakeAllYoutbersWithIdNum()
		{

			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("test2");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);
			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			var youtubeService = new ServiceYoutube(dbContext, categorySerivece);

			dbContext.Categories.Add(category);
			dbContext.Youtubers.Add(youtuber);
			dbContext.SaveChanges();

			var result = await youtubeService.TakeYoutuberAsync(1);


			Assert.Equal("Games", result.Category);
			Assert.Equal(1, result.Id);
			Assert.Equal(12, result.Subscribers);
		}

		[Fact]
		public async void TestToFaindChanelWithIdCategory()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("test3");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);
			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			var youtubeService = new ServiceYoutube(dbContext, categorySerivece);

			dbContext.Categories.Add(category);
			dbContext.Youtubers.Add(youtuber);
			dbContext.Youtubers.Add(youtuber1);
			dbContext.SaveChanges();

			var result = await youtubeService.GetChanelWithCategoryAsync(1);
			var result1 = await youtubeService.GetChanelWithCategoryAsync(2);


			Assert.Equal(2, result.Count());
			Assert.Equal(0, result1.Count());
		}

		[Fact]
		public async void TestGetAllMoneyFormWallet()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
		.UseInMemoryDatabase("test4");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);
			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			var youtubeService = new ServiceYoutube(dbContext, categorySerivece);

			AppUser user = new AppUser
			{
				Id = "1",
				UserName = "test",
				Email = "test",
				Wallet = 15,
			};

			dbContext.Users.Add(user);
			dbContext.SaveChanges();

			var result = await youtubeService.GetAllFinancesaAsync("1");

			Assert.Equal(15, result.Wallet);
		}

		[Fact]
		public void TestReduceMoneyFromUserWhenWitdraw()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
		.UseInMemoryDatabase("test5");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);
			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			var youtubeService = new ServiceYoutube(dbContext, categorySerivece);

			AppUser user = new AppUser
			{
				Id = "1",
				UserName = "test",
				Email = "test",
				Wallet = 2000,
			};

			AppUser user1 = new AppUser
			{
				Id = "2",
				UserName = "test",
				Email = "test",
				Wallet = 2000,
			};

			dbContext.Users.Add(user);
			dbContext.SaveChanges();

			YoutubeFinancesViewModel model = new YoutubeFinancesViewModel
			{
				Wallet = 2000
			};
			YoutubeFinancesViewModel model1 = new YoutubeFinancesViewModel
			{
				Wallet = 3000
			};

			youtubeService.WithdrawMOneyAsync(user.Id, model);

			var userMoney = dbContext.Users.Where(x => x.Id == user.Id).Select(x => x.Wallet).FirstOrDefault();

			Assert.Equal(0, userMoney);
			Assert.ThrowsAsync<InvalidOperationException>(() => youtubeService.WithdrawMOneyAsync(user1.Id, model1));
		}
	}
}