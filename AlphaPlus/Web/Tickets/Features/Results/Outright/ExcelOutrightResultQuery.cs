using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.Outright;

public class ExcelOutrightResultQuery : OutrightResultQuery, IRequest<ResultExcelViewModel>
{
}