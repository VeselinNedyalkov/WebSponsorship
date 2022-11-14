using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SponsorY.DataAccess.Models
{
    public class AppUser : IdentityUser
    {
        public bool IsDeleted { get; set; } = false;

        public decimal Wallet { get; set; }

        public List<Youtuber> Youtubers { get; set; } = new List<Youtuber>();
        public List<Sponsorship> Sponsorships { get; set; } = new List<Sponsorship>();
    }
}
