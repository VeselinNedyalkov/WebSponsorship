using SponsorY.DataAccess.Models;
using SponsorY.DataAccess.ModelsAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorY.DataAccess.Survices.Contract
{
	public interface IServiceTransaction
	{
		decimal GetTotalPrice(int quantity, decimal PricePerClip);
		Task<TransactionViewModel> CreatedTransactionViewModelAsync(int youId, int SponsorId);

		Task<Transaction> GetTransactionAsync(Guid TranslId);
		Task UpdateCompletedTransactionAsync(Transaction model, int SponsorId, int ChanelId);
		Task UpdateTransactionAsync(Transaction model);
		Task<FindChanelViewModel> GetFindModelAsync(int SponsorId);
		Task<Transaction> CreateTransactionAsync(TransactionViewModel model,string userId);

		Task<IEnumerable<NotAcceptedTransactionViewModel>> GetAllUnaceptedTransaction(string userId);

		Task DeleteNotCompletedTransactions();

		Task<TransactionViewModel> EditTransactionAsync(Guid TransId);

		Task EditSaveAsync(TransactionViewModel model, string userId);

		void DeleteTransactionAsync(Guid TransId);

		Task RemoveMoneyFromSponsorAsync(int SponsorId, decimal Amount);
	}
}
