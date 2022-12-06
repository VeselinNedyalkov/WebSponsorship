using SponsorY.DataAccess.Models;

namespace SponsorY.DataAccess.ModelsAccess.Categories
{
    public class DelCategoryViewModel
    {

        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }
}
