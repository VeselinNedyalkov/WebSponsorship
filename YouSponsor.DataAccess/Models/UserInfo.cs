using static SponsorY.Utility.DataConstant.UserInfoConstants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SponsorY.DataAccess.Models
{
    public class UserInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        public string? FirstName { get; set; } = null!;

		[StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        public string? LastName { get; set; } = null!;

		[Range(AgeMin, AgeMax)]
        public int? Age { get; set; }

      
        [StringLength(CountryMaxLength, MinimumLength = CountryMinLength)]
        public string? Country { get; set; }

        [ForeignKey(nameof(AppUser))]
        public string AppUserId { get; set; } = null!;
		public AppUser AppUser { get; set; } = null!;
	}

}
