using AlphaPlus.CashService.API.Models.PendingCommons;
using AlphaPlus.CashService.API.Services;
using Newtonsoft.Json;

namespace AlphaPlus.CashService.API.Endpoints;

public static class PendingCommonEndpoints
{
    public static void MapPendingCommonEndpoints(this WebApplication app)
    {
        app.MapPendingDepositEndpoints();
        app.MapPendingWithdrawalEndpoints();
    }
}