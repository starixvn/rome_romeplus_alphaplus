
using AlphaPlus.Settings.Site.Features.MemberInfo.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.Commission;

public class MemberCommissionController(IMediator mediator) : SiteBaseController(mediator)
{
    public Task<ActionResult> Index(MemberCommissionQuery message)
        => ViewAsync(message, "~/Features/MemberInfo/Features/Commission/Views/Index.cshtml");
}