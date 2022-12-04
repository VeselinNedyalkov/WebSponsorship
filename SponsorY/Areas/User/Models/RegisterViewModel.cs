using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using static SponsorY.Utility.DataConstant.RegisterConstant;
using static SponsorY.Utility.DataConstant.RegisterError;

namespace SponsorY.Areas.User.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(UserNameMaxLength, MinimumLength = UserNameMinLength, ErrorMessage = UserNameError)]
        [DisplayName("User Name")]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(EmailMaxlength, MinimumLength = EmailMinlength, ErrorMessage = EmailNameError)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;

        public int Role { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
