using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess;

namespace SponsorY.DataAccess.Survices.Contract
{
	public interface IServiceYoutub

	{
		Task<IEnumerable<YouTubeViewModel>> GetAllYoutubeChanlesAsync();
		

		Task AddYoutubChanelAsync(string userId ,AddYoutViewModel model);

		Task<YouTubeViewModel> GetYoutuberEditAsync(int id);

		Task EditYoutuberAsync(int EditId , YouTubeViewModel model);

		void DeleteYoutuber(int DelteId);
	}
}
