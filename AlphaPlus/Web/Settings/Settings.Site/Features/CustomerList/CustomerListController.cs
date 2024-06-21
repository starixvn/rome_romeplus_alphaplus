using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using MediatR;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using AlphaPlus.Settings.MemberInfo.API.Services.Transfer;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.MemberInfo.Domain.Configurations;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Shared;

namespace AlphaPlus.Settings.Site.Features.CustomerList
{
    public class CustomerListController : SiteBaseController
    {
        private const string IndexPath = "~/Features/CustomerList/Views/Index.cshtml";
        private const string MultipleDoubleCommPath = "~/Features/CustomerList/Views/MultipleDoubleComm.cshtml";
        private readonly Result ErrorDefault = Result.CreateErrorResult(Lang.NotAuthorized);
        private readonly TransferService transferService;

        public CustomerListController(IMediator mediator, TransferService transferService)
            : base(mediator)
        {
            this.transferService = transferService;
        }

        [Route("CustomerList/master")]
        [Route("CustomerList/agent")]
        [Route("CustomerList/member")]
        [HttpGet]
        public async Task<ActionResult> Index(CustomerListRequest message)
        {
            return await View(message, IndexPath);
        }

        [HttpPost]
        public async Task<ActionResult> GetCustomerListData(FetchDownlinesListRequest message)
        {
            if (!message.IsValid())
            {
                return Json(ErrorDefault);
            }

            if (message.EnableAllDayBalanceTransfer.HasValue && transferService.IsAutoTransferAffect(message.Currency, AppSettings.GetConfigValue("OfferAutoTransfer.Currencies")))
            {
                if (!message.EnableAllDayBalanceTransfer.Value && transferService.IsAllDayBalanceTransfer(message.CustId))
                {
                    return Json(Result.CreateErrorResult(Resources.ADBTOnError, 100));
                }

                if (message.EnableAllDayBalanceTransfer.Value && !transferService.IsAllDayBalanceTransfer(message.CustId))
                {
                    return Json(Result.CreateErrorResult(Resources.ADBTOffError, 100));
                }
            }

            return Json(await mediator.Send(message));
        }

        public async Task<ActionResult> GetCustomerAllStatus(CustomerStatusQuery message)
        {
            if (!message.IsValid())
            {
                return Json(ErrorDefault);
            }

            return Json(await mediator.Send(message));
        }

        [HttpPost]
        public async Task<ActionResult> MultipleEditPopup(CustomerStatusMultipleQuery message)
        {
            if (!message.IsValid())
            {
                return Json(ErrorDefault);
            }

            return Json(await mediator.Send(message));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateCustomerStatus(CustomerNonProductStatusCommand command)
        {
            if (!command.IsValid())
            {
                return Json(ErrorDefault);
            }

            return Json(await mediator.Send(command));
        }

        public async Task<ActionResult> MultipleDoubleComm(MultipleDoubleCommRequest message)
            => await View(message, MultipleDoubleCommPath);

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateMultipleDoubleComm(UpdateMultipleDoubleCommCommand command)
        {
            if (!command.IsValid())
            {
                return Json(ErrorDefault);
            }

            return Json(await mediator.Send(command));
        }
    }
}