using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SponsorY.DataAccess.Models;

namespace SponsorY.DataAccess.SeedConfiguration
{
	internal class RoleConfiguration : IEntityTypeConfiguration<Role>
	{
		public void Configure(EntityTypeBuilder<Role> builder)
		{
			builder.HasData(CreateUserRole());
		}

		private List<Role> CreateUserRole()
		{
			var comments = new List<Role>()
			{
				new Role()
				{
					Id = Guid.Parse("b139508b-15e9-4c2c-9424-b46c2cf71e10"),
					Name = "admin",
					NormalizedName = "ADMIN",
				},
				new Role()
				{
					Id = Guid.Parse("c2596b34-7e0e-4c6d-b546-662d667e180b"),
					Name = "youtuber",
					NormalizedName = "YOUTUBER",
				},
				new Role()
				{
					Id = Guid.Parse("4444ec54-3fd1-4920-88f7-ba1c4d0b0b68"),
					Name = "sponsor",
					NormalizedName = "SPONSOR",
				},
			};
			return comments;
		}
	}
}
