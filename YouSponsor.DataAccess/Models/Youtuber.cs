using static SponsorY.Utility.DataConstant.YoutuberConstants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SponsorY.DataAccess.Models
{
    public class Youtuber
    {
        [Key]
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

		public virtual ICollection<YoutuberTransaction> YoutuberTransactions { get; set; } = new List<YoutuberTransaction>();

		[ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;


        [ForeignKey(nameof(AppUser))]
        public string AppUserId { get; set; } = null!;
		public AppUser AppUser { get; set; } = null!;
	}
}
