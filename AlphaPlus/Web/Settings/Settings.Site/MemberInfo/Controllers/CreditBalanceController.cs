namespace AlphaPlus.Settings.Site.Controllers
{
    using System.Threading.Tasks;
    using AlphaPlus.Settings.MemberInfo.Domain.Models.Winloss;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Requests.CreditBalance;
    using ViewModels.CreditBalance;

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