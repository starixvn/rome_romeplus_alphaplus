using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class ExcelNormalResultQuery : NormalResultQuery, IRequest<ResultExcelViewModel>
{
    public string TemplatePath { get; set; }

    public string ExcelTitle { get; set; }
}