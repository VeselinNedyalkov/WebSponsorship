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
		Task<TransactionViewModel> CreatedTransactionViewModelAsync(int ChanelId, int SponsorId);

		Task<Transaction> GetTransactionAsync(int TranslId);
		Task UpdateTransaction(Transaction model);
		Task<FindChanelViewModel> GetFindModelAsync(int SponsorId);
		Task<Transaction> CreateTransactionAsync(TransactionViewModel model,string userId);

		Task<IEnumerable<NotAcceptedTransactionViewModel>> GetAllUnaceptedTransaction(string userId);
	}
}
