using Microsoft.EntityFrameworkCore;
using SponsorY.Data;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess;
using SponsorY.DataAccess.Survices.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SponsorY.DataAccess.Survices
{
    public class ServiceSponsorship : IServiceSponsorship
    {
        private readonly ApplicationDbContext context;

        public ServiceSponsorship(ApplicationDbContext _context)
        {
            context = _context;
        }

        /// <summary>
        /// add user to DB
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>

        public async Task AddSponsorshipAsync(string userId, AddSponsorViewModel model)
        {
            Sponsorship sponsor = new Sponsorship
            {
                CompanyName = model.CompanyName,
                Product = model.Product,
                Url = model.Url,
                Wallet = model.Wallet,
                AppUserId = userId,
            };

            await context.Sponsorships.AddAsync(sponsor);
            context.SaveChanges();
        }

        public async Task EditSponsorshipAsync(int EditId, Sponsorship model)
        {
            var original = await GetSponsorsEditAsync(EditId);

            Sponsorship edit = new Sponsorship
            {
                Id = original.Id,
                CompanyName = model.CompanyName,
                Product = model.Product,
                Url = model.Url,
                Wallet = model.Wallet,
                AppUserId = original.AppUserId,
                Transfers = original.Transfers,
            };

            context.Sponsorships.Update(edit);
            context.SaveChanges();
        }

        /// <summary>
        /// Take all sponsorships 
        /// </summary>
        /// <returns></returns>

        public async Task<IEnumerable<SponsorViewModel>> GetAllSponsorshipsAsync()
        {
            var result = await context.Sponsorships
                .Select(x => new SponsorViewModel
                {
                    Id = x.Id,
                    CompanyName = x.CompanyName,
                    Product = x.Product,
                    Url = x.Url,
                    Wallet = x.Wallet,
                }).ToListAsync();


            return result;
        }


        /// <summary>
        /// Take the user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Sponsorship> GetSponsorsEditAsync(int id)
        {
            var user = await context.Sponsorships.Where(x => x.Id == id)
                .Select(x => new Sponsorship
                {
                    Id = x.Id,
                    CompanyName = x.CompanyName,
                    Product = x.Product,
                    Url = x.Url,
                    Wallet = x.Wallet,
                    AppUserId = x.AppUserId,
                    Transfers = x.Transfers
                }).FirstOrDefaultAsync();

            return user!;
        }
    }
}
