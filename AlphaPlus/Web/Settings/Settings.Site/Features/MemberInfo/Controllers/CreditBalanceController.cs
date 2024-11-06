using AlphaPlus.Settings.MemberInfo.Domain.Models.Winloss;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.CreditBalance;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.CreditBalance;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Controllers
{
    public class CreditBalanceController : SiteBaseController
    {
        public CreditBalanceController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(CreditBalanceQuery message)
            => View(message.IsValid()
                ? await mediator.Send(message)
                : new CreditBalanceViewModel(message, new EnteredSportResult(), false));
    }
}