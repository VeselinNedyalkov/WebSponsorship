using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SponsorY.Areas.User.Models;
using SponsorY.DataAccess.ModelsAccess.Sponsor;
using SponsorY.DataAccess.ModelsAccess.Transaction;
using SponsorY.DataAccess.Survices.Contract;
using System.Diagnostics;
using System.Security.Claims;

namespace SponsorY.Areas.Transaction.Controllers
{
    [Area("Transaction")]
	[Authorize(Roles = "sponsor,admin")]

	public class TransactionController : Controller
    {

        private readonly IServiceTransaction tranService;
        private readonly IServiceSponsorship sponsorService;
        private readonly IServiceCategory categoryService;


        public TransactionController(IServiceTransaction _tranService,
            IServiceSponsorship _sponsorService,
			IServiceCategory _categoryService
			)
        {
            tranService = _tranService;
            sponsorService = _sponsorService;
            categoryService = _categoryService;
        }

		public async Task<IActionResult> Find(int SponsorId, FindChanelViewModel modelInput)
        {
            FindChanelViewModel model = null;


			if (modelInput.Sorting == null)
            {
				model = await tranService.GetFindModelAsync(SponsorId);
			}
            else
            {
				model = await tranService.ReworkModelAsync(modelInput, SponsorId);
            }

            return View(model);
        }

		public async Task<IActionResult> Details(int ChanelId, int SponsorId)
        {
            TransactionViewModel model = await tranService.CreatedTransactionViewModelAsync(ChanelId, SponsorId);

            decimal Total = tranService.GetTotalPrice(model.QuantityClips, model.PricePerClip);

            model.TotalPrice = Total;

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var transaction = await tranService.CreateTransactionAsync(model, userId);

            model.TransactionId = transaction.Id;

            return View(model);
        }

		public async Task<IActionResult> Submit(Guid TranslId, int SponsorId , int ChanelId)
        {
            var transaction = await tranService.GetTransactionAsync(TranslId);
            var sponsor = await sponsorService.GetSponsorsEditAsync(SponsorId);
            

            try
            {
	            await tranService.RemoveMoneyFromSponsorAsync(SponsorId, transaction.TransferMoveney);
            }
            catch (Exception e)
            {
				return View("Error", new ErrorViewModel { RequestId = e.Message});

			}

			transaction.SuccessfulCreated = true;
            await tranService.UpdateCompletedTransactionAsync(transaction, SponsorId ,  ChanelId);

            await tranService.DeleteNotCompletedTransactions();

            TempData["success"] = "Transaction send for acceptance";

            return RedirectToAction(nameof(Requested));
        }

		public async Task<IActionResult> Requested()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var model = await tranService.GetAllUnaceptedTransaction(userId);

            return View(model);
        }

		public async Task<IActionResult> Plus(Guid TranslId, int SponsorId, int ChanelId)
        {
            
            var transaction = await tranService.GetTransactionAsync(TranslId);
            transaction.QuntityClips += 1;


			TransactionViewModel model = await tranService.CreatedTransactionViewModelAsync(ChanelId, SponsorId);

            model.QuantityClips = transaction.QuntityClips;
            model.TransactionId = transaction.Id;

            decimal Total = tranService.GetTotalPrice(model.QuantityClips, model.PricePerClip);

            model.TotalPrice = Total;
            transaction.TransferMoveney = Total;

            await tranService.UpdateTransactionAsync(transaction);
            return View(nameof(Details), model);
        }

		public async Task<IActionResult> Minus(Guid TranslId, int SponsorId, int ChanelId)
        {
            var transaction = await tranService.GetTransactionAsync(TranslId);
            transaction.QuntityClips -= 1;

            if (transaction.QuntityClips < 1)
            {
                transaction.QuntityClips = 1;

			}

			TransactionViewModel model = await tranService.CreatedTransactionViewModelAsync(ChanelId, SponsorId);

            model.QuantityClips = transaction.QuntityClips;
            model.TransactionId = transaction.Id;

            decimal Total = tranService.GetTotalPrice(model.QuantityClips, model.PricePerClip);

            model.TotalPrice = Total;
            transaction.TransferMoveney = Total;

            await tranService.UpdateTransactionAsync(transaction);
            return View(nameof(Details), model);
        }

        public async Task<IActionResult> Edit(Guid TransId)
        {
            TransactionViewModel model = null;

			try
            {
				 model = await tranService.EditTransactionAsync(TransId);
			}
            catch
            {
				return View("Error", new ErrorViewModel { RequestId = $"Transaction has a problem" });

			}

			return View(model);
		}

        [HttpPost]
		public async Task<IActionResult> Edit(TransactionViewModel model)
		{
            try
            {
				var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

				await tranService.EditSaveAsync(model , userId);
				TempData["success"] = "Transaction edith successful";

			}
			catch
            {
				return View("Error", new ErrorViewModel { RequestId = $"Edith not successful" });

			}

            return RedirectToAction(nameof(Requested));
		}

		public IActionResult Delete(Guid TransId)
		{
			try
			{
                tranService.DeleteTransactionAsync(TransId);

				TempData["success"] = "Transaction deleted";

			}
			catch
			{
				return View("Error", new ErrorViewModel { RequestId = $"Problem with deleting the transaction" });

			}

			return RedirectToAction(nameof(Requested));
		}
	}

	
}
