
using AlphaPlus.Settings.Site.Features.MemberInfo.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.Commission;

public class CommissionController(IMediator mediator) : SiteBaseController(mediator)
{
    public Task<ActionResult> Index(CommissionQuery message)
        => View(message, "~/Features/MemberInfo/Features/Commission/Views/Index.cshtml");

    public Task<ActionResult> GetData(CommDataQuery message)
        => View(message, "~/Features/MemberInfo/Features/Commission/Views/CommData.cshtml");

    public Task<ActionResult> GetAgents(AgentListQuery message) => Json(message);
}