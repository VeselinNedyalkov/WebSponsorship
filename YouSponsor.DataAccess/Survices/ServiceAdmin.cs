using Microsoft.EntityFrameworkCore;
using SponsorY.Data;
using SponsorY.DataAccess.Survices.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.Survices
{
	public class ServiceAdmin : IServiceAdmin
	{
		private readonly ApplicationDbContext context;

		public ServiceAdmin(ApplicationDbContext _context)
		{
			context = _context;
		}
		public async Task<int> GetAllSponsorsAsync()
		{
			var count = await context.Sponsorships.ToListAsync();
			return count.Count();
		}

		public async Task<int> GetAllYoutubeChanelsAsync()
		{
			var count = await context.Youtubers.ToListAsync();
			return count.Count();
		}
	}
}
