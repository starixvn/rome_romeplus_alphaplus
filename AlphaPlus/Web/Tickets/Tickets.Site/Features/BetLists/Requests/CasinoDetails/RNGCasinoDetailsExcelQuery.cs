using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.CasinoDetails;
public class RNGCasinoDetailsExcelQuery : CasinoDetailsExcelQuery, IRequest<ExcelFileViewModel>;