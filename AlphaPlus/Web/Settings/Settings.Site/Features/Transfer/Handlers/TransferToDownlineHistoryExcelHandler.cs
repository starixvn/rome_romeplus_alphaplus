using AlphaPlus.Shared.AppResources.Settings.Transfer;
using AlphaPlus.Settings.Transfer.API.Extensions;
using AlphaPlus.Settings.Transfer.API.Models.TransferToDownlineHistory;
using AlphaPlus.Settings.Transfer.API.Services;
using MediatR;
using ONEbook.SpreadSheets;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferToDownlineHistory;
using AlphaPlus.Core.Utils;
using AlphaPlus.Core.Models;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers;

public class TransferToDownlineHistoryExcelHandler(
    TransferToDownlineHistoryService transferToDownlineHistoryService,
    Workbook<TransferToDownlineHistoryModel> workbook) :
        TransferToDownlineHistoryBaseHandler(transferToDownlineHistoryService), IRequestHandler<TransferToDownlineHistoryExcelQuery, ExcelFileViewModel>
{
    private readonly Workbook<TransferToDownlineHistoryModel> workbook = workbook;

    public async Task<ExcelFileViewModel> Handle(TransferToDownlineHistoryExcelQuery message, CancellationToken cancellation)
    {
        var content = workbook
            .UseTemplate(GetTemplateInfo(message))
            .Title(GetSheetTitle(message))
            .Header(Resources.ResourceManager.GetString)
            .Body((await GetTransferToDownlineHistory(message)).Item1, GetBodyMapper(message.CustLevel))
            .ExportToBytes();

        return new ExcelFileViewModel(GetExcelFileName(), content);
    }

    protected internal Mapper<TransferToDownlineHistoryModel> GetBodyMapper(UserLevel custLevel)
    {
        var mapper = new Mapper<TransferToDownlineHistoryModel>()
            .Map(A, m => m.FormattedActionDate)
            .Map(B, m => m.DownlineName);
        if (custLevel.IsAgentLevel())
        {
            mapper
                .Map(C, m => m.TransferTypeName)
                .Map(D, m => m.FormattedTransferAmount)
                .Map(E, m => m.FormattedOldBalance)
                .Map(F, m => m.FormattedNewBalance)
                .Map(G, m => m.Description);
        }
        else
        {
            mapper
                .Map(C, m => m.IsDirectMember ? Resources.DirectMember : (custLevel.IsSuperLevel() ? Resources.MasterAgent : Resources.Agent))
                .Map(D, m => m.TransferTypeName)
                .Map(E, m => m.FormattedTransferAmount)
                .Map(F, m => m.FormattedOldBalance)
                .Map(G, m => m.FormattedNewBalance)
                .Map(H, m => m.Description);
        }

        return mapper;
    }

    private TemplateInfo GetTemplateInfo(TransferToDownlineHistoryExcelQuery message)
        => new(
            GetExcelTemplatePath($"TransferToDownlineHistory/{message.CustLevel.GetDirectDownLineLevel().DisplayName}"),
            Resources.NoInformationIsAvailable);

    private static string GetSheetTitle(TransferToDownlineHistoryExcelQuery message)
    {
        string userName = string.IsNullOrWhiteSpace(message.UserName)
            ? string.Empty
            : string.Format(" - Username: {0}", message.UserName);

        return $"{Resources.TransferToDownlineHistory} {message.FromDate.ToShortDateWithSlash()} -> {message.ToDate.ToShortDateWithSlash()}{userName}";
    }

    private static string GetExcelFileName() => $"{Resources.TransferToDownlineHistory} - {SystemTime.Today.ToShortDateString()}";
}