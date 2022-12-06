using static SponsorY.Utility.DataConstant.RegisterConstant;
using static SponsorY.Utility.DataConstant.RegisterError;
using System.ComponentModel.DataAnnotations;


namespace SponsorY.DataAccess.ModelsAccess.Settings
{
	public class ChangePassViewModel
	{
        [DataType(DataType.Password)]
        public string? Password { get; set; }

		[Required]
		[StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength)]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

        [Compare(nameof(NewPassword))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;

        public bool IsPassCorrect { get; set; } = false;
	}
}
