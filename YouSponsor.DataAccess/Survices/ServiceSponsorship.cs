﻿using Microsoft.EntityFrameworkCore;
using SponsorY.Data;
using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess;
using SponsorY.DataAccess.Survices.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SponsorY.DataAccess.Survices
{
    public class ServiceSponsorship : IServiceSponsorship
    {
        private readonly ApplicationDbContext context;
        private readonly IServiceCategory categoryService;

        public ServiceSponsorship(ApplicationDbContext _context,
            IServiceCategory _categoryService)
        {
            context = _context;
            categoryService = _categoryService;
        }


        public async Task AddSponsorshipAsync(string userId, AddSponsorViewModel model)
        {
            Sponsorship sponsor = new Sponsorship
            {
                CompanyName = model.CompanyName,
                Product = model.Product,
                Url = model.Url,
                Wallet = model.Wallet,
                AppUserId = userId,
                CategoryId = model.CategoryId
            };

            await context.Sponsorships.AddAsync(sponsor);
            context.SaveChanges();
        }

        public async Task EditSponsorshipAsync(int EditId, SponsorViewModel model)
        {
            var original = await GetSponsorsEditAsync(EditId);

            Sponsorship edit = new Sponsorship
            {
                Id = original.Id,
                CompanyName = model.CompanyName,
                Product = model.Product,
                Url = model.Url,
                Wallet = model.Wallet,
                CategoryId= model.CategoryId,
                AppUserId = original.AppUserId,
            };

            context.Sponsorships.Update(edit);
			await context.SaveChangesAsync();
		}


        public async Task<IEnumerable<SponsorViewModel>> GetAllSponsorshipsAsync(string userId)
        {
            var result = await context.Sponsorships
                .Where(x => x.AppUserId == userId)
                .Select(x => new SponsorViewModel
                {
                    Id = x.Id,
                    CompanyName = x.CompanyName,
                    Product = x.Product,
                    Url = x.Url,
                    CategoryId = x.CategoryId,
                    Wallet = x.Wallet,
                }).ToListAsync();

            foreach (var item in result)
            {
                item.CategoryName = await categoryService.GetCategoryNameAsync(item.CategoryId);

			}

            return result;
        }

        public async Task<Sponsorship> GetSingelSponsorAsync(int SponsorId)
        {
            return await context.Sponsorships.Where(x => x.Id == SponsorId).FirstOrDefaultAsync();
        }

        public async Task<SponsorViewModel> GetSponsorsEditAsync(int id)
        {
            var user = await context.Sponsorships.Where(x => x.Id == id)
                .Select(x => new SponsorViewModel
                {
                    Id = x.Id,
                    CompanyName = x.CompanyName,
                    Product = x.Product,
                    Url = x.Url,
                    Wallet = x.Wallet,
                    AppUserId = x.AppUserId,
                }).FirstOrDefaultAsync();

            user.CategoryName = await categoryService.GetCategoryNameAsync(user.CategoryId);
            user.Categories = await categoryService.GetAllCategoryAsync();

            return user!;
        }
    }
}
