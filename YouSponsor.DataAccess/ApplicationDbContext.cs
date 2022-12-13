﻿
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess;
using SponsorY.DataAccess.SeedConfiguration;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using static SponsorY.Utility.DataConstant.RegisterConstant;

namespace SponsorY.Data
{
	public class ApplicationDbContext : IdentityDbContext<AppUser>
	{
		private bool seedDb;
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, bool seed = false)
			: base(options)
		{
			if (this.Database.IsRelational())
			{
				this.Database.Migrate();
			}
			else
			{
				this.Database.EnsureCreated();
			}

			this.seedDb = seed;
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


			//set seedDb = true in constructor for seeding data to DB
			if (seedDb)
			{
				builder.ApplyConfiguration(new CategoryConfiguration());
				builder.ApplyConfiguration(new RoleConfiguration());
				builder.ApplyConfiguration(new UsersConfiguration());
			}


			base.OnModelCreating(builder);
		}
	}
}