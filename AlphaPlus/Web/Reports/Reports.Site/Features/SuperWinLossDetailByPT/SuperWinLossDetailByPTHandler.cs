using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.Features.SuperWinLossDetailByPT;

public class SuperWinLossDetailByPTHandler
    (IProductService productService,
    IOptions<AppSettings> options) :
        BaseHandler(productService, options),
        IRequestHandler<SuperWinLossDetailByPTQuery, SuperWinLossDetailByPTViewModel>,
        IRequestHandler<SuperWinLossDetailByPTExcelQuery, ExcelFileViewModel>
{
    public Task<SuperWinLossDetailByPTViewModel> Handle(SuperWinLossDetailByPTQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ExcelFileViewModel> Handle(SuperWinLossDetailByPTExcelQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}