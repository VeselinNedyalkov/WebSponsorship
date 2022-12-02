using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.ModelsAccess
{
	public class SponsorHistoryViewModel
	{
		public string Product { get; set; } = null!;

		public string? CompanyUrl { get; set; } = null!;

		public string ChanelName { get; set; } = null!;

		public string ChanelUrl { get; set; } = null!;

		public int Subscribers { get; set; }

		public decimal PricePerClip { get; set; }

		public int SponroshipsClipsNum { get; set; }

		public decimal TotalPrice { get; set; }
	}
}
