using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.Reward;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.Reward;
using AlphaPlus.Settings.Transfer.API.Models.Reward;
using AlphaPlus.Settings.Transfer.API.Params.Reward;
using AlphaPlus.Settings.Transfer.API.Services;
using MediatR;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Settings.Site.Features.UserNameSuggestion;

namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers;
public class RewardHandler(
    IRewardService rewardService,
    ICustomerService customerService,
    UsernameSuggestionService usernameSuggestionService) :
    IRequestHandler<RewardPaymentQuery, RewardPaymentViewModel>,
    IRequestHandler<GetRewardPaymentDataQuery, RewardPayment>,
    IRequestHandler<UpdateRewardPaymentStatusCommand, RewardResult>,
    IRequestHandler<RewardDetailQuery, RewardDetailViewModel>,
    IRequestHandler<GetRewardDetailDataQuery, RewardDetailDataViewModel>
{
    private readonly IRewardService rewardService = rewardService;
    private readonly ICustomerService customerService = customerService;
    private readonly UsernameSuggestionService usernameSuggestionService = usernameSuggestionService;

    #region Reward Payment
    public async Task<RewardPaymentViewModel> Handle(RewardPaymentQuery message, CancellationToken cancellation)
    {
        var programs = await rewardService.GetRewardPrograms();
        if (string.IsNullOrWhiteSpace(message.ProgramIds))
        {
            message.ProgramIds = string.Join(',', programs.Select(p => p.ResourceId));
        }

        return new RewardPaymentViewModel(message, programs, await GetRewardPayments(message));
    }

    public Task<RewardPayment> Handle(GetRewardPaymentDataQuery message, CancellationToken cancellation)
    {
        if (!string.IsNullOrWhiteSpace(message.Username) && !message.IsInternal)
        {
            UpdateUserSearchInRewardPaymentTab(message.Username, message.ViewerId, message.ActorLevel);
        }

        return GetRewardPayments(message);
    }

    private async Task<RewardPayment> GetRewardPayments(RewardBaseQuery message)
    {
        var reportData = await rewardService.GetRewardPayments(GetRewardParam(message));
        reportData.ActorLevel = message.ActorLevel;

        return reportData;
    }

    private void UpdateUserSearchInRewardPaymentTab(string username, int viewerId, UserLevel actorLevel)
    {
        Task.Run(async () =>
        {
            try
            {
                var custInfo = await customerService.GetCustInfo(username, viewerId);

                if (custInfo != null && actorLevel.IsDirectUplineOf(custInfo.RoleId))
                {
                    await usernameSuggestionService.UpdateUserSearch(viewerId, custInfo.Username, custInfo.FirstName, custInfo.LastName, custInfo.Username2);
                }
            }
            catch (Exception ex)
            {
                await Fanex.Logging.Logger.Log.ErrorAsync(ex.Message, ex);
            }
        });
    }

    public async Task<RewardResult> Handle(UpdateRewardPaymentStatusCommand message, CancellationToken cancellation)
    {
        var isValidCheckMultipleCustomersBelonging = await CheckMultipleCustomersBelonging(message);

        return isValidCheckMultipleCustomersBelonging
            ? await rewardService.UpdateRewardPaymentStatus(new UpdateRewardPaymentStatusParam(
                message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
                message.ToDate,
                message.Data,
                message.ViewerId,
                message.SubAccId,
                message.SubAccName,
                message.IP,
                message.LanguageKey,
                message.AGIP))
            : new RewardResult(isKickout: true);
    }

    private async Task<bool> CheckMultipleCustomersBelonging(UpdateRewardPaymentStatusCommand message)
    {
        var normalCustIds = new List<int>();
        var directMemberCustIds = new List<int>();

        foreach (var item in message.Data)
        {
            if (item.IsDirectMember)
            {
                directMemberCustIds.Add(item.CustId);
            }
            else
            {
                normalCustIds.Add(item.CustId);
            }
        }

        var belongingNormalCustTask = normalCustIds.Count > 0
            ? customerService.CheckMultipleBelonging(message.ViewerId, message.SubAccId, string.Join("^", normalCustIds))
            : Task.FromResult(true);
        var belongingDirectMemberTask = directMemberCustIds.Count > 0
            ? customerService.CheckMultipleBelonging(message.ViewerId, message.SubAccId, string.Join("^", directMemberCustIds))
            : Task.FromResult(true);
        await Task.WhenAll(belongingNormalCustTask, belongingDirectMemberTask);

        return await belongingNormalCustTask && await belongingDirectMemberTask;
    }
    #endregion

    #region Reward Detail
    public async Task<RewardDetailViewModel> Handle(RewardDetailQuery message, CancellationToken cancellation)
    {
        var programs = await rewardService.GetRewardPrograms();
        if (string.IsNullOrWhiteSpace(message.ProgramIds))
        {
            message.ProgramIds = string.Join(",", programs.Select(p => p.ResourceId));
        }

        var reportData = await rewardService.GetRewardDetails(GetRewardParam(message));

        return new RewardDetailViewModel(message, programs, reportData);
    }

    public async Task<RewardDetailDataViewModel> Handle(GetRewardDetailDataQuery message, CancellationToken cancellation)
    {
        if (!string.IsNullOrWhiteSpace(message.Username) && !message.IsInternal)
        {
            UpdateUserSearchInRewardDetailTab(message.Username, message.ViewerId, message.ActorLevel);
        }

        var reportData = await rewardService.GetRewardDetails(GetRewardParam(message));

        return new RewardDetailDataViewModel(message.ActorLevel, reportData);
    }

    private void UpdateUserSearchInRewardDetailTab(string username, int viewerId, UserLevel actorLevel)
    {
        Task.Run(async () =>
        {
            try
            {
                var custInfo = await customerService.GetCustInfo(username, viewerId);

                if (custInfo != null)
                {
                    var nickname = actorLevel.IsDirectUplineOf(custInfo.RoleId)
                        ? custInfo.Username2
                        : string.Empty;
                    await usernameSuggestionService.UpdateUserSearch(viewerId, custInfo.Username, custInfo.FirstName, custInfo.LastName, nickname);
                }
            }
            catch (Exception ex)
            {
                await Fanex.Logging.Logger.Log.ErrorAsync(ex.Message, ex);
            }
        });
    }
    #endregion

    internal static RewardParam GetRewardParam(RewardBaseQuery message)
        => new RewardParam(
            message.ViewerId,
            message.ActorLevel,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            message.ProgramIds,
            message.PaymentStatus,
            message.LevelType,
            message.Username,
            message.PageIndex,
            message.PageSize);
}