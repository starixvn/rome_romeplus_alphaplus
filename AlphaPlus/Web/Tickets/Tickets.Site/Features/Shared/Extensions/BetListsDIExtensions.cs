using AlphaPlus.Tickets.Site.Features.BetLists.Handlers.Parlay;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.RemotingRef;
using Fanex.BetList.Core.TicketHelper;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.BetListNPOI;

namespace AlphaPlus.Tickets.Site.Features.Shared.Extensions;

public static class BetListsDIExtensions
{
    public static IServiceCollection AddBetListsCore(this IServiceCollection services)
        => services
        .AddSingleton<ICacheRef, CacheRef>()
        .AddSingleton<ITicketHelper, TicketHelper>()
        .AddScoped(typeof(Workbook<>), typeof(NpoiWorkbook<>))
        .AddSingleton<IBetListFactory<BetListHTMLBuilder>, BetListHTMLFactory>()
        .AddSingleton<IBetListFactory<BetListExcelBuilder>, BetListExcelFactory>()
        .AddSingleton<IBetListHTMLBuilder>(sp => sp.GetService<IBetListFactory<BetListHTMLBuilder>>().CreateBuilder())
        .AddSingleton<IBetListExcelBuilder>(sp => sp.GetService<IBetListFactory<BetListExcelBuilder>>().CreateBuilder())
        .AddSingleton<IComboParlayDetailWorkbook, ComboParlayDetailWorkbook>();
}
