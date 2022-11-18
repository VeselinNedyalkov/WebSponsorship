﻿using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.Survices.Contract
{
    public interface IServiceSponsorship
    {
        Task<IEnumerable<SponsorViewModel>> GetAllSponsorshipsAsync();

        Task AddSponsorshipAsync(string userId, AddSponsorViewModel model);

        Task<Sponsorship> GetSponsorsEditAsync(int id);

        Task EditSponsorshipAsync(int EditId, Sponsorship model);

        Task<Sponsorship> GetSingelSponsorAsync(int SponsorId);
    }
}
