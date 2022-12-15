using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.SeedConfiguration
{
	internal class UserRolesConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
	{
		public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
		{
			builder.HasData(CreateUserRoles());
		}

		private IdentityUserRole<string> CreateUserRoles()
		{
			var user = new IdentityUserRole<string>()
			{
				RoleId = "b139508b-15e9-4c2c-9424-b46c2cf71e10",
				UserId = "4edef44e-3985-42c3-9e03-7a39d9cab63b"
			};
			return user;
		}
	}
}
