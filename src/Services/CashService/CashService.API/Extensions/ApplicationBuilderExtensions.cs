using AlphaPlus.CashService.API.Features.MemberSites.Endpoints;
using AlphaPlus.CashService.API.Features.Notifications.Endpoints;
using AlphaPlus.CashService.API.Features.Payments.Endpoints;
using AlphaPlus.CashService.API.Features.PendingDeposit.Endpoints;
using AlphaPlus.CashService.API.Features.PendingWithdrawal.Endpoints;
using AlphaPlus.CashService.API.Features.Promotion.Endpoints;
using AlphaPlus.CashService.API.Features.Shared.Endpoints;
using AlphaPlus.CashService.API.Features.Transactions.Endpoints;

namespace AlphaPlus.CashService.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication UseCashServiceSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CashService API V1");
                c.RoutePrefix = string.Empty;
            });
        }

        return app;
    }

    public static WebApplication UseCashServiceExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context => await ErrorHandlingMiddleware.HandleError(context, app));
        });

        return app;
    }

    public static WebApplication UseCashServiceCors(this WebApplication app)
    {
        app.UseCors("AllowSpecificOrigins");
        return app;
    }

    public static WebApplication MapApiEndpoints(this WebApplication app)
    {
        app.MapSystemEndpoints();
        app.MapCommonEndpoints();
        app.MapPaymentsEndpoints();
        app.MapMemberSiteManagementEndpoints();
        app.MapPendingDepositEndpoints();
        app.MapPendingWithdrawalEndpoints();
        app.MapTransactionsHistoryEndpoints();
        app.MapPromotionEndpoints();
        app.MapNotificationsEndpoints();

        return app;
    }
}