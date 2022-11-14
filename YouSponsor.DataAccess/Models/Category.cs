using static SponsorY.Utility.DataConstant.CategoryConstants;
using System.ComponentModel.DataAnnotations;

namespace SponsorY.DataAccess.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(CategoryNameMaxLength)]
        public string CategoryName { get; set; } = null!;
    }
}
