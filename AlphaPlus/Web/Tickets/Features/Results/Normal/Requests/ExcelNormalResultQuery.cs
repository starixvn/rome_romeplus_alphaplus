using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class ExcelNormalResultQuery : NormalResultQuery, IRequest<ExcelFileViewModel>
{
    public string TemplatePath { get; set; }

    public string ExcelTitle { get; set; }
}