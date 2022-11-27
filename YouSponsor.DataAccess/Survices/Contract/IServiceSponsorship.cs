using SponsorY.DataAccess.Models;
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
        Task<IEnumerable<SponsorViewModel>> GetAllSponsorshipsAsync(string userId);

        Task AddSponsorshipAsync(string userId, AddSponsorViewModel model);

        Task<SponsorViewModel> GetSponsorsEditAsync(int id);

        Task EditSponsorshipAsync(int EditId, SponsorViewModel model);

        Task<Sponsorship> GetSingelSponsorAsync(int SponsorId);

        Task AddMoneyToSponsorAsync(int SponsorId, SponsorViewModel model);

		Task RemoveMoneyFromSponsorAsync(int SponsorId, SponsorViewModel model);

	}
}
