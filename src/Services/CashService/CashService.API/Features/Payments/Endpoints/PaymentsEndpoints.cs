namespace AlphaPlus.CashService.API.Features.Payments.Endpoints;

public static class PaymentsEndpoints
{
    public static void MapPaymentsEndpoints(this WebApplication app)
    {
        PaymentsProviderEndpoints.MapEndpoints(app);
        PaymentsBankAccountEndpoints.MapEndpoints(app);
    }
} 