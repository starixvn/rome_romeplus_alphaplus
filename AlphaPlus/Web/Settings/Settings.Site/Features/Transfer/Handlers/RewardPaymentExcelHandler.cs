using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.Reward;
using AlphaPlus.Settings.Transfer.API.Models.Reward;
using AlphaPlus.Settings.Transfer.API.Services;
using AlphaPlus.Shared.AppResources.Common;
using AlphaPlus.Shared.AppResources.Settings.Transfer;
using MediatR;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers;

public class RewardPaymentExcelHandler(
    IRewardService rewardService,
    Workbook<RewardPaymentEntity> workbook) : ExcelBaseHandler, IRequestHandler<RewardPaymentExcelQuery, ExcelFileViewModel>
{
    private readonly IRewardService rewardService = rewardService;
    private readonly Workbook<RewardPaymentEntity> workbook = workbook;

    public async Task<ExcelFileViewModel> Handle(RewardPaymentExcelQuery message, CancellationToken cancellation)
    {
        var isMasterOrSuper = message.ActorLevel.IsMasterOrSuperLevel();
        var levelTypeValue = message.ActorLevel.IsSuperLevel() ? CommonResx.MasterAgent : Resources.Agent;
        var reportData = await rewardService.GetRewardPayments(RewardHandler.GetRewardParam(message));

        var content = workbook
            .UseTemplate(GetTemplateInfo(message.ActorLevel))
            .Title(GetReportTitle(message))
            .Header(Resources.ResourceManager.GetString)
            .Body(reportData.RewardPayments, isMasterOrSuper ? GetRowMapper(levelTypeValue, false) : GetRowMapperMember(false))
            .Footer(reportData.Total, isMasterOrSuper ? GetRowMapper(levelTypeValue, true) : GetRowMapperMember(true))
            .ExportToBytes();

        return new ExcelFileViewModel(GetExcelFileName(message), content);
    }

    private TemplateInfo GetTemplateInfo(UserLevel actorLevel)
        => new TemplateInfo(
            GetExcelTemplatePath($"Reward/RewardPayment{actorLevel.GetDirectDownLineLevel().DisplayName}"),
            Resources.NoInformationIsAvailable,
            bodyStartIndex: 3,
            bodyEndIndex: 4);

    private static string GetReportTitle(RewardPaymentExcelQuery message)
        => $"{Resources.RewardPayment} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}";

    internal static Mapper<RewardPaymentEntity> GetRowMapper(string levelTypeValue, bool isTotal)
    {
        var mapper = new Mapper<RewardPaymentEntity>();

        if (isTotal)
        {
            mapper
                .Map(A, rp => Resources.Total)
                .Map(D, rp => rp.Amount);
        }
        else
        {
            mapper
                .Map(A, rp => rp.ProgramName)
                .Map(B, rp => rp.Username)
                .Map(C, rp => rp.IsDirectMember ? CommonResx.DirectMember : levelTypeValue)
                .Map(D, rp => rp.Amount)
                .Map(E, rp =>
                {
                    if (rp.PaymentDate.HasValue)
                    {
                        return rp.PaymentDate.Value;
                    }

                    return string.Empty;
                })
                .Map(F, rp => rp.PaymentStatus ? Resources.Paid : Resources.Unpaid);
        }

        return mapper;
    }

    internal static Mapper<RewardPaymentEntity> GetRowMapperMember(bool isTotal)
    {
        var mapper = new Mapper<RewardPaymentEntity>();

        if (isTotal)
        {
            mapper
                .Map(A, rp => Resources.Total)
                .Map(C, rp => rp.Amount);
        }
        else
        {
            mapper
                .Map(A, rp => rp.ProgramName)
                .Map(B, rp => rp.Username)
                .Map(C, rp => rp.Amount)
                .Map(D, rp =>
                {
                    if (rp.PaymentDate.HasValue)
                    {
                        return rp.PaymentDate.Value;
                    }

                    return string.Empty;
                })
                .Map(E, rp => rp.PaymentStatus ? Resources.Paid : Resources.Unpaid);
        }

        return mapper;
    }

    private string GetExcelFileName(RewardPaymentExcelQuery message)
       => string.Join(
            '-',
            Resources.RewardPayment,
            message.CustName,
            message.FromDate.ToShortDateWithSlash(),
            message.ToDate.ToShortDateWithSlash());
}