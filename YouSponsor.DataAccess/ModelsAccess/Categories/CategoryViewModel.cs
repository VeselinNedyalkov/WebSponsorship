using static SponsorY.Utility.DataConstant.CategoryConstants;
using System.ComponentModel.DataAnnotations;

namespace SponsorY.DataAccess.ModelsAccess.Categories
{
    public class CategoryViewModel
    {
        [Required]
        [StringLength(CategoryNameMaxLength)]
        public string CategoryName { get; set; } = null!;
    }
}
