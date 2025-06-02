using ONEbook.UM.Services;

namespace AgencyCash.APIGateway.Shared;

public sealed class UMMiddleware(RequestDelegate next, IMaintenanceService maintenanceService)
{
    private readonly RequestDelegate next = next;
    private readonly IMaintenanceService maintenanceService = maintenanceService;

    public async Task InvokeAsync(HttpContext context)
    {
        var umInfo = await maintenanceService.GetUnderMaintenanceTimeAsync();

        if (umInfo.IsUnderMaintenanceTime)
        {
            var error = $"The system is currently under maintenance. Maintenance time: {umInfo.From} - {umInfo.To} (GMT+8). Please try again later.";
            await WriteError(context.Response, StatusCodes.Status503ServiceUnavailable, error);
            return;
        }

        await next(context);
    }

    private static Task WriteError(HttpResponse response, int statusCode, string error)
    {
        response.ContentType = "application/json";
        response.StatusCode = statusCode;
        return response.WriteAsync(error);
    }
}
