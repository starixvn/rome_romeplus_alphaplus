using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Consolidation;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationHandler(
    IProductService productService,
    IOptions<AppSettings> options,
    IEnteredResultService enteredResultService,
    IConsolidationService consolidationService,
    IConsolidationByUsernameWorkbook usernameWorkbook,
    IConsolidationWorkbook dateOrProductWorkbook) :
        ConsolidationBaseHandler(productService, options, enteredResultService, consolidationService),
        IRequestHandler<ConsolidationByUsernameQuery, ConsolidationByUsernameViewModel>,
        IRequestHandler<ConsolidationByDateQuery, ConsolidationByDateViewModel>,
        IRequestHandler<ConsolidationByProductQuery, ConsolidationByProductViewModel>,
        IRequestHandler<ConsolidationExcelQuery, ExcelFileViewModel>
{
    public async Task<ConsolidationByUsernameViewModel> Handle(ConsolidationByUsernameQuery message, CancellationToken cancellationToken)
    {
        var (productViewModel, currencies, enteredResult, reportData) = await GetConsolidationByUsername(message);

        return new ConsolidationByUsernameViewModel(message, productViewModel, currencies, enteredResult, reportData);
    }

    public async Task<ConsolidationByDateViewModel> Handle(ConsolidationByDateQuery message, CancellationToken cancellationToken)
    {
        var (productViewModel, currencies, enteredResult, reportData) = await GetConsolidationByDateOrProduct(message);

        return new ConsolidationByDateViewModel(
            message,
            productViewModel,
            currencies,
            enteredResult,
            reportData.ToList());
    }

    public async Task<ConsolidationByProductViewModel> Handle(ConsolidationByProductQuery message, CancellationToken cancellationToken)
    {
        var (productViewModel, currencies, enteredResult, reportData) = await GetConsolidationByDateOrProduct(message);

        return new ConsolidationByProductViewModel(
            message,
            productViewModel,
            currencies,
            enteredResult,
            reportData.ToList());
    }

    public async Task<ExcelFileViewModel> Handle(ConsolidationExcelQuery message, CancellationToken cancellationToken)
    {
        if (message.ReportType == ConsolidationReportType.ByUsername)
        {
            var (consolidationByUsername, productNamesByUsername, currenciesByUsername) = await GetConsolidationByUsernameExcel(message);

            return usernameWorkbook.BuildExcelFile(message, consolidationByUsername, productNamesByUsername, currenciesByUsername);
        }

        var (consolidationByDateOrProduct, productNamesByDateOrProduct, currenciesByDateOrProduct) = await GetConsolidationByDateOrProductExcel(message);

        return dateOrProductWorkbook.BuildExcelFile(message, consolidationByDateOrProduct, productNamesByDateOrProduct, currenciesByDateOrProduct);
    }
}
