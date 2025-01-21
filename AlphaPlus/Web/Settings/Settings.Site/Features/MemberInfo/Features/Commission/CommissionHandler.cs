using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using AlphaPlus.Settings.MemberInfo.API.Features.Shared;
using AlphaPlus.Settings.MemberInfo.API.Services;
using AlphaPlus.Settings.MemberInfo.Domain.Configurations;
using AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models;
using MediatR;
using AlphaPlus.Settings.MemberInfo.API.Features.Commission;
using AlphaPlus.Settings.MemberInfo.Domain.Features.Commission;
using AlphaPlus.Shared.AppResources.Common;

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
                new CommissionParam(message.ProductId, message.ViewerId, AppSettings.SettingStatus, false));
        }
        else if (actorLevel.IsMasterLevel())
        {
            agents = await GetAgentList(message.ViewerId, isIncludeAllAgentsText: false, isIncludeDirectMemberText: true);
            commReport = await commissionService.GetAgentComm(
                new CommissionParam(message.ProductId, message.ViewerId, AppSettings.SettingStatus, true));
        }
        else
        {
            masters = await GetMasterList(message.ViewerId);
            agents = Enumerable.Empty<CustInfo>();
            commReport = await commissionService.GetMasterComm(
                new CommissionParam(message.ProductId, message.ViewerId, AppSettings.SettingStatus, true));
        }

        return new CommissionViewModel(message, commReport, products, masters, agents);
    }

    public async Task<CommissionViewModel> Handle(CommDataQuery message, CancellationToken cancellationToken)
    {
        CommissionParam param;
        Task<CommissionReport> commReportTask = null;
        var actorLevel = message.ActorLevel;

        if (actorLevel.IsMasterLevel())
        {
            param = message.RecommendId == 0
                ? new CommissionParam(message.ProductId, message.ViewerId, AppSettings.SettingStatus, true)
                : new CommissionParam(message.ProductId, message.RecommendId, AppSettings.SettingStatus, false);
            commReportTask = commissionService.GetAgentComm(param);
        }
        else if (actorLevel.IsSuperLevel())
        {
            if (message.RecommendId > 0)
            {
                param = new CommissionParam(message.ProductId, message.RecommendId, AppSettings.SettingStatus, false);
                commReportTask = commissionService.GetAgentComm(param);
            }
            else
            {
                param = message.MasterId == 0
                    ? new CommissionParam(message.ProductId, message.ViewerId, AppSettings.SettingStatus, true)
                    : new CommissionParam(message.ProductId, message.MasterId, AppSettings.SettingStatus, false);
                commReportTask = commissionService.GetMasterComm(param);
            }
        }

        return new CommissionViewModel(message, await commReportTask);
    }

    public Task<IEnumerable<CustInfo>> Handle(AgentListQuery message, CancellationToken cancellationToken)
        => GetAgentList(message.MasterId, isIncludeAllAgentsText: true, isIncludeDirectMemberText: false);

    private async Task<IEnumerable<CustInfo>> GetMasterList(int viewerId)
    {
        var masters = (await accountService.GetMasterList(viewerId)).ToList();
        masters.Insert(0, new CustInfo(0, CommonResx.DirectMember, 0));

        return masters;
    }

    private async Task<IEnumerable<CustInfo>> GetAgentList(
        int masterId,
        bool isIncludeAllAgentsText,
        bool isIncludeDirectMemberText)
    {
        var agents = (await accountService.GetAgentList(masterId)).ToList();

        if (isIncludeAllAgentsText && agents.Any())
        {
            agents.Insert(0, new CustInfo(0, Resources.AllAgent, 0));
        }

        if (isIncludeDirectMemberText)
        {
            agents.Insert(0, new CustInfo(0, CommonResx.DirectMember, 0));
        }

        return agents;
    }
}