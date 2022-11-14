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
        public string ChanelName { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public int Subscribers { get; set; }

        [Required]
        public decimal PricePerClip { get; set; }

        public decimal Wallet { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [ForeignKey(nameof(Transfer))]
        public int? TransferId { get; set; }
        public Transfer? Transfer { get; set; }

        [ForeignKey(nameof(AppUser))]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
