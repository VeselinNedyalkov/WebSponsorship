using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SponsorY.Data;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess.Categories;
using SponsorY.DataAccess.Survices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.Test
{
	public class CategoryTest
	{
		Category category = null;
		Category category1 = null;
		public CategoryTest()
		{
			category = new Category
			{
				Id = 1,
				CategoryName = "Test",
			};

			category1 = new Category
			{
				Id = 2,
				CategoryName = "Test2",
			};
		}

		[Fact]
		public async void TestAddCategoryWorkProperly()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase("testCat");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			var serviceCategory = new ServiceCategory(dbContext);

			CategoryViewModel model = new CategoryViewModel
			{
				CategoryName = "Games"
			};

			await serviceCategory.AddCategoryAync(model);

			var result = dbContext.Categories.Where(x => x.CategoryName == model.CategoryName).FirstOrDefault();

			Assert.Equal("Games", result.CategoryName);
		}

		[Fact]
		public async void TestDeleteCategoryWorkingProperly()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
							.UseInMemoryDatabase("testCat1");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			var serviceCategory = new ServiceCategory(dbContext);

			dbContext.Add(category);
			dbContext.Add(category1);
			dbContext.SaveChanges();

			var result = dbContext.Categories.ToList();

			Assert.Equal(2, result.Count);

			await serviceCategory.DeleteCategoryAsync(1);

			result = dbContext.Categories.ToList();

			Assert.Equal(1, result.Count);
		}

		[Fact]
		public async void TestGetAllCategoriesWorkProperly()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
							.UseInMemoryDatabase("testCat2");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			var serviceCategory = new ServiceCategory(dbContext);

			dbContext.Add(category);
			dbContext.Add(category1);
			dbContext.SaveChanges();

			var result = await serviceCategory.GetAllCategoryAsync();

			Assert.Equal(2, result.Count());
		}

		[Fact]
		public async void TestGetCategoryByName()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
							.UseInMemoryDatabase("testCat3");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			var serviceCategory = new ServiceCategory(dbContext);

			dbContext.Add(category);
			dbContext.Add(category1);
			dbContext.SaveChanges();

			var result = await serviceCategory.GetCategoryNameAsync(1);

			Assert.Equal("Test", result);
		}

		[Fact]
		public async void TestGetIdByCategoryName()
		{
			var opitionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
							.UseInMemoryDatabase("testCat4");
			var dbContext = new ApplicationDbContext(opitionBuilder.Options);

			var serviceCategory = new ServiceCategory(dbContext);

			dbContext.Add(category);
			dbContext.Add(category1);
			dbContext.SaveChanges();

			var result = await serviceCategory.GetIdByNameAsync("Test");

			Assert.Equal(1, result);
		}
	}
}
