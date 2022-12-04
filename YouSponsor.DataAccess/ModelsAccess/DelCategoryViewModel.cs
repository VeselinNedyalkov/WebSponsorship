using SponsorY.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.ModelsAccess
{
	public class DelCategoryViewModel
	{

		public IEnumerable<Category> Categories { get; set; } = new List<Category>();
	}
}
