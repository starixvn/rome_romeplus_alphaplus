using System.Diagnostics.CodeAnalysis;
using System.Net;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.BetChoiceMatching;
using AlphaPlus.Reports.Site.Features.BetCreditTracking;
using AlphaPlus.Reports.Site.Features.CommissionByBetType;
using AlphaPlus.Reports.Site.Features.Consolidation;
using AlphaPlus.Reports.Site.Features.Customer;
using AlphaPlus.Reports.Site.Features.EnteredResult;
using AlphaPlus.Reports.Site.Features.IPMatching;
using AlphaPlus.Reports.Site.Features.MatchWinLossDetail;
using AlphaPlus.Reports.Site.Features.ProgressiveContribution;
using AlphaPlus.Reports.Site.Features.Statement;
using AlphaPlus.Reports.Site.Features.SuperWinLossDetailByPT;
using AlphaPlus.Reports.Site.Features.WhiteLabel;
using AlphaPlus.Reports.Site.Features.WinLoss;
using AlphaPlus.Reports.Site.Features.WinLossAnalysis;
using AlphaPlus.Reports.Site.Features.WinLossByProduct;
using AlphaPlus.Reports.Site.Features.WinLossDetail;
using AlphaPlus.SecurityCode.Extensions;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.RemotingRef;
using Fanex.BetList.Core.TicketHelper;

namespace AlphaPlus.Reports.Site.Features.Shared.Extensions;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        => services
        .AddSecurityCodeServices(configuration)
        .AddReportsServices(configuration, UserPermission.IsShowOrNotExistProduct)
        .AddTransient<IEnteredResultWorkbook, EnteredResultWorkbook>()
        .AddTransient<IWinLossWorkbook, WinLossWorkbook>()
        .AddTransient<IWinLossDetailWorkbook, WinLossDetailWorkbook>()
        .AddTransient<IMatchWinLossDetailWorkbook, MatchWinLossDetailWorkbook>()
        .AddTransient<IWinLossAnalysisWorkbook, WinLossAnalysisWorkbook>()
        .AddTransient<IWinLossByProductWorkbook, WinLossByProductWorkbook>()
        .AddTransient<ISuperWinLossDetailByWorkbook, SuperWinLossDetailByWorkbook>()
        .AddTransient<ICommissionByBetTypeWorkbook, CommissionByBetTypeWorkbook>()
        .AddTransient<IConsolidationByUsernameWorkbook, ConsolidationByUsernameWorkbook>()
        .AddTransient<IConsolidationWorkbook, ConsolidationWorkbook>()
        .AddTransient<IConsolidationDetailWorkbook, ConsolidationDetailWorkbook>()
        .AddTransient<ISMAStatementWorkbook, SMAStatementWorkbook>()
        .AddTransient<IStatementDetailWorkbook, StatementDetailWorkbook>()
        .AddTransient<IMemberStatementWorkbook, MemberStatementWorkbook>()
        .AddTransient<IProgressiveContributionWookbook, ProgressiveContributionWookbook>()
        .AddTransient<IBetCreditHistoryWorkbook, BetCreditHistoryWorkbook>()
        .AddTransient<IMoveStatementHistoryWorkbook, MoveStatementHistoryWorkbook>()
        .AddTransient<IIPMatchingWorkbook, IPMatchingWorkbook>()
        .AddTransient<IBetChoiceMatchingWorkbook, BetChoiceMatchingWorkbook>()
        .AddTransient<ISettlementWorkbook, SettlementWorkbook>()
        .AddTransient<IDailyWinLossWorkbook, DailyWinLossWorkbook>()
        .AddBetListExcelServices()
        .AddHttpClient<ICustomerClient, CustomerClient>(configuration.GetValue<string>("Fanex.Customer:ServiceUri"))
        .AddTransient<ICashFeeWorkbook, CashFeeWorkbook>();

    private static IServiceCollection AddBetListExcelServices(this IServiceCollection services)
        => services
        .AddSingleton<ICacheRef, CacheRef>()
        .AddScoped<ITicketHelper, TicketHelper>()
        .AddScoped<IBetListFactory<BetListExcelBuilder>, BetListExcelFactory>()
        .AddScoped<IBetListExcelBuilder>(sp => sp.GetRequiredService<IBetListFactory<BetListExcelBuilder>>().CreateBuilder());

    private static IServiceCollection AddHttpClient<TClient, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(
        this IServiceCollection services,
        string baseUrl,
        int pooledConnectionLifetime = 15)
            where TClient : class
            where TImplementation : class, TClient
    {
        services
            .AddHttpClient<TClient, TImplementation>(httpClient => httpClient.BaseAddress = new Uri(baseUrl))
            .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            })
            .ConfigurePrimaryHttpMessageHandler(_ => new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(pooledConnectionLifetime)
            })
            .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        return services;
    }
}
