using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess.Youtube;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.ModelsAccess.Sponsor
{
    public class FindChanelViewModel
    {
        public int SponsorshipId { get; set; }

        public IEnumerable<Category>? Categories { get; set; } = new List<Category>();

        public IEnumerable<YoutubersFilterCatViewModel>? Youtubers { get; set; } = new List<YoutubersFilterCatViewModel>();

        public string CategoryName { get; set; } = null!;

        public int? Sorting { get; set; }
    }
}
