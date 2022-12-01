
using SponsorY.DataAccess.Models;
using System.ComponentModel.DataAnnotations;
using static SponsorY.Utility.DataConstant.YoutuberConstants;

namespace SponsorY.DataAccess.ModelsAccess
{
    public class YouTubeViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(ChanelNameMaxLenght)]
        public string ChanelName { get; set; } = null!;

		[Required]
        public string Url { get; set; } = null!;

		[Required]
        public int Subscribers { get; set; }

        [Required]
        public decimal PricePerClip { get; set; }

        public decimal Wallet { get; set; }

        public IEnumerable<Category> Categories { get; set; } = new List<Category>();

        public string Category { get; set; } = null!;

		public Guid? TransferId { get; set; }

        public int CategoryId { get; set; }

        public string AppUserId { get; set; } = null!;
	}
}
