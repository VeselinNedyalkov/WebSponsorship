using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.Survices.Contract
{
    public interface IServiceCategory
    {
        Task AddCategoryAync(CategoryViewModel model);

        Task<IEnumerable<Category>> GetAllCategoryAsync();
    }
}
