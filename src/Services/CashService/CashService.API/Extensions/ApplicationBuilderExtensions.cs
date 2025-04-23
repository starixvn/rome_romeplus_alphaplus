using AlphaPlus.CashService.API.Features.Shared.Endpoints;
using AlphaPlus.CashService.API.Endpoints;
using Microsoft.AspNetCore.Diagnostics;

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

        return app;
    }
}