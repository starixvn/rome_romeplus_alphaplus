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
        .AddScoped<ITicketHelper, TicketHelper>()
        .AddScoped(typeof(Workbook<>), typeof(NpoiWorkbook<>))
        .AddScoped<IBetListFactory<BetListHTMLBuilder>, BetListHTMLFactory>()
        .AddScoped<IBetListFactory<BetListExcelBuilder>, BetListExcelFactory>()
        .AddScoped<IBetListHTMLBuilder>(sp => sp.GetService<IBetListFactory<BetListHTMLBuilder>>().CreateBuilder())
        .AddScoped<IBetListExcelBuilder>(sp => sp.GetService<IBetListFactory<BetListExcelBuilder>>().CreateBuilder())
        .AddSingleton<IComboParlayDetailWorkbook, ComboParlayDetailWorkbook>();
}
