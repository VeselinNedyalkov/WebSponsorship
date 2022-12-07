using Microsoft.EntityFrameworkCore;
using SponsorY.Data;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.Survices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.Test
{
	public class Admintests
	{
		[Fact]
		public async void TestGetAllSponsors()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("testAdmin");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			var serviceAdmin = new ServiceAdmin(dbContext);

			var sponsor = new Sponsorship
			{
				Id = 1,
				CompanyName = "test",
				Product = "beer",
				Url = "More beer",
				Wallet = 2000,
				CategoryId = 1,
				AppUserId = "1",
			};
			var sponsor1 = new Sponsorship
			{
				Id = 2,
				CompanyName = "test1",
				Product = "vodka",
				Url = "More vodka",
				Wallet = 1000,
				CategoryId = 1,
				AppUserId = "1",
			};

			dbContext.Sponsorships.Add(sponsor);
			dbContext.Sponsorships.Add(sponsor1);
			dbContext.SaveChanges();

			var result = await serviceAdmin.GetAllSponsorsAsync();

			Assert.Equal(2, result);
		}

		[Fact]
		public async void TestGetAllYputubers()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("testAdmin1");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			var serviceAdmin = new ServiceAdmin(dbContext);

			var youtuber = new Youtuber
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
			var youtuber1 = new Youtuber
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
			var youtuber2 = new Youtuber
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

			dbContext.Youtubers.Add(youtuber);
			dbContext.Youtubers.Add(youtuber1);
			dbContext.Youtubers.Add(youtuber2);
			dbContext.SaveChanges();

			var result = await serviceAdmin.GetAllYoutubeChanelsAsync();

			Assert.Equal(3, result);
		}
	}
}
