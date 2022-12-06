using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.Models
{
	public class YoutuberTransaction
	{
		[ForeignKey(nameof(Youtuber))]
		public int YoutuberId { get; set; }
		public Youtuber Youtuber { get; set; } = null!;

		[ForeignKey(nameof(Transaction))]

		public Guid TransactionId { get; set; }
		public Transaction Transaction { get; set; } = null!;
	}
}
