﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.ModelsAccess.Youtube
{
    public class YoutuberAwaitTransactionViewModel
    {
        public decimal MoneyOffer { get; set; }
        public int QuntityClips { get; set; }
        public Guid TransactionId { get; set; }
        public string CompanyName { get; set; } = null!;
		public string Product { get; set; } = null!;

		public string? ProductUrl { get; set; }
    }
}
