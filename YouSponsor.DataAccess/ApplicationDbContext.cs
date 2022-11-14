
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

        public DbSet<Category> Categories { get; set; }
        public DbSet<Sponsorship> Sponsorships { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<UserInfo> UsersInfo { get; set; }
        public DbSet<Youtuber> Youtubers { get; set; }

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

            builder.Entity<Youtuber>()
                .HasOne(e => e.Transfer)
                .WithOne(c => c.Youtuber)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}