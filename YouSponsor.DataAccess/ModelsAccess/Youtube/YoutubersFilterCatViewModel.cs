using SponsorY.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.ModelsAccess.Youtube
{
    public class YoutubersFilterCatViewModel
    {
        public int Id { get; set; }

        [Required]
        public string ChanelName { get; set; } = null!;

        [Required]
        public string Url { get; set; } = null!;

        [Required]
        public int Subscribers { get; set; }

        [Required]
        public decimal PricePerClip { get; set; }

        public decimal Wallet { get; set; }

        public int? TransferId { get; set; }

        public int CategoryId { get; set; }

        public string AppUserId { get; set; } = null!;
    }
}
