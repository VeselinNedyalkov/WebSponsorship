using SponsorY.DataAccess.ModelsAccess.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.Survices.Contract
{
    public interface IServiceUser
    {
        Task<bool> IsUserDeletedAsync(string userName);
        Task UpdateUserInfoAsync(string userId, UserInfoViewModel model);

        Task DeleteUserAsync(string userId);

	}
}
