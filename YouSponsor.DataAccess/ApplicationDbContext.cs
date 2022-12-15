
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SponsorY.DataAccess.Models;
using static SponsorY.Utility.DataConstant.RegisterConstant;

namespace SponsorY.Data
{
	public class ApplicationDbContext : IdentityDbContext<AppUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, bool seed = true)
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

			builder.Entity<YoutuberTransaction>()
			  .HasKey(x => new { x.YoutuberId, x.TransactionId });

			builder.Entity<YoutuberTransaction>()
				.HasOne(yt => yt.Youtuber)
				.WithMany(y => y.YoutuberTransactions)
				.HasForeignKey(yt => yt.YoutuberId);

			builder.Entity<YoutuberTransaction>()
				.HasOne(yt => yt.Transaction)
				.WithMany(y => y.YoutuberTransactions)
				.HasForeignKey(yt => yt.TransactionId);

			builder.Entity<SponsorshipTransaction>()
			  .HasKey(x => new { x.SponsorId, x.TransactionId });

			builder.Entity<SponsorshipTransaction>()
				.HasOne(st => st.Sponsorship)
				.WithMany(s => s.SponsorshipTransactions)
				.HasForeignKey(st => st.SponsorId);

			builder.Entity<SponsorshipTransaction>()
				.HasOne(st => st.Transaction)
				.WithMany(s => s.SponsorshipTransactions)
				.HasForeignKey(st => st.TransactionId);

			builder
			  .Entity<Category>()
			  .HasData(
				new Category
				{
					Id = 1,
					CategoryName = "Gaming"
				},
				new Category
				{
					Id = 2,
					CategoryName = "Traveling"
				},
				new Category
				{
					Id = 3,
					CategoryName = "Sport"
				},
				new Category
				{
					Id = 4,
					CategoryName = "History"
				}
				);

			builder
			  .Entity<IdentityRole>()
			  .HasData(
				new IdentityRole()
				{
					Id = "b139508b-15e9-4c2c-9424-b46c2cf71e10",
					Name = "admin",
					NormalizedName = "ADMIN",
				},
				new IdentityRole()
				{
					Id = "c2596b34-7e0e-4c6d-b546-662d667e180b",
					Name = "youtuber",
					NormalizedName = "YOUTUBER",
				},
				new IdentityRole()
				{
					Id = "4444ec54-3fd1-4920-88f7-ba1c4d0b0b68",
					Name = "sponsor",
					NormalizedName = "SPONSOR",
				}
				);

			PasswordHasher<AppUser> hasher = new PasswordHasher<AppUser>();

			builder
			  .Entity<AppUser>()
			  .HasData(
				new AppUser()
				{
					Id = "4edef44e-3985-42c3-9e03-7a39d9cab63b",
					UserName = "Admin",
					NormalizedUserName =  "ADMIN",
					Email = "admin@abv.bg",
					NormalizedEmail = "ADMIN@ABV.BG",
					PasswordHash = hasher.HashPassword(null, "123456"),
					IsDeleted = false,
				}
				);

			builder
			  .Entity<IdentityUserRole<string>>()
			  .HasData(new IdentityUserRole<string>()
			  {
				  RoleId = "b139508b-15e9-4c2c-9424-b46c2cf71e10",
				  UserId = "4edef44e-3985-42c3-9e03-7a39d9cab63b"
			  }
			  );

			base.OnModelCreating(builder);
		}
	}
}