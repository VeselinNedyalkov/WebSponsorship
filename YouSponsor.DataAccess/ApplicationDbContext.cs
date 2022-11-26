
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SponsorY.DataAccess.Models;
using static SponsorY.Utility.DataConstant.RegisterConstant;

namespace SponsorY.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Sponsorship> Sponsorships { get; set; } = null!;
		public DbSet<Transaction> Transactions { get; set; } = null!;
		public DbSet<UserInfo> UsersInfo { get; set; } = null!;
        public DbSet<Youtuber> Youtubers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            


            builder.Entity<AppUser>()
               .Property(x => x.UserName)
               .HasMaxLength(UserNameMinLength)
               .IsRequired();

            builder.Entity<AppUser>()
                .Property(x => x.Email)
                .HasMaxLength(EmailMaxlength)
                .IsRequired();

            builder.Entity<Transaction>()
                .HasOne(e => e.Youtuber)
                .WithOne(e => e.Transaction)
                .OnDelete(DeleteBehavior.Restrict);

			builder.Entity<Transaction>()
				.HasOne(e => e.Sponsorship)
				.WithOne(e => e.Transaction)
				.OnDelete(DeleteBehavior.Restrict);

			base.OnModelCreating(builder);
        }
    }
}