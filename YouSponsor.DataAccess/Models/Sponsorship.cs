using static SponsorY.Utility.DataConstant.SponsorshipConstants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SponsorY.DataAccess.Models
{
    public class Sponsorship
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(CompanyNameMaxLenght)]
        public string CompanyName { get; set; } = null!;

		[Required]
        [StringLength(ProductMaxLenght)]
        public string Product { get; set; } = null!;

		public string? Url { get; set; }


        [Range(typeof(decimal), "0.0", "79228162514264337593543950335", ConvertValueInInvariantCulture = true)]
        public decimal Wallet { get; set; } = 0;

		[ForeignKey(nameof(Transaction))]
		public int? TransactionId { get; set; }
		public Transaction? Transaction { get; set; }

		public int CategoryId { get; set; }

        [ForeignKey(nameof(AppUser))]
        public string AppUserId { get; set; } = null!;
		public AppUser AppUser { get; set; } = null!;
	}
}
