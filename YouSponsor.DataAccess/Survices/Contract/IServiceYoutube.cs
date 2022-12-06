using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess.Youtube;

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

		Task TransactionCompletedAsync(Guid transactionId);

		Task TransactionDenialAsync(Guid transactionId);

		Task<YoutubeFinancesViewModel> GetAllFinancesaAsync(string userId);

		Task WithdrawMOneyAsync(string userId, YoutubeFinancesViewModel model);

		Task<IEnumerable<YoutuberAwaitTransactionViewModel>> GetallCompletedTransactionsAsync(string userId);
	}
}
