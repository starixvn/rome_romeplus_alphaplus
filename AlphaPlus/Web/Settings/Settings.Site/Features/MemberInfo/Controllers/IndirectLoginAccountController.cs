namespace AlphaPlus.Settings.Site.Features.MemberInfo.Controllers
{
    using System.Threading.Tasks;
    using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.IndirectLoginAccount;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class IndirectLoginAccountController : SiteBaseController
    {
        public IndirectLoginAccountController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(ManageAccountQuery message)
            => await ViewAsync(message);

        public async Task<ActionResult> NewAccount(NewAccountQuery message)
            => View(model: await mediator.Send(message));

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> NewAccount(NewAccountCommand message)
            => await JsonAsync(message);

        public async Task<ActionResult> ValidateAccount(ValidateAccountQuery message)
            => await JsonAsync(message);

        public async Task<ActionResult> UpdatePassword(UpdatePasswordQuery message)
            => await View(message, "~/Features/MemberInfo/Views/IndirectLoginAccount/UpdatePassword.cshtml");

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdatePassword(UpdatePasswordCommand message)
            => await JsonAsync(message);

        public ActionResult DisconnectPopup(DisconnectCommand message)
            => View(message);

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Disconnect(DisconnectCommand message)
            => await JsonAsync(message);

        public async Task<ActionResult> Redirect(RedirectQuery message)
        {
            if (message.IsValid())
            {
                var url = await mediator.Send(message);

                if (!string.IsNullOrWhiteSpace(url))
                {
                    return Redirect(url);
                }
            }

            return new EmptyResult();
        }
    }
}