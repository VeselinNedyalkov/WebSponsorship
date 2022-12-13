using Microsoft.EntityFrameworkCore;
using SponsorY.Data;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess.Youtube;
using SponsorY.DataAccess.Survices;
using SponsorY.DataAccess.Survices.Contract;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace SponsorY.Test
{
	public class YoutubeTest
	{
		Category category = null;
		Youtuber youtuber = null;
		Youtuber youtuber1 = null;
		Youtuber youtuber2 = null;
		Sponsorship sponsor = null;
		private Transaction transaction = null;
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

			sponsor = new Sponsorship
			{
				Id = 1,
				AppUserId = "1",
				CategoryId = 1,
				CompanyName = "Sponsor1",
				Product = "Product",
				Url = "This is URL",
				Wallet = 50,
			};

			transaction = new Transaction
			{
				Id = Guid.Parse("c9df9b73-99be-43c1-82aa-d905f1aeb51c"),
				TransferMoveney = 50,
				QuntityClips = 5,
				AppUserId = "1"
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
				Value = 2000
			};
			YoutubeFinancesViewModel model1 = new YoutubeFinancesViewModel
			{
				Value = 3000
			};

			youtubeService.WithdrawMOneyAsync(user.Id, model);

			var userMoney = dbContext.Users.Where(x => x.Id == user.Id).Select(x => x.Wallet).FirstOrDefault();

			Assert.Equal(0, userMoney);
			Assert.ThrowsAsync<InvalidOperationException>(() => youtubeService.WithdrawMOneyAsync(user1.Id, model1));
		}

		[Fact]
		public void GetAllYoutubeChanelsFindYoutuberViewModelTest()
		{

			//Arrange

			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("test6");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);
			dbContext.Categories.Add(category);
			dbContext.Youtubers.Add(youtuber);
			dbContext.Youtubers.Add(youtuber1);
			dbContext.Youtubers.Add(youtuber2);
			dbContext.SaveChanges();


			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			var youtubeService = new ServiceYoutube(dbContext, categorySerivece);

			var result = youtubeService.FindAllYoutubersAsync();

			Assert.Equal(3, result.Result.Count());
		}



		[Fact]
		public void DeleteYoutuberWorkProperlyTest()
		{

			//Arrange

			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("test7");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);
			dbContext.Categories.Add(category);
			dbContext.Youtubers.Add(youtuber);
			dbContext.Youtubers.Add(youtuber1);
			dbContext.Youtubers.Add(youtuber2);
			dbContext.SaveChanges();


			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			var youtubeService = new ServiceYoutube(dbContext, categorySerivece);

			youtubeService.DeleteYoutuber(1);

			var result = dbContext.Youtubers.ToList();
			var deleted = dbContext.Youtubers
				.Where(x => x.Id == 1).FirstOrDefault();

			Assert.Equal(2, result.Count());
			Assert.Equal(null, deleted);
		}

		[Fact]
		public async void EditYoutuberAsyncWorkingProperlyAsyncTest()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("test9");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);
			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			var youtubeService = new ServiceYoutube(dbContext, categorySerivece);

			dbContext.Categories.Add(category);
			dbContext.Youtubers.Add(youtuber);
			dbContext.SaveChanges();

			YouTubeViewModel model = new YouTubeViewModel
			{
				ChanelName = "change",
				Url = "new",
				Subscribers = 1,
				PricePerClip = 15
			};

			dbContext.Entry<Youtuber>(youtuber).State = EntityState.Detached;
			await youtubeService.EditYoutuberAsync(1, model);

			var result = dbContext.Youtubers
				.Where(x => x.Id == 1)
				.FirstOrDefault();

			Assert.Equal("change", result.ChanelName);
			Assert.Equal("new", result.Url);
			Assert.Equal(1 , result.Subscribers);
			Assert.Equal(15 , result.PricePerClip);
		}
	}
}