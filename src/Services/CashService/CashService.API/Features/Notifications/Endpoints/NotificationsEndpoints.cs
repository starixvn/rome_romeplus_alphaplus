using AlphaPlus.CashService.API.Features.Notifications.Services;

namespace AlphaPlus.CashService.API.Features.Notifications.Endpoints;

public static class NotificationsEndpoints
{
    public static void MapNotificationsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/notifications")
            .WithTags("Notifications");

        group.MapGet("/GetPendingCount", async (int custId, INotificationsService service) =>
            Results.Ok(await service.GetPendingCount(custId))
        ).WithName("PendingCount");
    }
}