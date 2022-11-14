using SponsorY.DataAccess.Models;
using static SponsorY.Utility.DataConstant.SponsorshipConstants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.ModelsAccess
{
    public class SponsorViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(CompanyNameMaxLenght)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(ProductMaxLenght)]
        public string Product { get; set; }

        public string? Url { get; set; }


        [Range(typeof(decimal), "0.0", "79228162514264337593543950335", ConvertValueInInvariantCulture = true)]
        public decimal Wallet { get; set; } = 0;

        public string AppUserId { get; set; }

    }
}
