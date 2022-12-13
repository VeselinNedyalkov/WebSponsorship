using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.Models
{
	public class Role
	{
		public Guid Id { get; set; }

		public string Name { get; set; }
		public string NormalizedName { get; set; }
	}
}
