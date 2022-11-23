using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.ModelsAccess
{
	public class TransactionViewModel
	{
		public int SponsorId { get; set; }
		public string CompanyName { get; set; }

		public string Product { get; set; }

		public string? CompanyUrl { get; set; }

		public decimal CompanyBudget { get; set; }


		public int ChanelId { get; set; }
		public string ChanelName { get; set; }

		public string ChanelUrl { get; set; }

		public int Subscribers { get; set; }

		public decimal PricePerClip { get; set; }

		public int SponroshipsClipsNum { get; set; }
	}
}
