using AlphaPlus.Core.Models;
using AlphaPlus.Core.Services;
using AlphaPlus.Settings.MemberInfo.API.Features.Shared;
using AlphaPlus.Settings.Site.Features.MemberBettingBehavior.BettingStatistic;
using AlphaPlus.Settings.Site.Features.MemberBettingBehavior.Profile;
using AlphaPlus.Settings.Site.Features.Shared;
using AlphaPlus.Settings.Site.Features.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.Features.MemberBettingBehavior;

public class MemberBettingBehaviorController : SiteBaseController
{
    private readonly IUser viewer;
    private readonly IIdentityClient identityClient;

    public MemberBettingBehaviorController(
        IMediator mediator,
        IUser viewer,
        IIdentityClient identityClient) : base(mediator)
    {
        this.viewer = viewer;
        this.identityClient = identityClient;
    }

    public ActionResult Index()
    {
        var isDarkMode = !string.IsNullOrWhiteSpace(viewer.DarkModeName);
        var viewModel = new MemberBettingBehaviorViewModel(isDarkMode, viewer.Language);

        return View(viewModel);
    }

    [HttpGet]
    public async Task<ActionResult> MemberProfile(MemberProfileQuery message)
    {
        var memberCustId = await GetMemberDownlineCustId(message.ViewerId, message.SubAccId, message.UserName);

        if (memberCustId > 0)
        {
            message.MemberCustId = memberCustId;
            return await Json(message);
        }

        return Json(Result.CreateErrorResult(""));
    }

    [HttpGet]
    public async Task<ActionResult> MemberBettingStatistic(MemberBettingStatisticQuery message)
    {
        var memberCustId = await GetMemberDownlineCustId(message.ViewerId, message.SubAccId, message.UserName);

        if (memberCustId > 0)
        {
            message.MemberCustId = memberCustId;
            return await Json(message);
        }

        return Json(Result.CreateErrorResult(""));
    }

    [HttpGet]
    public async Task<ActionResult> Excel(MemberBettingBehaviorExcelQuery message)
    {
        var memberCustId = await GetMemberDownlineCustId(message.ViewerId, message.SubAccId, message.UserName);

        if (memberCustId > 0)
        {
            message.MemberCustId = memberCustId;
            return await ExcelView(message);
        }

        return Json(Result.CreateErrorResult(""));
    }

    private async Task<int> GetMemberDownlineCustId(int custId, int subAccId, string username)
    {
        var downlineInfo = await identityClient.CheckBelongingByUsername(custId, subAccId, username);

        if (downlineInfo.IsActive && Enumeration.FromValue<UserLevel>(downlineInfo.RoleId).IsMemberLevel())
        {
            return downlineInfo.CustId;
        }

        return 0;
    }
}
