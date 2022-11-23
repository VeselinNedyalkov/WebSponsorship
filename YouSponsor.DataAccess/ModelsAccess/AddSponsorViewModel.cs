using static SponsorY.Utility.DataConstant.SponsorshipConstants;

using System.ComponentModel.DataAnnotations;
using SponsorY.DataAccess.Models;

namespace SponsorY.DataAccess.ModelsAccess
{
    public class AddSponsorViewModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(CompanyNameMaxLenght)]
        public string CompanyName { get; set; } = null!;

		[Required]
        [StringLength(ProductMaxLenght)]
        public string Product { get; set; } = null!;

		public string? Url { get; set; } 


        [Range(typeof(decimal), "0.0", "79228162514264337593543950335", ConvertValueInInvariantCulture = true)]
        public decimal Wallet { get; set; } = 0;

        public IEnumerable<Category> Categories { get; set; } = new List<Category>();

        public int CategoryId { get; set; }
	}
}
