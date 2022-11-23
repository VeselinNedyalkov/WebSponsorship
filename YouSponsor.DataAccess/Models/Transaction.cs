
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SponsorY.DataAccess.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(typeof(decimal), "0.0", "79228162514264337593543950335M", ConvertValueInInvariantCulture = true)]
        public decimal TransferMoveney { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int QuntityClips { get; set; }

       
        public int SponsorshipId { get; set; }
        [ForeignKey(nameof(SponsorshipId))]
        public virtual Sponsorship Sponsorship { get; set; } = null!;

		public int YoutuberId { get; set; }
        [ForeignKey(nameof(YoutuberId))]
        public Youtuber Youtuber { get; set; } = null!;

		public string AllUserSponsor { get; set; } = null!;

        public bool SubmiteToYoutuber { get; set; }

        public bool HasAccepted { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}
