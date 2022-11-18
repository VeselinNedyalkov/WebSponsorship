﻿using static SponsorY.Utility.DataConstant.SponsorshipConstants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SponsorY.DataAccess.Models
{
    public class Sponsorship
    {
        [Key]
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


        public ICollection<Transaction> Transfers { get; set; } = new List<Transaction>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();

        public int CategoryId { get; set; }

        [ForeignKey(nameof(AppUser))]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
