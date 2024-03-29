﻿using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.ModelsAccess.Transaction
{
    public class TransactionViewModel
    {
        public int SponsorId { get; set; }
        public string CompanyName { get; set; } = null!;

        public string Product { get; set; } = null!;

        public string? CompanyUrl { get; set; } = null!;

        public decimal CompanyBudget { get; set; }

        public int QuantityClips { get; set; }


        public int ChanelId { get; set; }
        public string ChanelName { get; set; } = null!;

        public string ChanelUrl { get; set; } = null!;

        public int Subscribers { get; set; }

        public decimal PricePerClip { get; set; }

        public int SponroshipsClipsNum { get; set; }

        public decimal TotalPrice { get; set; }

        public Guid? TransactionId { get; set; }
    }
}
