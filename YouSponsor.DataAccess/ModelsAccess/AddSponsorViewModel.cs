using static SponsorY.Utility.DataConstant.SponsorshipConstants;

using System.ComponentModel.DataAnnotations;

namespace SponsorY.DataAccess.ModelsAccess
{
    public class AddSponsorViewModel
    {
        [Required]
        [StringLength(CompanyNameMaxLenght)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(ProductMaxLenght)]
        public string Product { get; set; }

        public string? Url { get; set; }


        [Range(typeof(decimal), "0.0", "79228162514264337593543950335", ConvertValueInInvariantCulture = true)]
        public decimal Wallet { get; set; } = 0;
    }
}
