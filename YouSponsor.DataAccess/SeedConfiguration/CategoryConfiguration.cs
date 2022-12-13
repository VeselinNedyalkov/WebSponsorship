using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SponsorY.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.SeedConfiguration
{
	internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.HasData(CreateCategories());
		}

		private List<Category> CreateCategories()
		{
			List<Category> categories = new List<Category>
			{
				new Category
				{
					Id = 1,
					CategoryName = "Gaming"
				},
				new Category
				{
					Id = 2,
					CategoryName = "Traveling"
				},
				new Category
				{
					Id = 3,
					CategoryName = "Sport"
				},
				new Category
				{
					Id = 4,
					CategoryName = "History"
				},
			};

			return categories;
		}
	}
}
