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

namespace SponsorY.DataAccess.Survices
{
    public class ServiceUser : IServiceUser
    {

        private readonly ApplicationDbContext context;

        public ServiceUser(ApplicationDbContext _context)
        {
            context = _context;
        }

        /// <summary>
        /// Update Information for user in DB
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateUserInfoAsync(string userId, UserInfoViewModel model)
        {
            var isUserExist = context.UsersInfo.Any(x => x.AppUserId == userId);

            UserInfo addUser = new UserInfo
            {
                Id = 1,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Age = model.Age,
                Country = model.Country,
                AppUserId = userId,
            };

            if (!isUserExist)
            {
                UserInfo createUser = new UserInfo
                {
                    Id = 1,
                    FirstName = null,
                    LastName = null,
                    Age = null,
                    Country = null,
                    AppUserId = userId,
                };
                await context.UsersInfo.AddAsync(addUser);
                context.SaveChanges();
            }


            context.UsersInfo.Update(addUser);
            context.SaveChanges();
        }

        /// <summary>
        /// Check if the account is deleted
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<bool> IsUserDeletedAsync(string userName)
        {
            var isD = await context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            return  isD.IsDeleted;
        }

		public async Task DeleteUserAsync(string userId)
		{
			var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            user.IsDeleted = true;

            context.Users.Update(user);
            await context.SaveChangesAsync();
		}
	}
}
