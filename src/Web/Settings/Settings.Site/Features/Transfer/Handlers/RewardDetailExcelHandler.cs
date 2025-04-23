using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.Reward;
using AlphaPlus.Settings.Transfer.API.Models.Reward;
using AlphaPlus.Settings.Transfer.API.Services;
using AlphaPlus.Shared.AppResources.Settings.Transfer;
using MediatR;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers;

public class RewardDetailExcelHandler(
    IRewardService rewardService,
    Workbook<RewardDetailEntity> workbook) : ExcelBaseHandler, IRequestHandler<RewardDetailExcelQuery, ExcelFileViewModel>
{
    private readonly IRewardService rewardService = rewardService;
    private readonly Workbook<RewardDetailEntity> workbook = workbook;

    public async Task<ExcelFileViewModel> Handle(RewardDetailExcelQuery message, CancellationToken cancellation)
    {
        var reportData = await rewardService.GetRewardDetails(RewardHandler.GetRewardParam(message));

        var content = workbook
            .UseTemplate(GetTemplateInfo(message.ActorLevel))
            .Title(GetReportTitle(message))
            .Header(Resources.ResourceManager.GetString)
            .Body(reportData.RewardDetails, GetRowMapper(message.ActorLevel))
            .ExportToBytes();

        return new ExcelFileViewModel(GetExcelFileName(message), content);
    }

    private TemplateInfo GetTemplateInfo(UserLevel actorLevel)
        => new TemplateInfo(
            GetExcelTemplatePath($"Reward/RewardDetail{actorLevel.GetDirectDownLineLevel().DisplayName}"),
            Resources.NoInformationIsAvailable);

    private static string GetReportTitle(RewardDetailExcelQuery message)
        => $"{Resources.RewardDetail} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}";

    internal static Mapper<RewardDetailEntity> GetRowMapper(UserLevel actorLevel)
    {
        var mapper = new Mapper<RewardDetailEntity>()
            .Map(A, rp => rp.CreatedDate)
            .Map(B, rp => rp.ProgramName)
            .Map(C, rp => rp.MemberUsername)
            .Map(D, rp => rp.MemberAmount)
            .Map(E, rp => rp.MemberPaymentStatus ? Resources.Paid : Resources.Unpaid)
            .Map(F, rp =>
            {
                if (rp.MemberPaymentDate.HasValue)
                {
                    return rp.MemberPaymentDate.Value;
                }

                return string.Empty;
            })
            .Map(G, rp => rp.MemberFeedbackText);

        if (actorLevel.IsMasterOrSuperLevel())
        {
            mapper
                .Map(H, rp => rp.IsDirectMember || string.IsNullOrWhiteSpace(rp.AgentUsername) ? string.Empty : rp.AgentUsername)
                .Map(I, rp => rp.IsDirectMember || string.IsNullOrWhiteSpace(rp.AgentUsername) ? string.Empty : (rp.AgentPaymentStatus ? Resources.Paid : Resources.Unpaid))
                .Map(J, rp =>
                {
                    if (rp.IsDirectMember || string.IsNullOrWhiteSpace(rp.AgentUsername) || !rp.AgentPaymentDate.HasValue)
                    {
                        return string.Empty;
                    }

                    return rp.AgentPaymentDate.Value;
                });
        }

        if (actorLevel.IsSuperLevel())
        {
            mapper
                .Map(K, rp => rp.IsDirectMember ? string.Empty : rp.MasterUsername)
                .Map(L, rp => rp.IsDirectMember ? string.Empty : (rp.MasterPaymentStatus ? Resources.Paid : Resources.Unpaid))
                .Map(M, rp =>
                {
                    if (!rp.IsDirectMember && rp.MasterPaymentDate.HasValue)
                    {
                        return rp.MasterPaymentDate.Value;
                    }

                    return string.Empty;
                });
        }

        return mapper;
    }

    private string GetExcelFileName(RewardDetailExcelQuery message)
       => string.Join(
            '-',
            Resources.RewardDetail,
            message.CustName,
            message.FromDate.ToShortDateWithSlash(),
            message.ToDate.ToShortDateWithSlash());
}