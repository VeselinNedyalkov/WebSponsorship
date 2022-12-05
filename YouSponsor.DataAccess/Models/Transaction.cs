
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SponsorY.DataAccess.Models
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Range(typeof(decimal), "0.0", "79228162514264337593543950335M", ConvertValueInInvariantCulture = true)]
        public decimal TransferMoveney { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int QuntityClips { get; set; }


		public virtual ICollection<SponsorshipTransaction> SponsorshipTransactions { get; set; } = new List<SponsorshipTransaction>();


		public virtual ICollection<YoutuberTransaction> YoutuberTransactions { get; set; } = new List<YoutuberTransaction>();


		public string? AppUserId { get; set; } 
        public bool SuccessfulCreated { get; set; }

        public bool HasAccepted { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}
