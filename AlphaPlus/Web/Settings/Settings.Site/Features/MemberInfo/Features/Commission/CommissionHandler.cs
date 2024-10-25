using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using AlphaPlus.Settings.MemberInfo.API.Features.Shared;
using AlphaPlus.Settings.MemberInfo.API.Services;
using AlphaPlus.Settings.MemberInfo.Domain.Configurations;
using AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models;
using MediatR;
using AlphaPlus.Settings.MemberInfo.API.Features.Commission;
using AlphaPlus.Settings.MemberInfo.Domain.Features.Commission;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.Commission;

public class CommissionHandler(
    IProductService productService,
    IAccountService accountService,
    ICommissionService commissionService) :
        IRequestHandler<CommissionQuery, CommissionViewModel>,
        IRequestHandler<CommDataQuery, CommissionViewModel>,
        IRequestHandler<AgentListQuery, IEnumerable<CustInfo>>
{
    private readonly IAccountService accountService = accountService;
    private readonly IProductService productService = productService;
    private readonly ICommissionService commissionService = commissionService;

    public async Task<CommissionViewModel> Handle(CommissionQuery message, CancellationToken cancellationToken)
    {
        IEnumerable<CustInfo> masters = null;
        IEnumerable<CustInfo> agents = null;
        CommissionReport commReport;
        var actorLevel = message.ActorLevel;
        var products = await productService.GetCommProducts(message.ViewerId, AppSettings.SettingStatus);

        if (actorLevel.IsAgentLevel())
        {
            commReport = await commissionService.GetAgentComm(
                new CommissionParam(message.Id, message.ViewerId, AppSettings.SettingStatus));
        }
        else if (actorLevel.IsMasterLevel())
        {
            agents = await accountService.GetAgentList(message.ViewerId);
            var firstAgentId = agents.FirstOrDefault()?.CustId ?? 0;
            commReport = await commissionService.GetAgentComm(
                new CommissionParam(message.Id, firstAgentId, AppSettings.SettingStatus));
        }
        else
        {
            masters = await accountService.GetMasterList(message.ViewerId);
            var firstMasterId = masters.FirstOrDefault()?.CustId ?? 0;

            var agentList = (await accountService.GetAgentList(firstMasterId)).ToList();
            if (agentList.Any())
            {
                agentList.Insert(0, new CustInfo(0, Resources.AllAgent, 0));
            }

            agents = agentList;
            commReport = await commissionService.GetMasterComm(
                new CommissionParam(message.Id, firstMasterId, AppSettings.SettingStatus));
        }

        return new CommissionViewModel(message, commReport, products, masters, agents);
    }

    public async Task<CommissionViewModel> Handle(CommDataQuery message, CancellationToken cancellationToken)
    {
        var actorLevel = message.ActorLevel;
        var commReportTask = actorLevel.IsSuperLevel() && message.MasterId > 0
            ? commissionService.GetMasterComm(
                new CommissionParam(message.Id, message.MasterId, AppSettings.SettingStatus))
            : commissionService.GetAgentComm(
                new CommissionParam(message.Id, message.CustId, AppSettings.SettingStatus));

        return new CommissionViewModel(message, await commReportTask);
    }

    public async Task<IEnumerable<CustInfo>> Handle(AgentListQuery message, CancellationToken cancellationToken)
    {
        var agentList = (await accountService.GetAgentList(message.MasterId)).ToList();
        if (agentList.Any())
        {
            agentList.Insert(0, new CustInfo(0, Resources.AllAgent, 0));
        }

        return agentList;
    }
}