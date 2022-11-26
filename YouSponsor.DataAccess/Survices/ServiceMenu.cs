using Microsoft.EntityFrameworkCore;
using SponsorY.Data;
using SponsorY.DataAccess.ModelsAccess;
using SponsorY.DataAccess.Survices.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.Survices
{
	public class ServiceMenu : IServiceMenu
	{
		private readonly ApplicationDbContext context;

		public ServiceMenu(ApplicationDbContext _context)
		{
			context = _context;
		}

		public async Task<UserInfoViewModel> GetUserInfo(string userId)
		{

			UserInfoViewModel model = await context.UsersInfo
				.Where(x => x.AppUserId == userId)
				.Select(x => new UserInfoViewModel
				{
					FirstName = x.FirstName,
					LastName = x.LastName,
					Age = x.Age,
					Country= x.Country,
				})
				.FirstOrDefaultAsync();

			return model;
		}
	}
}
