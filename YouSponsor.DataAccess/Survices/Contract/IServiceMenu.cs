using SponsorY.DataAccess.ModelsAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.Survices.Contract
{
	public interface IServiceMenu
	{
		Task<UserInfoViewModel> GetUserInfo(string userId);
	}
}
