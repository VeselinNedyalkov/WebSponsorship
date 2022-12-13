using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SponsorY.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.SeedConfiguration
{
	internal class UsersConfiguration : IEntityTypeConfiguration<AppUser>
	{
		public void Configure(EntityTypeBuilder<AppUser> builder)
		{
			builder.HasData(CreateUser());

		}

		PasswordHasher<AppUser> hasher = new PasswordHasher<AppUser>();
		private List<AppUser> CreateUser()
		{
			var users = new List<AppUser>()
			{
				new AppUser()
				{
					UserName = "AdminAccount",
					Email = "admin@abv.bg",
					IsDeleted= false,
				}
			};

			foreach (var user in users)
			{
				user.PasswordHash = hasher.HashPassword(user, "123456");
			}

			return users;
		}


	}
}
