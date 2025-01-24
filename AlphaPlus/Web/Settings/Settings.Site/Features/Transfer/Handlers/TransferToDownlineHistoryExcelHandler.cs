using AlphaPlus.Shared.AppResources.Settings.Transfer;
using AlphaPlus.Settings.Transfer.API.Extensions;
using AlphaPlus.Settings.Transfer.API.Models.TransferToDownlineHistory;
using AlphaPlus.Settings.Transfer.API.Params.TransferToDownlineHistory;
using AlphaPlus.Settings.Transfer.API.Services;
using MediatR;
using ONEbook.SpreadSheets;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferToDownlineHistory;
using AlphaPlus.Core.Utils;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers
{
    public class TransferToDownlineHistoryExcelHandler : TransferToDownlineHistoryBaseHandler, IRequestHandler<TransferToDownlineHistoryExcelQuery, ExcelFileViewModel>
    {
        private readonly Workbook<TransferToDownlineHistoryModel> workbook;

        public TransferToDownlineHistoryExcelHandler(
            TransferToDownlineHistoryService transferToDownlineHistoryService,
            Workbook<TransferToDownlineHistoryModel> workbook)
            : base(transferToDownlineHistoryService)
        {
            this.workbook = workbook;
        }

        public async Task<ExcelFileViewModel> Handle(TransferToDownlineHistoryExcelQuery message, CancellationToken cancellation)
        {
            var param = new TransferToDownlineHistoryPagingParam(
                message.CustId,
                message.UserName,
                message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
                message.ToDate,
                0,
                0);
            var content = workbook
                .UseTemplate(GetTemplateInfo(message))
                .Title(GetSheetTitle(param))
                .Header(Resources.ResourceManager.GetString)
                .Body((await GetTransferToDownlineHistory(param)).Item1, GetBodyMapper)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(), content);
        }

        protected internal Mapper<TransferToDownlineHistoryModel> GetBodyMapper
            => new Mapper<TransferToDownlineHistoryModel>()
            .Map(Column.A, m => m.FormattedActionDate)
            .Map(Column.B, m => m.DownlineName)
            .Map(Column.C, m => m.TransferTypeName)
            .Map(Column.D, m => m.FormattedTransferAmount)
            .Map(Column.E, m => m.FormattedOldBalance)
            .Map(Column.F, m => m.FormattedNewBalance)
            .Map(Column.G, m => m.Description);

        private TemplateInfo GetTemplateInfo(TransferToDownlineHistoryExcelQuery message)
            => new(
                GetExcelTemplatePath($"TransferToDownlineHistory/{message.CustLevel.GetDirectDownLineLevel().DisplayName}"),
                Resources.NoInformationIsAvailable);

        private static string GetSheetTitle(TransferToDownlineHistoryParam param)
        {
            string userName = string.IsNullOrWhiteSpace(param.UserName)
                ? string.Empty
                : string.Format(" - Username: {0}", param.UserName);

            return $"{Resources.TransferToDownlineHistory} {param.FromDate.ToShortDateWithSlash()} -> {param.ToDate.ToShortDateWithSlash()}{userName}";
        }

        private static string GetExcelFileName() => $"{Resources.TransferToDownlineHistory} - {SystemTime.Today.ToShortDateString()}";
    }
}