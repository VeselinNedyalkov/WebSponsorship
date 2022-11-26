using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.ModelsAccess
{
	public class YoutuberAwaitTransactionViewModel
	{
		public decimal MoneyOffer { get; set; }
		public int QuntityClips { get; set; }
		public int TransactionId { get; set; }
		public string CompanyName { get; set; }
		public string Product { get; set; }

		public string? ProductUrl { get; set; }
	}
}
