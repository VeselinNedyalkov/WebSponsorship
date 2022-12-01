using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.ModelsAccess
{
	public class NotAcceptedTransactionViewModel
	{
		public Guid Id { get; set; }
		public decimal TransferMoveney { get; set; }

		public int QuntityClips { get; set; }

		public int SponsorshipId { get; set; }

		public string YoutubeChanelName { get; set; } = null!;

		public int ChanelSubscribers { get; set; }
	}
}
