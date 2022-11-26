using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SponsorY.Areas.User.Models;
using SponsorY.DataAccess.ModelsAccess;
using SponsorY.DataAccess.Survices.Contract;
using System.Diagnostics;
using System.Security.Claims;

namespace SponsorY.Areas.Transaction.Controllers
{
    [Authorize]
    [Area("Transaction")]

    public class TransactionController : Controller
    {

        private readonly IServiceTransaction tranService;
        private readonly IServiceSponsorship sponsorService;


        public TransactionController(IServiceTransaction _tranService,
            IServiceSponsorship _sponsorService
            )
        {
            tranService = _tranService;
            sponsorService = _sponsorService;
        }

        public async Task<IActionResult> Find(int SponsorId)
        {
            FindChanelViewModel model = await tranService.GetFindModelAsync(SponsorId);

            return View(model);
        }

        [HttpPost]
        public IActionResult Find(FindChanelViewModel model, int SponsorId)
        {
            model.SponsorshipId = SponsorId;

            return RedirectToAction("Search", model);
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

        public async Task<IActionResult> Submit(int TranslId, int SponsorId)
        {
            var transaction = await tranService.GetTransactionAsync(TranslId);
            var sponsor = await sponsorService.GetSponsorsEditAsync(SponsorId);

            if (sponsor.Wallet < transaction.TransferMoveney)
            {

                return View(new ErrorViewModel { RequestId = $"{sponsor.Wallet} Not enought! You need to increase your money for sponsorship!" });
            }

            transaction.SuccessfulCreated = true;
            await tranService.UpdateTransaction(transaction);

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

        public async Task<IActionResult> Plus(int TranslId)
        {
            var transaction = await tranService.GetTransactionAsync(TranslId);
            transaction.QuntityClips += 1;

            TransactionViewModel model = await tranService.CreatedTransactionViewModelAsync(transaction.YoutuberId, transaction.SponsorshipId);

            model.QuantityClips = transaction.QuntityClips;
            model.TransactionId = transaction.Id;

            decimal Total = tranService.GetTotalPrice(model.QuantityClips, model.PricePerClip);

            model.TotalPrice = Total;
            transaction.TransferMoveney = Total;

            await tranService.UpdateTransaction(transaction);
            return View(nameof(Details), model);
        }

        public async Task<IActionResult> Minus(int TranslId)
        {
            var transaction = await tranService.GetTransactionAsync(TranslId);
            transaction.QuntityClips -= 1;

            TransactionViewModel model = await tranService.CreatedTransactionViewModelAsync(transaction.YoutuberId, transaction.SponsorshipId);

            model.QuantityClips = transaction.QuntityClips;
            model.TransactionId = transaction.Id;

            decimal Total = tranService.GetTotalPrice(model.QuantityClips, model.PricePerClip);

            model.TotalPrice = Total;
            transaction.TransferMoveney = Total;

            await tranService.UpdateTransaction(transaction);
            return View(nameof(Details), model);
        }

        public async Task<IActionResult> Edit(int TransId)
        {
            TransactionViewModel model = null;

			try
            {
				 model = await tranService.EditTransactionAsync(TransId);
			}
            catch(Exception ex)
            {
				return View(new ErrorViewModel { RequestId = $"Transaction has a problem" });
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
			catch (Exception ex) 
            {
				return View(new ErrorViewModel { RequestId = $"Edith not successful" });
			}

            return RedirectToAction(nameof(Requested));
		}

		public async Task<IActionResult> Delete(int TransId)
		{
			try
			{
                tranService.DeleteTransactionAsync(TransId);

				TempData["success"] = "Transaction deleted";

			}
			catch (Exception ex)
			{
				return View(new ErrorViewModel { RequestId = $"Problem with deleting the transaction" });
			}

			return RedirectToAction(nameof(Requested));
		}
	}

	
}
