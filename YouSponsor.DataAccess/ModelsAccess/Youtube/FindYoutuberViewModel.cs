using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.ModelsAccess.Youtube
{
    public class FindYoutuberViewModel
    {
        public int Id { get; set; }

        public string ChanelName { get; set; } = null!;

        public string Url { get; set; } = null!;

        public int Subscribers { get; set; }

        public decimal PricePerClip { get; set; }
    }
}
