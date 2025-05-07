namespace AlphaPlus.CashService.API.Endpoints;

public static class CommonEndpoints
{
    public static void MapPendingCommonEndpoints(this WebApplication app)
    {
        app.MapPendingDepositEndpoints();
        app.MapPendingWithdrawalEndpoints();
    }
}