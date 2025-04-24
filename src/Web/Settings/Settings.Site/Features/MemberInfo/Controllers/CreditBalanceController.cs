using MediatR;
using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.CreditBalance;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Controllers;

public class CreditBalanceController(IMediator mediator) : SiteBaseController(mediator)
{
    public Task<ActionResult> Index(CreditBalanceQuery message) => ViewAsync(message);
}