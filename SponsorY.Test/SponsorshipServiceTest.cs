using Microsoft.EntityFrameworkCore;
using SponsorY.Data;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess.Sponsor;
using SponsorY.DataAccess.Survices;
using SponsorY.DataAccess.Survices.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SponsorY.Test
{
	public class SponsorshipServiceTest
	{
		Sponsorship sponsor = null;
		Sponsorship sponsor1 = null;
		Category category = null;
		AppUser user = null;
		public SponsorshipServiceTest()
		{
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

			sponsor1 = new Sponsorship
			{
				Id = 2,
				CompanyName = "test1",
				Product = "vodka",
				Url = "More vodka",
				Wallet = 1000,
				CategoryId = 1,
				AppUserId = "1",
			};

			category = new Category
			{
				Id = 1,
				CategoryName = "test",
			};

			user = new AppUser
			{
				Id = "1",
				UserName = "test",
				Email = "test",
				Wallet = 2000,
			};
		}

		[Fact]
		public async void TestingAddMoneyToSponsor()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("test");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			var serviceSporship = new ServiceSponsorship(dbContext, categorySerivece);

			dbContext.Sponsorships.Add(sponsor);
			dbContext.SaveChanges();

			SponsorViewModel model = new SponsorViewModel
			{
				Id = 1,
				CompanyName = "test",
				Product = "test",
				ValueMoney = 500,
			};

			await serviceSporship.AddMoneyToSponsorAsync(1, model);

			Assert.Equal(2500, sponsor.Wallet);
		}

		

		[Fact]
		public async void TestingAddingSponsorToDb()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("test1");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			var serviceSporship = new ServiceSponsorship(dbContext, categorySerivece);

			AddSponsorViewModel model = new AddSponsorViewModel
			{
				Id = 1,
				CompanyName = "test",
				Product = "test",
				Url = "test",
				Wallet = 0,
				CategoryId = 1
			};

			await serviceSporship.AddSponsorshipAsync(user.Id , model);

			var sponsor = dbContext.Sponsorships.SingleOrDefault(x => x.Id == 1);

			Assert.Equal("test", sponsor.CompanyName);
			Assert.Equal(0 , sponsor.Wallet);
		}

		[Fact]
		public async void TestingDeletingSponsorship()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("test2");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			var serviceSporship = new ServiceSponsorship(dbContext, categorySerivece);

			dbContext.Add(sponsor);
			dbContext.SaveChanges();

			var result = dbContext.Sponsorships.ToList();

			Assert.Equal(1, result.Count);

			await serviceSporship.DeleteSponsorshipOfferAsync(1);

			result = dbContext.Sponsorships.ToList();

			Assert.Equal(0 , result.Count);
		}

		[Fact]
		public async void TestGetAllSponsors()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("test3");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			var serviceSporship = new ServiceSponsorship(dbContext, categorySerivece);

			dbContext.Add(sponsor);
			dbContext.Add(sponsor1);
			dbContext.SaveChanges();

			var result = await serviceSporship.GetAllSponsorshipsAsync("1");

			Assert.Equal(2 , result.Count());
		}

		[Fact]
		public async void TestToGetOneSponsor()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("test4");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			var serviceSporship = new ServiceSponsorship(dbContext, categorySerivece);

			dbContext.Add(sponsor);
			dbContext.Add(sponsor1);
			dbContext.SaveChanges();

			var result = await serviceSporship.GetSingelSponsorAsync(1);

			Assert.Equal(1, result.Id);
			Assert.Equal("test", result.CompanyName);
		}

		[Fact]
		public async void TestWithdrowMoneyFromSponsor()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("test5");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			var serviceSporship = new ServiceSponsorship(dbContext, categorySerivece);


			dbContext.Add(sponsor);
			dbContext.SaveChanges();

			SponsorViewModel model = new SponsorViewModel
			{
				Id = 1,
				ValueMoney = 500,
			};

			await serviceSporship.RemoveMoneyFromSponsorAsync(1 , model);

			Assert.Equal(1500, sponsor.Wallet);
		}

		[Fact]
		public async void EditSponsorshipAsyncWorkingProperlyTest()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("test6");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			IServiceCategory categorySerivece = new ServiceCategory(dbContext);
			var serviceSporship = new ServiceSponsorship(dbContext, categorySerivece);

			dbContext.Sponsorships.Add(sponsor);
			dbContext.SaveChanges();

			SponsorViewModel model = new SponsorViewModel
			{
				CompanyName = "new",
				Product = "updated",
				Url = "new url",
				Wallet = 111,
				CategoryId = 5
			};

			dbContext.Entry<Sponsorship>(sponsor).State = EntityState.Detached;
			await serviceSporship.EditSponsorshipAsync(1, model);

			var result = dbContext.Sponsorships
				.Where(x => x.Id == 1)
				.FirstOrDefault();

			Assert.Equal("new" , result.CompanyName);
			Assert.Equal("updated", result.Product);
			Assert.Equal("new url", result.Url);
			Assert.Equal(111 , result.Wallet);
			Assert.Equal(5 , result.CategoryId);
		}
	}
}
