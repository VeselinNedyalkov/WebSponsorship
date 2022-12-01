using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.Models
{
	public class SponsorshipTransaction
	{
		[ForeignKey(nameof(Sponsorship))]
		public int SponsorId { get; set; }
		public Sponsorship Sponsorship { get; set; }

		[ForeignKey(nameof(Transaction))]

		public Guid TransactionId { get; set; }
		public Transaction Transaction { get; set; }
	}
}
