using AlphaPlus.Settings.MemberInfo.API.Features.Commission;
using AlphaPlus.Settings.MemberInfo.API.Features.Shared;
using AlphaPlus.Settings.MemberInfo.API.Services;
using AlphaPlus.Settings.MemberInfo.Domain.Configurations;
using AlphaPlus.Settings.MemberInfo.Domain.Features.Commission;
using AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models;
using AlphaPlus.Shared.AppResources.Common;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.Commission;

public class CommissionHandler(
    IProductService productService,
    IAccountService accountService,
    ICommissionService commissionService) :
        IRequestHandler<CommissionQuery, CommissionViewModel>,
        IRequestHandler<CommDataQuery, CommDataViewModel>,
        IRequestHandler<AgentListQuery, IList<CustInfo>>
{
    private readonly IAccountService accountService = accountService;
    private readonly IProductService productService = productService;
    private readonly ICommissionService commissionService = commissionService;

    public async Task<CommissionViewModel> Handle(CommissionQuery message, CancellationToken cancellationToken)
    {
        IList<CustInfo> masters = null;
        IList<CustInfo> agents = null;
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
            if (message.RecommendId == -1)
            {
                message.RecommendId = agents.Count > 1 ? agents[1].CustId : 0;
            }

            commReport = await GetMasterComm(message);
        }
        else
        {
            masters = await GetMasterList(message.ViewerId);
            if (message.MasterId == -1)
            {
                message.MasterId = masters.Count > 1 ? masters[1].CustId : 0;
            }

            agents = message.MasterId == 0
                ? new List<CustInfo>()
                : await GetAgentList(message.MasterId, isIncludeAllAgentsText: true, isIncludeDirectMemberText: false);
            commReport = await GetSuperComm(message);
        }

        return new CommissionViewModel(message, commReport, products, masters, agents, message.MasterId, message.RecommendId);
    }

    public async Task<CommDataViewModel> Handle(CommDataQuery message, CancellationToken cancellationToken)
    {
        Task<CommissionReport> commReportTask = null;
        var actorLevel = message.ActorLevel;

        if (actorLevel.IsMasterLevel())
        {
            commReportTask = GetMasterComm(message);
        }
        else if (actorLevel.IsSuperLevel())
        {
            commReportTask = GetSuperComm(message);
        }

        return new CommDataViewModel(message, await commReportTask);
    }

    private Task<CommissionReport> GetSuperComm(CommDataQuery message)
    {
        CommissionParam param;
        Task<CommissionReport> commReportTask;
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

        return commReportTask;
    }

    private Task<CommissionReport> GetMasterComm(CommDataQuery message)
    {
        var param = message.RecommendId == 0
            ? new CommissionParam(message.ProductId, message.ViewerId, AppSettings.SettingStatus, true)
            : new CommissionParam(message.ProductId, message.RecommendId, AppSettings.SettingStatus, false);
        return commissionService.GetAgentComm(param);
    }

    public Task<IList<CustInfo>> Handle(AgentListQuery message, CancellationToken cancellationToken)
        => GetAgentList(message.MasterId, isIncludeAllAgentsText: true, isIncludeDirectMemberText: false);

    private async Task<IList<CustInfo>> GetMasterList(int viewerId)
    {
        var masters = (await accountService.GetMasterList(viewerId)).ToList();
        masters.Insert(0, new CustInfo(0, CommonResx.DirectMember, 0));

        return masters;
    }

    private async Task<IList<CustInfo>> GetAgentList(
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