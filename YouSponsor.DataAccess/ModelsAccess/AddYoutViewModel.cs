using static SponsorY.Utility.DataConstant.YoutuberConstants;
using System.ComponentModel.DataAnnotations;
using SponsorY.DataAccess.Models;

namespace SponsorY.DataAccess.ModelsAccess
{
    public class AddYoutViewModel
    {
        [Required]
        [StringLength(ChanelNameMaxLenght)]
        public string ChanelName { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public int Subscribers { get; set; }

        [Required]
        public decimal PricePerClip { get; set; }

        public IEnumerable<Category> Categories { get; set; } = new List<Category>();

        public int CategoryId { get; set; }

    }
}
