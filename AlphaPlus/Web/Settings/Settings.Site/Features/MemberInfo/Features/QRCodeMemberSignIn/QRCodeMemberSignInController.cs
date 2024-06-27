namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.QRCodeMemberSignIn
{
    using System.Threading.Tasks;
    using AlphaPlus.Settings.Site.Features.MemberInfo.Controllers;
    using AlphaPlus.Settings.Site.Features.MemberInfo.Features.QRCodeMemberSignIn.Requests;
    using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.QRCodeMemberSignIn.Models;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class QRCodeMemberSignInController : SiteBaseController
    {
        private const string IndexPath = "~/Features/MemberInfo/Features/QRCodeMemberSignIn/Views/Index.cshtml";
        private const string ShowQRCodePath = "~/Features/MemberInfo/Features/QRCodeMemberSignIn/Views/ShowQRCode.cshtml";
        private const string NotAuthorizedPath = "~/Features/MemberInfo/Features/QRCodeMemberSignIn/Views/_NotAuthorized.cshtml";

        private readonly int ErrorCodeDefault = 998;

        public QRCodeMemberSignInController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult> Index(QRCodeMemberSignInRequest message)
            => await View(message, IndexPath);

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAccount(CreateAccountByQRCodeRequest message)
        {
            if (message != null && !message.IsValid())
            {
                return Json(new ResultCreateMemberQRCode
                {
                    ErrCode = ErrorCodeDefault,
                    ErrMsg = message.IsInternal ? Lang.NotAuthorized : Resources.UsernameIsInvalid
                });
            }

            var result = await mediator.Send(message).ConfigureAwait(false);

            if (result.Success)
            {
                return Json(new
                {
                    result.ErrCode,
                    result.ErrMsg,
                    result.CustIdCopyFrom,
                    result.RawPassword,
                    SearchAccountId = result.CustId,
                    SearchAccountName = message?.Username,
                    message?.FirstName,
                    message?.Credit,
                    message?.CreditMin,
                    message?.CreditMax,
                    message?.ShowPageTitle,
                    CreateMode = true
                });
            }
            else
            {
                ModelState.AddModelError("DBErrors", result.ErrMsg ?? string.Empty);
                return Json(result);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public  async Task<ActionResult> GetMemberCreditAndStatus(GetMemberCreditAndStatusRequest message)
            => Json(await mediator.Send(message));

        [HttpGet]
        public async Task<ActionResult> ShowQRCode(ShowQRCodeRequest message)
        {
            if (message != null && !message.IsValid())
            {
                return View(message.IsInternal ? NotAuthorizedPath : InvalidPageViewPath);
            }

            var viewModel = await mediator.Send(message);
            return View(viewModel == null ? InvalidPageViewPath : ShowQRCodePath, viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> GenerateQrCode(GenerateQRCodeRequest message)
            => Json(await mediator.Send(message));

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> CheckExistedCustomer(CheckExistedCustomerRequest message)
           => Json(await mediator.Send(message));
    }
}