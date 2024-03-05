namespace AlphaPlus.Settings.Site.Features.CustomerList
{
    using AlphaPlus.Settings.Site.Controllers;
    using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
    using MediatR;
    using AlphaPlus.Settings.MemberInfo.Domain.AppResources;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using AlphaPlus.Settings.MemberInfo.API.Services.Transfer;
    using Microsoft.AspNetCore.Mvc;
    using AlphaPlus.Settings.MemberInfo.Domain.Configurations;

    public class CustomerListController : SiteBaseController
    {
        private const string IndexPath = "~/MemberInfo/Features/CustomerList/Views/Index.cshtml";
        private const string MultipleDoubleCommPath = "~/MemberInfo/Features/CustomerList/Views/MultipeDoubleComm.cshtml";
        private readonly Result ErrorDefault = Result.CreateErrorResult(Lang.NotAuthorized);
        private readonly TransferService transferService;
        private readonly IUser viewer;

        public CustomerListController(
            IMediator mediator,
            TransferService transferService,
            IUser viewer)
            : base(mediator)
        {
            this.transferService = transferService;
            this.viewer = viewer;
        }

        [Route("CustomerList/master")]
        [Route("CustomerList/agent")]
        [Route("CustomerList/member")]
        [HttpGet]
        public ActionResult Index(CustomerListRequest message)
        {
            message.SupportAutoTransfer = transferService.IsEnableAutoTransfer(viewer,AppSettings.GetConfigValue("OfferAutoTransfer.Currencies"));
            message.AutoTransferAffect = transferService.IsAutoTransferAffect(viewer, AppSettings.GetConfigValue("OfferAutoTransfer.Currencies"));
            return View(message, IndexPath);
        }

        [HttpPost]
        public ActionResult GetCustomerListData(FetchDownlinesListRequest message)
        {
            if (!message.IsValid())
            {
                return Json(ErrorDefault);
            }

            if (message.EnableAllDayBalanceTransfer.HasValue && transferService.IsAutoTransferAffect(viewer, AppSettings.GetConfigValue("OfferAutoTransfer.Currencies")))
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

            message.SupportAutoTransfer = transferService.IsEnableAutoTransfer(viewer, AppSettings.GetConfigValue("OfferAutoTransfer.Currencies"));
            message.AutoTransferAffect = transferService.IsAutoTransferAffect(viewer, AppSettings.GetConfigValue("OfferAutoTransfer.Currencies"));
            return Json(mediator.Send(message));
        }

        public ActionResult GetCustomerAllStatus(CustomerStatusQuery message)
        {
            if (!message.IsValid())
            {
                return Json(ErrorDefault);
            }

            message.SupportAutoTransfer = transferService.IsEnableAutoTransfer(viewer, AppSettings.GetConfigValue("OfferAutoTransfer.Currencies"));
            message.AutoTransferAffect = transferService.IsAutoTransferAffect(viewer, AppSettings.GetConfigValue("OfferAutoTransfer.Currencies"));
            return Json(mediator.Send(message));
        }

        [HttpPost]
        public ActionResult MultipleEditPopup(CustomerStatusMultipleQuery message)
        {
            if (!message.IsValid())
            {
                return Json(ErrorDefault);
            }

            message.SupportAutoTransfer = transferService.IsEnableAutoTransfer(viewer, AppSettings.GetConfigValue("OfferAutoTransfer.Currencies"));
            message.AutoTransferAffect = transferService.IsAutoTransferAffect(viewer, AppSettings.GetConfigValue("OfferAutoTransfer.Currencies"));
            return Json(mediator.Send(message));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UpdateCustomerStatus(CustomerNonProductStatusCommand command)
        {
            if (!command.IsValid())
            {
                return Json(ErrorDefault);
            }

            command.SupportAutoTransfer = transferService.IsEnableAutoTransfer(viewer, AppSettings.GetConfigValue("OfferAutoTransfer.Currencies"));
            command.AutoTransferAffect = transferService.IsAutoTransferAffect(viewer, AppSettings.GetConfigValue("OfferAutoTransfer.Currencies"));

            return Json(mediator.Send(command));
        }

        public ActionResult MultipleDoubleComm(MultipleDoubleCommRequest message)
            => View(message, MultipleDoubleCommPath);

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UpdateMultipleDoubleComm(UpdateMultipleDoubleCommCommand command)
        {
            if (!command.IsValid())
            {
                return Json(ErrorDefault);
            }

            return Json(mediator.Send(command));
        }
    }
}