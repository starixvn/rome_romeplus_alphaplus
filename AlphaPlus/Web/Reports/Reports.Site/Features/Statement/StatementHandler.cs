using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Statement;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Statement;

public class StatementHandler(
    IStatementService statementService,
    ISMAStatementWorkbook smaStatementWorkbook,
    IStatementDetailWorkbook statementDetailWorkbook,
    IMemberStatementWorkbook memberStatementWorkbook) :
        IRequestHandler<SMAStatementQuery, SMAStatementViewModel>,
        IRequestHandler<SMAStatementExcelQuery, ExcelFileViewModel>,
        IRequestHandler<StatementDetailsQuery, StatementDetailViewModel>,
        IRequestHandler<StatementDetailExcelQuery, ExcelFileViewModel>,
        IRequestHandler<MemberStatementQuery, MemberStatementViewModel>,
        IRequestHandler<MemberStatementExcelQuery, ExcelFileViewModel>
{
    public async Task<SMAStatementViewModel> Handle(SMAStatementQuery message, CancellationToken cancellationToken)
        => new SMAStatementViewModel(message, await GetStatements(message));

    public async Task<ExcelFileViewModel> Handle(SMAStatementExcelQuery message, CancellationToken cancellationToken)
    {
        var statements = await GetStatements(message);

        return smaStatementWorkbook.BuildExcelFile(
            message.CustLevel,
            message.CustName,
            message.FromDate,
            message.ToDate,
            statements);
    }

    public async Task<StatementDetailViewModel> Handle(StatementDetailsQuery message, CancellationToken cancellationToken)
    {
        var reportData = await statementService.StatementDetails(message.CustId, message.TransferDate);

        return new StatementDetailViewModel(message, reportData);
    }

    public async Task<ExcelFileViewModel> Handle(StatementDetailExcelQuery message, CancellationToken cancellationToken)
    {
        var reportData = await statementService.StatementDetails(message.CustId, message.TransferDate);

        return statementDetailWorkbook.BuildExcelFile(message, reportData);
    }

    public async Task<MemberStatementViewModel> Handle(MemberStatementQuery message, CancellationToken cancellationToken)
        => new MemberStatementViewModel(message, await GetStatements(message));

    public async Task<ExcelFileViewModel> Handle(MemberStatementExcelQuery message, CancellationToken cancellationToken)
    {
        var statements = await GetStatements(message);

        return memberStatementWorkbook.BuildExcelFile(
            message.CustLevel,
            message.CustName,
            message.FromDate,
            message.ToDate,
            statements);
    }

    private Task<IEnumerable<StatementModel>> GetStatements(SMAStatementQuery message)
    {
        var param = new GetStatementParam(
            message.CustId,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            message.Site);

        return statementService.GetStatements(param);
    }
}
