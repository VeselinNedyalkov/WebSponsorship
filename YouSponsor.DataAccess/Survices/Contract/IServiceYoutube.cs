using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess;

namespace SponsorY.DataAccess.Survices.Contract
{
	public interface IServiceYoutub

	{
		Task<IEnumerable<YouTubeViewModel>> GetAllYoutubeChanelsAsync(string userId);
		Task<IEnumerable<FindYoutuberViewModel>> FindAllYoutubersAsync();
		

		Task AddYoutubChanelAsync(string userId ,AddYoutViewModel model);

		Task<YouTubeViewModel> TakeYoutuberAsync(int id);

		Task EditYoutuberAsync(int EditId , YouTubeViewModel model);

		void DeleteYoutuber(int DelteId);

		Task<IEnumerable<YoutubersFilterCatViewModel>> GetChanelWithCategoryAsync(int categoryId);

		Task<IEnumerable<YoutuberAwaitTransactionViewModel>> GetAllTransactionsAwaitingAsync(string userId);

		Task TransactionCompletedAsync(int transactionId);

		Task TransactionDenialAsync(int transactionId);

		Task<YoutubeFinancesViewModel> GetAllFinancesaAsync(string userId);
	}
}
